using Dot.Core.Dispose;
using Dot.Core.Generic;
using Dot.Log;
using Dot.Net.Message;
using Dot.Pool;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Dot.Net.Server
{
    public delegate void OnMessageHandler(long netID,int messageID,object message);

    public class ServerNetMessageData : IObjectPoolItem
    {
        public long netID = -1;
        public int messageID = -1;
        public object message = null;

        public void OnGet()
        {
        }

        public void OnNew()
        {
            
        }

        public void OnRelease()
        {
            netID = -1;
            messageID = -1;
            message = null;
        }
    }

    public class ServerNetListener : IDispose
    {
        private ManualResetEvent allDone = new ManualResetEvent(false);

        private ObjectPool<ServerNetMessageData> dataPool = new ObjectPool<ServerNetMessageData>();
        private object dataListLock = new object();
        private List<ServerNetMessageData> dataList = new List<ServerNetMessageData>();

        private Socket socket = null;
        private UniqueID idCreator = new UniqueID();

        private object netDicLock = new object();
        private Dictionary<long, ServerNet> netDic = new Dictionary<long, ServerNet>();
        private Dictionary<int, OnMessageHandler> handlerDic = new Dictionary<int, OnMessageHandler>();

        private IServerNetCreator netCreator = null;

        public ServerNetListener(IServerNetCreator creator)
        {
            netCreator = creator;
        }

        public void Startup(string ip,int port,int maxCount)
        {
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            LogUtil.LogInfo(ServerNetConst.LOGGER_NAME, $"ServerNetListener::Startup->address = {ipAddress.ToString()}");

            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(maxCount);

                Thread thread = new Thread(() =>
                {
                    while (true)
                    {
                        allDone.Reset();
                        LogUtil.LogInfo("ServerNet", "Waiting for a connection...");

                        listener.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            listener);

                        allDone.WaitOne();
                    }
                });
                thread.Start();
            }
            catch (Exception e)
            {
                LogUtil.LogInfo("ServerNet", e.ToString());
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            IMessageWriter writer = netCreator.GetMessageWriter();
            IMessageReader reader = netCreator.GetMessageReader();

            long id = idCreator.NextID;

            ServerNet serverNet = new ServerNet(id,handler,writer,reader);
            serverNet.MessageReceived = OnMessageReceived;
            serverNet.NetDisconnected = OnNetDisconnected;

            lock(netDicLock)
            {
                netDic.Add(id, serverNet);
            }
        }

        private void OnMessageReceived(long netID,int messageID,object message)
        {
            if (handlerDic.TryGetValue(messageID, out OnMessageHandler handler))
            {
                handler?.Invoke(netID, messageID, message);
            }else
            {
                LogUtil.LogWarning(ServerNetConst.LOGGER_NAME, $"ServerNetListener::OnMessageReceived->message handler is not found.messageID = {messageID}");
            }
        }

        private void OnNetDisconnected(long id)
        {
            lock(netDicLock)
            {
                if(netDic.ContainsKey(id))
                {
                    netDic.Remove(id);
                }
            }
        }

        internal void DoUpdate(float deltaTime)
        {
        }

        internal void DoLateUpdate()
        {
            lock(netDicLock)
            {
                foreach(var kvp in netDic)
                {
                    kvp.Value.DoLateUpdate();
                }
            }
        }

        public void RegisterHandler(int messageID, OnMessageHandler handler)
        {
            if (!handlerDic.ContainsKey(messageID))
            {
                handlerDic.Add(messageID, handler);
            }
            else
            {
                LogUtil.LogError(ServerNetConst.LOGGER_NAME, $"ServerNet::RegisterHandler->the messageID has been added.messageID={messageID}");
            }
        }

        public void UnregisterHandler(int messageID)
        {
            if (handlerDic.ContainsKey(messageID))
            {
                handlerDic.Remove(messageID);
            }
        }

        public void SendMessage<T>(long netID,int messageID, T msg)
        {
            if(netDic.TryGetValue(netID,out ServerNet serverNet))
            {
                serverNet.SendMessage<T>(messageID, msg);
            }
        }

        public void SendEmptyMessage(long netID, int messageID)
        {
            if (netDic.TryGetValue(netID, out ServerNet serverNet))
            {
                serverNet.SendEmptyMessage(messageID);
            }
        }

        public void SendData(long netID, int messageID, byte[] msg)
        {
            if (netDic.TryGetValue(netID, out ServerNet serverNet))
            {
                serverNet.SendData(messageID,msg);
            }
        }

        public void Dispose()
        {
            allDone.Set();
            dataPool.Clear();
            lock(dataListLock)
            {
                dataList.Clear();
            }
            lock(netDicLock)
            {
                foreach(var kvp in netDic)
                {
                    kvp.Value.Dispose();
                }
                netDic.Clear();
            }
            handlerDic.Clear();
            netCreator = null;

            if (socket != null)
            {
                if (socket.Connected)
                {
                    try
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception e)
                    {
                        LogUtil.LogError(ServerNetConst.LOGGER_NAME, $"ServerNetListener::Disconnect->e = {e.Message}");
                    }
                    finally
                    {
                        socket.Close();
                        socket = null;
                    }
                }
                else
                {
                    socket.Close();
                    socket = null;
                }
            }
        }
    }
}
