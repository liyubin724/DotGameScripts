using Dot.Core;
using Dot.Core.Proxy;
using Dot.Net.Message;
using Dot.Net.Message.Compressor;
using Dot.Net.Message.Crypto;
using Dot.Net.Message.Reader;
using Dot.Net.Message.Writer;
using System;

namespace Dot.Net
{
    public partial class NetManager : Singleton<NetManager>
    {
        public MessageCompressorType CompressorType { get; set; } = MessageCompressorType.None;
        public MessageCryptoType CryptoType { get; set; } = MessageCryptoType.None;
        public MessageReaderWriterType RWType { get; set; } = MessageReaderWriterType.Json;

        protected override void DoInit()
        {
            UpdateProxy.GetInstance().DoUpdateHandle += DoUpdate;
            UpdateProxy.GetInstance().DoLateUpdateHandle += DoLateUpdate;
        }

        private void DoUpdate(float deltaTime)
        {
            DoUpdateClient(deltaTime);
            DoUpdateServer(deltaTime);
        }

        private void DoLateUpdate()
        {
            DoLateUpdateClient();
            DoLateUpdateServer();
        }

        private string aesKey = string.Empty;
        private string aesIV = string.Empty;
        public void SetAESKey(string aesKey,string aesIV)
        {
            this.aesKey = aesKey;
            this.aesIV = aesIV;
        }

        private IMessageCrypto GetCrypto()
        {
            if(CryptoType == MessageCryptoType.AES)
            {
                if(string.IsNullOrEmpty(aesKey) ||string.IsNullOrEmpty(aesIV))
                {
                    throw new ArgumentNullException();
                }else
                {
                    return new AESMessageCrypto(aesKey, aesIV);
                }
            }

            return null;
        }

        private IMessageCompressor GetCompressor()
        {
            if(CompressorType == MessageCompressorType.Snappy)
            {
                return new SnappyMessageCompressor();
            }
            return null;
        }

        private IMessageReader GetReader()
        {
            if(RWType == MessageReaderWriterType.Json)
            {
                return new JsonMessageReader();
            }

            return null;
        }

        private IMessageWriter GetWriter()
        {
            IMessageCrypto crypto = GetCrypto();
            IMessageCompressor compressor = GetCompressor();
            if(RWType == MessageReaderWriterType.Json)
            {
                return new JsonMessageWriter(compressor, crypto);
            }else if(RWType == MessageReaderWriterType.ProtoBuf)
            {

            }

            return null;
        }
    }
}
