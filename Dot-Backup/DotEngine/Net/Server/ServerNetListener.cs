using Dot.Core.Dispose;
using Dot.Generic;
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
    public delegate object ServerMessageParser(int messageID, byte[] msgDatas);
    public delegate void ServerMessageHandler(int netID, int messageID, object message);

    public class ServerNetMessageData : IObjectPoolItem
    {
        public int netID = -1;
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

    public partial class ServerNetListener : IDispose
    {
        private int uniqueID = -1;
        public int UniqueID { get => uniqueID; }

        private ManualResetEvent allDone = new ManualResetEvent(false);

        private ObjectPool<ServerNetMessageData> dataPool = new ObjectPool<ServerNetMessageData>();
        private object dataListLock = new object();
        private List<ServerNetMessageData> dataList = new List<ServerNetMessageData>();

        private Socket socket = null;
        private UniqueIntID clientIDCreator = new UniqueIntID();

        private object netDicLock = new object();
        private Dictionary<int, ServerNet> netDic = new Dictionary<int, ServerNet>();

        private Dictionary<int, ServerMessageParser> messageParserDic = new Dictionary<int, ServerMessageParser>();
        private Dictionary<int, ServerMessageHandler> messageHandlerDic = new Dictionary<int, ServerMessageHandler>();

        private IMessageCrypto messageCrypto = null;
        private IMessageCompressor messageCompressor = null;
        public ServerNetListener(int id,IMessageCrypto crypto,IMessageCompressor compressor)
        {
            uniqueID = id;
            messageCrypto = crypto;
            messageCompressor = compressor;
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

            int netID = clientIDCreator.NextID;
            ServerNet serverNet = new ServerNet(netID, handler,messageCrypto,messageCompressor);
            serverNet.MessageReceived = OnMessageReceived;
            serverNet.NetDisconnected = OnNetDisconnected;

            lock(netDicLock)
            {
                netDic.Add(netID, serverNet);
            }
        }

        private void OnMessageReceived(int netID,int messageID,byte[] msgBytes)
        {
            if (messageParserDic.TryGetValue(messageID, out ServerMessageParser parser) && parser != null)
            {
                if (messageHandlerDic.TryGetValue(messageID, out ServerMessageHandler handler) && handler != null)
                {
                    handler.Invoke(netID,messageID, parser.Invoke(messageID, msgBytes));
                }
                else
                {
                    LogUtil.LogError(ServerNetConst.LOGGER_NAME, $"ServerNetListener::OnMessageReceived->the handler not found.messageID = {messageID}");
                }
            }
            else
            {
                LogUtil.LogError(ServerNetConst.LOGGER_NAME, $"ServerNetListener::OnMessageReceived->the parser not found.messageID = {messageID}");
            }
        }

        private void OnNetDisconnected(int id)
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

        public void SendData(int netID,int messageID)
        {
            if (netDic.TryGetValue(netID, out ServerNet serverNet))
            {
                serverNet.SendData(messageID);
            }
        }

        public void SendData(int netID, int messageID, byte[] msg)
        {
            if (netDic.TryGetValue(netID, out ServerNet serverNet))
            {
                serverNet.SendData(messageID,msg);
            }
        }

        public void SendData(int netID,int messageID,byte[] msg,bool isCrypto,bool isCompress)
        {
            if (netDic.TryGetValue(netID, out ServerNet serverNet))
            {
                serverNet.SendData(messageID, msg,isCrypto,isCompress);
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

        #region Register Message Parser
        public void RegisterMessageParser(int messageID, ServerMessageParser parser)
        {
            if (!messageParserDic.ContainsKey(messageID))
            {
                messageParserDic.Add(messageID, parser);
            }
            else
            {
                LogUtil.LogError(ServerNetConst.LOGGER_NAME, $"ServerNetListener::RegisterMessageParser->The parser has been added.messageID={messageID}");
            }
        }

        public void UnregisterMessageParser(int messageID)
        {
            if (messageParserDic.ContainsKey(messageID))
            {
                messageParserDic.Remove(messageID);
            }
            else
            {
                LogUtil.LogError(ServerNetConst.LOGGER_NAME, $"ServerNetListener::UnregisterMessageParser->The parser not found.messageID={messageID}");
            }
        }
        #endregion

        #region Register Message Handler

        public void RegisterMessageHandler(int messageID, ServerMessageHandler handler)
        {
            if (!messageHandlerDic.ContainsKey(messageID))
            {
                messageHandlerDic.Add(messageID, handler);
            }
            else
            {
                LogUtil.LogError(ServerNetConst.LOGGER_NAME, $"ServerNetListener::RegisterMessageHandler->the handler has been added.messageID={messageID}");
            }
        }

        public void UnregisterMessageHandler(int messageID)
        {
            if (messageHandlerDic.ContainsKey(messageID))
            {
                messageHandlerDic.Remove(messageID);
            }
            else
            {
                LogUtil.LogError(ServerNetConst.LOGGER_NAME, $"ServerNetListener::UnregisterMessageHandler->The handler not found.messageID={messageID}");
            }
        }

        #endregion
    }
}
