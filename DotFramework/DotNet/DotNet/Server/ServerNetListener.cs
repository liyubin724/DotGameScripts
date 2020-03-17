using Dot.Core.Dispose;
using Dot.Core.Generic;
using Dot.Log;
using Dot.Net.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.Server
{
    public delegate IMessageCrypto GetMessageCrypto();
    public delegate IMessageCompressor GetMessageCompressor();
    public delegate IMessageWriter GetMessageWriter();

    public delegate void OnMessageHandler(int id,byte[] datas);

    public class ServerNetListener : IDispose
    {
        private ManualResetEvent allDone = new ManualResetEvent(false);

        private Socket socket = null;
        private UniqueID idCreator = new UniqueID();

        private Dictionary<int, ServerNet> netDic = new Dictionary<int, ServerNet>();
        private Dictionary<int, OnMessageHandler> handlerDic = new Dictionary<int, OnMessageHandler>();

        public GetMessageCrypto GetCrypto { get; set; } = null;
        public GetMessageCompressor GetCompressor { get; set; } = null;
        public GetMessageWriter GetWriter { get; set; } = null;

        public void Startup(int port,int maxCount)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(maxCount);

                while (true)
                {
                    allDone.Reset();
                    LogUtil.LogInfo("ServerNet", "Waiting for a connection...");

                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    allDone.WaitOne();
                }
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

            
        }

        internal void DoLateUpdate()
        {

        }

        public void Dispose()
        {
            
        }
    }
}
