using Dot.Core.Dispose;
using Dot.Log;
using Dot.Net.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.Server
{
    public class ServerNet : IDispose
    {
        private IMessageWriter messageWriter = null;
        private IMessageReader messageReader = null;

        private ServerNetSession netSession = null;

        public ServerNet(int id, Socket socket, IMessageWriter writer, IMessageReader reader)
        {
            messageWriter = writer;
            messageReader = reader;

            netSession = new ServerNetSession(id,socket,messageReader);
            messageReader.MessageError = OnMessageError;
            messageReader.MessageReceived = OnMessageReceived;
        }

        private void OnMessageError(MessageErrorCode code)
        {
            LogUtil.LogError(ServerNetConst.LOGGER_NAME, $"ServerNet::OnMessageError->message error.code = {code}");
            netSession.Disconnect();
        }

        private void OnMessageReceived(int messageID, byte[] datas)
        {
            //if (handlerDic.TryGetValue(messageID, out OnMessageHandler handler))
            //{
            //    handler(datas);
            //}
            //else
            //{
            //    LogUtil.LogWarning(ClientNetConst.LOGGER_NAME, $"ClientNet::OnMessageHandler->not found handler.messageID={messageID}");
            //}
        }

        public void Dispose()
        {
            
        }
    }
}
