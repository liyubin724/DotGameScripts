using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Net.Message.Reader
{
    public class PBMessageReader : AMessageReader
    {
        public PBMessageReader()
        {
        }

        public PBMessageReader(IMessageCrypto crypto, IMessageCompressor compressor) : base(crypto, compressor)
        {
        }

        protected override object DecodeMessage(byte[] datas)
        {
            throw new NotImplementedException();
        }
    }
}
