using DotEngine.Log;
using DotEngine.Net.Message;
using System.Net.Sockets;

namespace DotEngine.Net.Server
{
    public delegate void OnMessageReceived(int id, int messageID, byte[] datas);
    public delegate void OnNetDisconnected(int id);

    public class ServerNet //: IDispose
    {
        private int uniqueID = -1;
        public int UniqueID { get => uniqueID; }

        private MessageWriter messageWriter = null;
        private MessageReader messageReader = null;

        private ServerNetSession netSession = null;

        public OnMessageReceived MessageReceived { get; set; } = null;
        public OnNetDisconnected NetDisconnected { get; set; } = null;

        private ServerNetSessionState sessionState = ServerNetSessionState.Unavailable;

        public ServerNet(int id, Socket socket, IMessageCrypto crypto,IMessageCompressor compressor)
        {
            uniqueID = id;
            messageWriter = new MessageWriter(compressor,crypto);
            messageReader = new MessageReader(crypto,compressor);

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
            LogUtil.LogError(NetConst.SERVER_LOGGER_TAG, $"ServerNet::OnMessageError->message error.code = {code}");
            Dispose();
        }

        private void OnMessageReceived(int messageID, byte[] datas)
        {
            MessageReceived?.Invoke(uniqueID, messageID, datas);
        }

        internal void DoUpdate(float deltaTime)
        {
            ServerNetSessionState currentSessionState = netSession.State;
            if (currentSessionState != sessionState)
            {
                sessionState = currentSessionState;
                 if (currentSessionState == ServerNetSessionState.Disconnected)
                {
                    NetDisconnected?.Invoke(uniqueID);
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
            uniqueID = -1;
            
            messageReader.MessageError = null;
            messageReader.MessageReceived = null;
            messageReader = null;

            messageWriter = null;
            MessageReceived = null;

            netSession.Dispose();
        }
    }
}
