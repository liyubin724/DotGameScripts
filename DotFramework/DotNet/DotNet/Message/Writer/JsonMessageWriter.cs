using Newtonsoft.Json;
using System.Text;

namespace Dot.Net.Message.Writer
{
    public class JsonMessageWriter : AMessageWriter
    {
        public JsonMessageWriter()
        {
        }

        public JsonMessageWriter(IMessageCompressor compressor, IMessageCrypto crypto) : base(compressor, crypto)
        {
        }

        protected override byte[] EncodeMessage<T>(T message)
        {
            string json = JsonConvert.SerializeObject(message);
            if(string.IsNullOrEmpty(json))
            {
                return null;
            }else
            {
                return Encoding.Default.GetBytes(json);
            }
        }
    }
}
