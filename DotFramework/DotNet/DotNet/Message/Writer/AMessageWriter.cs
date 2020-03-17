using Dot.Core.Util;
using Dot.Net.Stream;
using System;
using System.Net;

namespace Dot.Net.Message.Writer
{
    public enum MessageWriterType : byte
    {
        None = 0,
        Json,
        ProtoBuf,
    }

    public abstract class AMessageWriter : IMessageWriter
    {
        public MessageWriterType WriterType { get; set; } = MessageWriterType.None;
        public IMessageCrypto Crypto { get; set; } = null;
        public IMessageCompressor Compressor { get; set; } = null;

        private byte serialNumber = 0;
        private MemoryStreamEx bufferStream = new MemoryStreamEx();

        protected AMessageWriter(MessageWriterType writerType)
        {
            WriterType = writerType;
        }

        protected AMessageWriter(MessageWriterType writerType,
            IMessageCompressor compressor,
            IMessageCrypto crypto):this(writerType)
        {
            Compressor = compressor;
            Crypto = crypto;
        }

        public byte[] EncodeData(int messageID, byte[] datas)
        {
            return EncodeData(messageID, datas, false, false);
        }

        public byte[] EncodeData(int messageID, byte[] datas, bool isCrypto, bool isCompress)
        {
            bufferStream.Clear();

            if(isCrypto && Crypto == null)
            {
                isCrypto = false;
            }
            if(isCompress && Compressor ==null)
            {
                isCompress = false;
            }

            byte flag = 0;
            byte[] dataBytes = datas;
            if(isCrypto)
            {
                BitUtil.SetBit(flag, 0, true);
                dataBytes = Crypto.Encrypt(dataBytes);
            }
            if(isCompress)
            {
                BitUtil.SetBit(flag, 1, true);
                dataBytes = Compressor.Compress(dataBytes);
            }

            int byteTotalSize = MessageConst.MessageMinSize + (dataBytes != null ? dataBytes.Length + sizeof(byte) : 0);
            int netByteTotalSize = IPAddress.HostToNetworkOrder(byteTotalSize);
            byte[] netSizeBytes = BitConverter.GetBytes(netByteTotalSize);

            ++serialNumber;

            bufferStream.Write(netSizeBytes, 0, netSizeBytes.Length);
            bufferStream.WriteByte((byte)serialNumber);
            bufferStream.WriteByte(flag);

            int netMessageID = IPAddress.HostToNetworkOrder(messageID);
            byte[] netMessageIDBytes = BitConverter.GetBytes(netMessageID);

            bufferStream.Write(netMessageIDBytes, 0, netMessageIDBytes.Length);
            if (dataBytes != null)
            {
                bufferStream.WriteByte((byte)WriterType);
                bufferStream.Write(dataBytes, 0, dataBytes.Length);
            }
            return bufferStream.ToArray();
        }

        public byte[] EncodeMessage<T>(int messageID, T message)
        {
            return EncodeMessage(messageID, EncodeMessage(message),false,false);
        }

        public byte[] EncodeMessage<T>(int messageID, T message, bool isCrypto, bool isCompress)
        {
            return EncodeData(messageID, EncodeMessage(message),isCrypto,isCompress);
        }

        public byte[] EncodeMessage(int messageID)
        {
            return EncodeData(messageID, null);
        }

        public abstract byte[] EncodeMessage<T>(T message);

        public virtual void Reset()
        {
            serialNumber = 0;
            bufferStream.Clear();
        }

    }
}
