﻿using Dot.Core.Dispose;
using Dot.Log;
using Dot.Net.Message;
using System.Collections.Generic;

namespace Dot.Net.Client
{
    public delegate void OnNetStateChanged(ClientNet clientNet);

    public delegate object MessageParser(int messageID, byte[] msgDatas);
    public delegate void MessageHandler(int messageID, object message);

    public class ClientNet : IDispose
    {
        private int id = -1;

        private MessageWriter messageWriter = null;
        private MessageReader messageReader = null;

        private ClientNetSession netSession = null;
        private ClientNetSessionState sessionState = ClientNetSessionState.Unavailable;

        private Dictionary<int, MessageParser> messageParserDic = new Dictionary<int, MessageParser>();
        private Dictionary<int, MessageHandler> messageHandlerDic = new Dictionary<int, MessageHandler>();

        public event OnNetStateChanged NetConnecting;
        public event OnNetStateChanged NetConnectedSuccess;
        public event OnNetStateChanged NetConnectedFailed;
        public event OnNetStateChanged NetDisconnected;

        public ClientNet(int id,IMessageCrypto crypto,IMessageCompressor compressor)
        {
            this.id = id;
            messageWriter = new MessageWriter(compressor,crypto);
            messageReader = new MessageReader(crypto, compressor);

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

        public void Connect(string ip, int port)
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
            if (currentSessionState != sessionState)
            {
                sessionState = currentSessionState;

                if (currentSessionState == ClientNetSessionState.Connecting)
                {
                    NetConnecting?.Invoke(this);
                }
                else if (currentSessionState == ClientNetSessionState.Normal)
                {
                    NetConnectedSuccess?.Invoke(this);
                }
                else if (currentSessionState == ClientNetSessionState.ConnectedFailed)
                {
                    NetConnectedFailed?.Invoke(this);
                }
                else if (currentSessionState == ClientNetSessionState.Disconnected)
                {
                    NetDisconnected?.Invoke(this);
                }
            }
        }

        internal void DoLateUpdate()
        {
            if (IsConnected())
            {
                netSession.DoLateUpdate();
            }
        }

        public void Dispose()
        {
            messageReader.MessageError = null;
            messageReader.MessageReceived = null;

            messageParserDic.Clear();
            messageHandlerDic.Clear();

            netSession.Dispose();
            netSession = null;
        }

        private void OnMessageError(MessageErrorCode code)
        {
            LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNet::OnMessageError->message error.code = {code}");
            netSession.Disconnect();
        }

        private void OnMessageReceived(int messageID, byte[] datas)
        {
            if(messageParserDic.TryGetValue(messageID,out MessageParser parser) && parser!=null)
            {
                if(messageHandlerDic.TryGetValue(messageID,out MessageHandler handler) && handler !=null)
                {
                    handler.Invoke(messageID, parser.Invoke(messageID, datas));
                }else
                {
                    LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNet::OnMessageReceived->the handler not found.messageID = {messageID}");
                }
            }else
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNet::OnMessageReceived->the parser not found.messageID = {messageID}");
            }
        }

        #region Send Data
        public void SendData(int messageID)
        {
            if (IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeData(messageID);
                if (netBytes != null && netBytes.Length > 0)
                {
                    netSession.Send(netBytes);
                }
            }
        }

        public void SendData(int messageID, byte[] msg)
        {
            if (IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeData(messageID, msg);
                if (netBytes != null && netBytes.Length > 0)
                {
                    netSession.Send(netBytes);
                }
            }
        }

        public void SendData(int messageID, byte[] msg, bool isCrypto, bool isCompress)
        {
            if (IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeData(messageID, msg, isCrypto, isCompress);
                if (netBytes != null && netBytes.Length > 0)
                {
                    netSession.Send(netBytes);
                }
            }
        }
        #endregion

        #region Register Message Parser
        public void RegisterMessageParser(int messageID,MessageParser parser)
        {
            if(!messageParserDic.ContainsKey(messageID))
            {
                messageParserDic.Add(messageID, parser);
            }else
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNet::RegisterMessageParser->The parser has been added.messageID={messageID}");
            }
        }

        public void UnregisterMessageParser(int messageID)
        {
            if (messageParserDic.ContainsKey(messageID))
            {
                messageParserDic.Remove(messageID);
            }
            else
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNet::UnregisterMessageParser->The parser not found.messageID={messageID}");
            }
        }
        #endregion

        #region Register Message Handler

        public void RegisterMessageHandler(int messageID, MessageHandler handler)
        {
            if (!messageHandlerDic.ContainsKey(messageID))
            {
                messageHandlerDic.Add(messageID, handler);
            }
            else
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNet::RegisterMessageHandler->the handler has been added.messageID={messageID}");
            }
        }

        public void UnregisterMessageHandler(int messageID)
        {
            if (messageHandlerDic.ContainsKey(messageID))
            {
                messageHandlerDic.Remove(messageID);
            }
            else
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNet::UnregisterMessageHandler->The handler not found.messageID={messageID}");
            }
        }

        #endregion
    }
}
