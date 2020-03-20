using System.Text;

namespace Dot.Net.Message.Reader
{
    public class JsonMessageReader : AMessageReader
    {
        public JsonMessageReader()
        {
        }

        public JsonMessageReader(IMessageCrypto crypto, IMessageCompressor compressor) : base(crypto, compressor)
        {
        }

        protected override object DecodeMessage(int messageID,byte[] datas)
        {
            if(datas == null || datas.Length == 0)
            {
                return null;
            }
            return Encoding.UTF8.GetString(datas);
        }
    }
}
