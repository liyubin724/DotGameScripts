using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Net.Message.Writer
{
    public class PBMessageWriter : AMessageWriter
    {
        public PBMessageWriter()
        {
        }

        public PBMessageWriter(IMessageCompressor compressor, IMessageCrypto crypto) : base(compressor, crypto)
        {
        }

        protected override byte[] EncodeMessage<T>(T message)
        {
            throw new NotImplementedException();
        }
    }
}
