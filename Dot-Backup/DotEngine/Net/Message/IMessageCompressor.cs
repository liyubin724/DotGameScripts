namespace Dot.Net.Message
{
    public interface IMessageCompressor
    {
        byte[] Compress(byte[] bytes);
        byte[] Uncompress(byte[] bytes);
    }
}
