namespace Dot.Net
{
    public enum MessageReaderWriterType
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
    }
}
