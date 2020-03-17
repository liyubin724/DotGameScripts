using Dot.Core.Proxy;
using Dot.Net.Message;

namespace Dot.Net.Client
{
    public class ClientNet : IClientNetStateListener
    {
        private IMessageWriter messageWriter = null;
        private IMessageReader messageReader = null;

        private ClientNetSession netSession = null;

        public ClientNet(IMessageWriter writer, IMessageReader reader)
        {
            messageWriter = writer;
            messageReader = reader;

            netSession = new ClientNetSession(messageReader, this);
            messageReader.MessageError = OnMessageError;
            messageReader.MessageReceived = OnMessageReceived;

            UpdateProxy.GetInstance().DoUpdateHandle += DoUpdate;
            UpdateProxy.GetInstance().DoLateUpdateHandle += DoLateUpdate;
        }

        public bool IsConnected()
        {
            return netSession.IsConnected();
        }

        public void Connect(string address)
        {
            netSession.Connect(address);
        }

        public void Connect(string ip,int port)
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

        public void OnStateChanged(ClientNetSessionState state)
        {
            
        }

        private void DoUpdate(float deltaTime)
        {

        }

        private void DoLateUpdate()
        {

        }

        public void Dispose()
        {
            UpdateProxy.GetInstance().DoUpdateHandle -= DoUpdate;
            UpdateProxy.GetInstance().DoLateUpdateHandle -= DoLateUpdate;

            messageReader.MessageError = null;
            messageReader.MessageReceived = null;

            netSession.Disconnect();
            netSession = null;
        }

        private void OnMessageError(MessageErrorCode code)
        {

        }

        private void OnMessageReceived(int messageID,byte[] datas)
        {

        }

    }
}
