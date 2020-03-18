using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected override object DecodeMessage(byte[] datas)
        {
            return null;
        }
    }
}
