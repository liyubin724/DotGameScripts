namespace Dot.Net.Message
{
    public interface IMessageCrypto
    {
        byte[] Encrypt(byte[] datas);
        byte[] Decrypt(byte[] datas);
    }
}