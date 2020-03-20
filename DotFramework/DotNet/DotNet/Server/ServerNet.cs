using Dot.Core.Dispose;
using Dot.Log;
using Dot.Net.Message;
using System.Net.Sockets;

namespace Dot.Net.Server
{
    public delegate void OnMessageReceived(long id, int messageID, byte[] datas);
    public delegate void OnNetDisconnected(long id);

    public class ServerNet : IDispose
    {
        private long id = -1;
        public long ID { get => id; }

        private IMessageWriter messageWriter = null;
        private IMessageReader messageReader = null;

        private ServerNetSession netSession = null;

        public OnMessageReceived MessageReceived { get; set; } = null;
        public OnNetDisconnected NetDisconnected { get; set; } = null;

        private ServerNetSessionState sessionState = ServerNetSessionState.Unavailable;

        public ServerNet(long id, Socket socket, IMessageWriter writer, IMessageReader reader)
        {
            this.id = id;
            messageWriter = writer;
            messageReader = reader;

            netSession = new ServerNetSession(socket,messageReader);
            messageReader.MessageError = OnMessageError;
            messageReader.MessageReceived = OnMessageReceived;
        }

        public bool IsConnected()
        {
            return netSession.IsConnected();
        }

        private void OnMessageError(MessageErrorCode code)
        {
            LogUtil.LogError(ServerNetConst.LOGGER_NAME, $"ServerNet::OnMessageError->message error.code = {code}");
            Dispose();
        }

        private void OnMessageReceived(int messageID, byte[] datas)
        {
            MessageReceived?.Invoke(id, messageID, datas);
        }

        internal void DoUpdate(float deltaTime)
        {
            ServerNetSessionState currentSessionState = netSession.State;
            if (currentSessionState != sessionState)
            {
                sessionState = currentSessionState;
                 if (currentSessionState == ServerNetSessionState.Disconnected)
                {
                    NetDisconnected?.Invoke(id);
                }
            }
        }

        internal void DoLateUpdate()
        {
            netSession.DoLateUpdate();
        }

        public void SendData(int messageID)
        {
            if (IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeData(messageID);
                if (netBytes != null && netBytes.Length > 0)
                {
                    netSession.Send(netBytes);
                }
            }
        }

        public void SendData(int messageID, byte[] msgDatas)
        {
            if (IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeData(messageID, msgDatas);
                if (netBytes != null && netBytes.Length > 0)
                {
                    netSession.Send(netBytes);
                }
            }
        }

        public void SendData(int messageID,byte[] msgDatas,bool isCrypto,bool isCompress)
        {
            if (IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeData(messageID, msgDatas,isCrypto,isCompress);
                if (netBytes != null && netBytes.Length > 0)
                {
                    netSession.Send(netBytes);
                }
            }
        }

        public void Dispose()
        {
            id = -1;
            
            messageReader.MessageError = null;
            messageReader.MessageReceived = null;
            messageReader = null;

            messageWriter = null;
            MessageReceived = null;

            netSession.Dispose();
        }
    }
}
