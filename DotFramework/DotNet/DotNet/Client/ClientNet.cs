using Dot.Core.Dispose;
using Dot.Log;
using Dot.Net.Message;

namespace Dot.Net.Client
{
    public delegate void OnClientReceiveNetMessage(int id, int messageID, byte[] msgDatas);
    public delegate void OnClientChangeState(int id, ClientNetSessionState oldState, ClientNetSessionState newState);

    public class ClientNet : IDispose
    {
        private int id = -1;

        private MessageWriter messageWriter = null;
        private MessageReader messageReader = null;

        private ClientNetSession netSession = null;
        private ClientNetSessionState sessionState = ClientNetSessionState.Unavailable;

        public OnClientChangeState NetStageChanged { get; set; }
        public OnClientReceiveNetMessage NetMessageRecevied { get; set; }

        public ClientNet(int id,IMessageCrypto crypto,IMessageCompressor compressor)
        {
            this.id = id;
            messageWriter = new MessageWriter(compressor,crypto);
            messageReader = new MessageReader(crypto, compressor);

            netSession = new ClientNetSession(messageReader);
            messageReader.MessageError = OnMessageError;
            messageReader.MessageReceived = OnMessageReceived;
        }

        public bool IsConnected()
        {
            return netSession.IsConnected();
        }

        public void Connect(string address)
        {
            netSession.Connect(address);
        }

        public void Connect(string ip, int port)
        {
            netSession.Connect(ip, port);
        }

        public void Reconnect()
        {
            netSession.Reconnect();
        }

        public void Disconnect()
        {
            netSession.Disconnect();
        }

        internal void DoUpdate(float deltaTime)
        {
            ClientNetSessionState currentSessionState = netSession.State;
            if (currentSessionState != sessionState)
            {
                ClientNetSessionState oldState = sessionState;
                sessionState = currentSessionState;
                NetStageChanged?.Invoke(id, oldState, sessionState);

                //if (currentSessionState == ClientNetSessionState.Connecting)
                //{
                //    NetConnecting?.Invoke(this);
                //}
                //else if (currentSessionState == ClientNetSessionState.Normal)
                //{
                //    NetConnectedSuccess?.Invoke(this);
                //}
                //else if (currentSessionState == ClientNetSessionState.ConnectedFailed)
                //{
                //    NetConnectedFailed?.Invoke(this);
                //}
                //else if (currentSessionState == ClientNetSessionState.Disconnected)
                //{
                //    NetDisconnected?.Invoke(this);
                //}
            }
        }

        internal void DoLateUpdate()
        {
            if (IsConnected())
            {
                netSession.DoLateUpdate();
            }
        }

        public void Dispose()
        {
            messageReader.MessageError = null;
            messageReader.MessageReceived = null;

            netSession.Dispose();
            netSession = null;
        }

        private void OnMessageError(MessageErrorCode code)
        {
            LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNet::OnMessageError->message error.code = {code}");
            netSession.Disconnect();
        }

        private void OnMessageReceived(int messageID, byte[] datas)
        {
            NetMessageRecevied?.Invoke(id,messageID, datas);
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

        public void SendData(int messageID, byte[] msg)
        {
            if (IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeData(messageID, msg);
                if (netBytes != null && netBytes.Length > 0)
                {
                    netSession.Send(netBytes);
                }
            }
        }

        public void SendData(int messageID, byte[] msg, bool isCrypto, bool isCompress)
        {
            if (IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeData(messageID, msg, isCrypto, isCompress);
                if (netBytes != null && netBytes.Length > 0)
                {
                    netSession.Send(netBytes);
                }
            }
        }

    }
}
