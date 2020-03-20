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

        protected override object DecodeMessage(int messageID,byte[] datas)
        {
            return datas;
        }
    }
}
