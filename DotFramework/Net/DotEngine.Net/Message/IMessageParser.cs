namespace DotEngine.Net.Message
{
    public interface IMessageParser
    {
        byte[] EncodeMessage(int messageID, object message);
        object DecodeMessage(int messageID, byte[] bytes);
    }
}
