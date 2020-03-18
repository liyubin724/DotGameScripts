namespace Dot.Net.Message
{
    public delegate void OnMessageReceived(int messageID, object message);
    public delegate void OnMessageError(MessageErrorCode errorCode);

    public interface IMessageReader
    {
        IMessageCrypto Crypto { get; set; }
        IMessageCompressor Compressor { get; set; }

        OnMessageReceived MessageReceived { get; set; }
        OnMessageError MessageError { get; set; }

        void OnDataReceived(byte[] datas, int size);
        void DoReadData();
        void Reset();
    }
}
