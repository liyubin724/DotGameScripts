#if NET_MESSAGE_PB
using Google.Protobuf;
namespace Dot.Net.Server
{
    public partial class ServerNet
    {
        public void SendPBMessage(int messageID)
        {
            if(IsConnected())
            {
                SendData(messageID);
            }
        }

        public void SendPBMessage<T>(int messageID, T msg) where T : IMessage
        {
            byte[] msgBytes = null;
            if (msg != null)
            {
                msgBytes = msg.ToByteArray();
            }
            SendData(messageID, msgBytes);
        }
        public void SendPBMessage<T>(int messageID, T msg, bool isCrypto, bool isCompress) where T : IMessage
        {
            byte[] msgBytes = null;
            if (msg != null)
            {
                msgBytes = msg.ToByteArray();
            }
            SendData(messageID, msgBytes,isCrypto,isCompress);
        }
    }
}
#endif