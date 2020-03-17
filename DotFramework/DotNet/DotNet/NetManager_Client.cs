﻿using Dot.Core;
using Dot.Log;
using Dot.Net.Client;
using Dot.Net.Message;
using Dot.Net.Message.Reader;
using Dot.Net.Message.Writer;
using System.Collections.Generic;

namespace Dot.Net
{
    public partial class NetManager : Singleton<NetManager>
    {
        private Dictionary<int, ClientNet> clientNetDic = null;

        public ClientNet CreateClientNet(int netID, MessageWriterType writerType)
        {
            if(clientNetDic== null)
            {
                clientNetDic = new Dictionary<int, ClientNet>();
            }
            if(clientNetDic.ContainsKey(netID))
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"NetMananger::CreateClientNet->the net has been created.netID={netID}");
                return null;
            }

            IMessageWriter writer = null;
            if (writerType == MessageWriterType.Json)
            {
                writer = new JsonMessageWriter();
            }

            IMessageReader reader = new MessageReader();
            ClientNet net = new ClientNet(writer, reader);

            clientNetDic.Add(netID, net);

            return net;
        }

        public ClientNet GetClientNet(int netID)
        {
            if(clientNetDic!=null && clientNetDic.TryGetValue(netID,out ClientNet net))
            {
                return net;
            }
            return null;
        }

        public bool HasClientNet(int netID)
        {
            return clientNetDic != null && clientNetDic.ContainsKey(netID);
        }

        public void DestroyClientNet(int netID)
        {
            if(clientNetDic.TryGetValue(netID,out ClientNet net))
            {
                net.Dispose();
                clientNetDic.Remove(netID);
            }
        }

        private void DoUpdateClient(float deltaTime)
        {
            if(clientNetDic!=null)
            {
                foreach(var kvp in clientNetDic)
                {
                    kvp.Value.DoUpdate(deltaTime);
                }
            }
        }

        private void DoLateUpdateClient()
        {
            if (clientNetDic != null)
            {
                foreach (var kvp in clientNetDic)
                {
                    kvp.Value.DoLateUpdate();
                }
            }
        }
    }
}