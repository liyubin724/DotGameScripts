using DotEngine.Utilities;
using DotEngine.Net.Stream;
using System;
using System.Net;

namespace DotEngine.Net.Message
{
    public class MessageWriter
    {
        private byte serialNumber = 0;
        private MemoryStreamEx bufferStream = new MemoryStreamEx();
        private IMessageParser messageParser = null;

        public MessageWriter(IMessageParser parser)
        {
            messageParser = parser;
        }

        public byte[] EncodeEmptyMessage(int messageID)
        {
            return EncodeNetData(messageID,null);
        }

        public byte[] EncodeMessage(int messageID,object message)
        {
            return EncodeNetData(messageID,messageParser.EncodeMessage(messageID,message));
        }

        private byte[] EncodeNetData(int messageID,byte[] datas)
        {
            bufferStream.Clear();


            byte flag = 0;
            byte[] dataBytes = msgBytes;

            int byteTotalSize = MessageConst.MESSAGE_MIN_LENGTH + (dataBytes != null ? dataBytes.Length : 0);
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
                bufferStream.Write(dataBytes, 0, dataBytes.Length);
            }
            return bufferStream.ToArray();

        }
        
        public byte[] EncodeData(int messageID)
        {
            return EncodeData(messageID, null, false, false);
        }

        public byte[] EncodeData(int messageID, byte[] msgBytes)
        {
            return EncodeData(messageID, msgBytes, true, true);
        }

        public byte[] EncodeData(int messageID, byte[] msgBytes)
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
            byte[] dataBytes = msgBytes;
            if(isCrypto)
            {
                BitUtility.SetBit(flag, MessageConst.MESSAGE_CRYPTO_FLAG_INDEX, true);
                dataBytes = Crypto.Encrypt(dataBytes);
            }
            if(isCompress)
            {
                BitUtility.SetBit(flag, MessageConst.MESSAGE_COMPRESSOR_FLAG_INDEX, true);
                dataBytes = Compressor.Compress(dataBytes);
            }

            int byteTotalSize = MessageConst.MESSAGE_MIN_LENGTH + (dataBytes != null ? dataBytes.Length : 0);
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
                bufferStream.Write(dataBytes, 0, dataBytes.Length);
            }
            return bufferStream.ToArray();
        }

        public virtual void Reset()
        {
            serialNumber = 0;
            bufferStream.Clear();
        }
    }
}
