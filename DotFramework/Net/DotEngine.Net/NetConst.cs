using DotEngine.Net.Message;
using DotEngine.Net.Message.Compressor;
using DotEngine.Net.Message.Crypto;

namespace DotEngine.Net
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
        public const string CLIENT_LOGGER_TAG = "ClientNet";
        public const string SERVER_LOGGER_TAG = "ServerNet";
        public static readonly int BUFFER_SIZE = 4096;

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
