using Dot.Net.Message;
using Dot.Net.Message.Compressor;
using Dot.Net.Message.Crypto;

namespace Dot.Net
{
    public enum MessageFormatType
    {
        Json = 0,
        ProtoBuf = 1,
    }

    public enum MessageCryptoType
    {
        None = 0,
        AES,
    }

    public enum MessageCompressorType
    {
        None = 0,
        Snappy,
    }

    public static class NetConst
    {
        public static IMessageCrypto GetAESMessageCrypto(string aesKey,string aesIV)
        {
            return new AESMessageCrypto(aesKey, aesIV);
        }

        public static IMessageCompressor GetMessageCompressor(MessageCompressorType compressorType)
        {
            if(compressorType == MessageCompressorType.Snappy)
            {
                return new SnappyMessageCompressor();
            }
            return null;
        }
    }
}
