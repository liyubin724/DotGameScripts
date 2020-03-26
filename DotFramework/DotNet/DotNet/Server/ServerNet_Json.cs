#if NET_MESSAGE_JSON
using Newtonsoft.Json;
using System.Text;

namespace Dot.Net.Server
{
    public partial class ServerNet
    {
        public void SendJsonMessage(int messageID)
        {
            SendData(messageID);
        }

        public void SendJsonMessage<T>(int messageID, T msg)
        {
            string msgJson = JsonConvert.SerializeObject(msg);
            byte[] msgBytes = null;
            if (!string.IsNullOrEmpty(msgJson))
            {
                msgBytes = Encoding.UTF8.GetBytes(msgJson);
            }
            SendData(messageID, msgBytes);
        }

        public void SendJsonMessage<T>(int messageID, T msg, bool isCrypto, bool isCompress)
        {
            string msgJson = JsonConvert.SerializeObject(msg);
            byte[] msgBytes = null;
            if (!string.IsNullOrEmpty(msgJson))
            {
                msgBytes = Encoding.UTF8.GetBytes(msgJson);
            }
            SendData(messageID, msgBytes,isCrypto,isCompress);
        }
    }
}
#endif