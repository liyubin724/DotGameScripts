using Dot.Core.Util;
using Dot.Net.Stream;
using System.Net;

namespace Dot.Net.Message.Reader
{
    public class MessageReader : IMessageReader
    {
        public IMessageCrypto Crypto { get; set; } = null;
        public IMessageCompressor Compressor { get; set; } = null;
        public OnMessageReceived MessageReceived { get; set; } = null;
        public OnMessageError MessageError { get; set; } = null;

        private BufferStream bufferStream = new BufferStream();
        private byte serialNumber = 0;

        public MessageReader()
        {
        }

        public MessageReader(IMessageCrypto crypto,IMessageCompressor compressor)
        {
            Crypto = crypto;
            Compressor = compressor;
        }

        public void OnDataReceived(byte[] datas, int size)
        {
            MemoryStreamEx stream = bufferStream.GetActivedStream();
            stream.Write(datas, 0, size);

            int streamLength = (int)stream.Length;
            if (streamLength >= MessageConst.MESSAGE_MIN_LENGTH)
            {
                int startIndex = 0;
                while (true)
                {
                    if (!stream.ReadInt(startIndex, out int totalLength))
                    {
                        break;
                    }
                    totalLength = IPAddress.NetworkToHostOrder(totalLength);
                    if (streamLength < totalLength)
                    {
                        break;
                    }

                    int offsetIndex = startIndex;
                    offsetIndex += sizeof(int);
                    if (!stream.ReadByte(offsetIndex, out byte serialNum))
                    {
                        MessageError?.Invoke(MessageErrorCode.Reader_ReadSerialNumberError);
                        break;
                    }
                    if (serialNumber + 1 != serialNum)
                    {
                        MessageError?.Invoke(MessageErrorCode.Reader_CompareSerialNumberError);
                        break;
                    }

                    offsetIndex += sizeof(byte);
                    if (!stream.ReadByte(offsetIndex, out byte flag))
                    {
                        MessageError?.Invoke(MessageErrorCode.Reader_ReadFlagTypeError);
                        break;
                    }

                    bool isCrypto = BitUtil.IsEnable(flag, MessageConst.MESSAGE_CRYPTO_FLAG_INDEX);
                    bool isCompressor = BitUtil.IsEnable(flag, MessageConst.MESSAGE_COMPRESSOR_FLAG_INDEX);

                    offsetIndex += sizeof(byte);
                    if (!stream.ReadInt(offsetIndex, out int messageID))
                    {
                        MessageError?.Invoke(MessageErrorCode.Reader_ReadMessageIDError);
                        break;
                    }

                    messageID = IPAddress.NetworkToHostOrder(messageID);

                    offsetIndex += sizeof(int);
                    if (offsetIndex < totalLength + startIndex)
                    {
                        int messageDataLength = totalLength + startIndex - offsetIndex;
                        byte[] messageDatas = new byte[messageDataLength];
                        if (stream.Read(messageDatas, 0, messageDataLength) != messageDataLength)
                        {
                            MessageError?.Invoke(MessageErrorCode.Reader_CompareMessageDataLengthError);
                            break;
                        }

                        OnMessage(messageID, messageDatas,isCrypto,isCompressor);
                    }
                    else
                    {
                        OnMessage(messageID,null,isCrypto,isCompressor);
                    }

                    startIndex += totalLength;
                    serialNumber = serialNum;
                }

                bufferStream.MoveStream(startIndex);
            }
        }

        private void OnMessage(int messageID,byte[] messageDatas,bool isCrypt,bool isCompress)
        {
            byte[] msgBytes = messageDatas;
            if (msgBytes != null)
            {
                if (isCompress)
                {
                    msgBytes = Compressor.Uncompress(msgBytes);
                }
                if(isCrypt)
                {
                    msgBytes = Crypto.Decrypt(msgBytes);
                }
            }
            MessageReceived?.Invoke(messageID, msgBytes);
        }

        public void Reset()
        {
            bufferStream.Reset();
            serialNumber = 0;
        }
    }
}
