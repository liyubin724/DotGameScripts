using Dot.Core.Proxy;
using Dot.Log;
using Dot.Net.Message;
using System.Collections.Generic;

namespace Dot.Net.Client
{
    public delegate void OnMessageHandler(byte[] datas);

    public delegate void OnNetConnecting(ClientNet net);
    public delegate void OnNetConnectedSuccess(ClientNet clientNet);
    public delegate void OnNetConnectedFailed(ClientNet clientNet);
    public delegate void OnNetDisconnected(ClientNet clientNet);
    
    public class ClientNet
    {
        private IMessageWriter messageWriter = null;
        private IMessageReader messageReader = null;

        private ClientNetSession netSession = null;
        private ClientNetSessionState sessionState = ClientNetSessionState.Unavailable;

        private Dictionary<int, OnMessageHandler> handlerDic = new Dictionary<int, OnMessageHandler>();

        public event OnNetConnecting NetConnecting;
        public event OnNetConnectedSuccess NetConnectedSuccess;
        public event OnNetConnectedFailed NetConnectedFailed;
        public event OnNetDisconnected NetDisconnected;

        public ClientNet(IMessageWriter writer, IMessageReader reader)
        {
            messageWriter = writer;
            messageReader = reader;

            netSession = new ClientNetSession(messageReader);
            messageReader.MessageError = OnMessageError;
            messageReader.MessageReceived = OnMessageReceived;
        }

        public bool IsConnected()
        {
            return netSession.IsConnected();
        }

        public void Connect(string address)
        {
            netSession.Connect(address);
        }

        public void Connect(string ip,int port)
        {
            netSession.Connect(ip, port);
        }

        public void Reconnect()
        {
            netSession.Reconnect();
        }

        public void Disconnect()
        {
            netSession.Disconnect();
        }

        internal void DoUpdate(float deltaTime)
        {
            ClientNetSessionState currentSessionState = netSession.State;
            if(currentSessionState!=sessionState)
            {
                sessionState = currentSessionState;
                if(currentSessionState == ClientNetSessionState.Connecting)
                {
                    NetConnecting?.Invoke(this);
                }else if(currentSessionState == ClientNetSessionState.Normal)
                {
                    NetConnectedSuccess?.Invoke(this);
                }else if(currentSessionState == ClientNetSessionState.ConnectedFailed)
                {
                    NetConnectedFailed?.Invoke(this);
                }else if(currentSessionState == ClientNetSessionState.Disconnected)
                {
                    NetDisconnected?.Invoke(this);
                }
            }
        }

        internal void DoLateUpdate()
        {
            if(IsConnected())
            {
                netSession.DoLateUpdate();
            }
        }

        public void Dispose()
        {
            messageReader.MessageError = null;
            messageReader.MessageReceived = null;

            handlerDic.Clear();

            netSession.Disconnect();
            netSession = null;
        }

        private void OnMessageError(MessageErrorCode code)
        {
            LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNet::OnMessageError->message error.code = {code}");
            netSession.Disconnect();
        }

        private void OnMessageReceived(int messageID,byte[] datas)
        {
            if(handlerDic.TryGetValue(messageID,out OnMessageHandler handler))
            {
                handler(datas);
            }else
            {
                LogUtil.LogWarning(ClientNetConst.LOGGER_NAME, $"ClientNet::OnMessageHandler->not found handler.messageID={messageID}");
            }
        }

        public void RegisterHandler(int messageID,OnMessageHandler handler)
        {
            if(!handlerDic.ContainsKey(messageID))
            {
                handlerDic.Add(messageID, handler);
            }else
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNet::RegisterHandler->the messageID has been added.messageID={messageID}");
            }
        }

        public void UnregisterHandler(int messageID)
        {
            if(handlerDic.ContainsKey(messageID))
            {
                handlerDic.Remove(messageID);
            }
        }

        public void SendMessage<T>(int messageID,T msg)
        {
            if(IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeMessage<T>(messageID, msg);
                if(netBytes!=null && netBytes.Length>0)
                {
                    netSession.Send(netBytes);
                }
            }
        }

        public void SendEmptyMessage(int messageID)
        {
            if(IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeMessage(messageID);
                if(netBytes!=null && netBytes.Length>0)
                {
                    netSession.Send(netBytes);
                }
            }
        }

        public void SendData(int messageID,byte[] msg)
        {
            if(IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeData(messageID, msg);
                if(netBytes!=null && netBytes.Length>0)
                {
                    netSession.Send(netBytes);
                }
            }
        }
    }
}
