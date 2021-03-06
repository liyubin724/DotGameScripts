﻿using Dot.Log;
using Dot.Net.Message;
using Dot.Net.Server;
using System.Collections.Generic;

namespace Dot.Net
{
    public partial class NetManager : Singleton<NetManager>
    {
        private const int DEFAULT_MAX_CLIENT_COUNT = 100;
        private Dictionary<int, ServerNetListener> serverNetListenerDic = null;

        public ServerNetListener CreateServerNet(int listenPort, int maxClientNetCount = DEFAULT_MAX_CLIENT_COUNT, IMessageCrypto crypto = null, IMessageCompressor compressor = null)
        {
            int netID = idCreator.NextID;
            return CreateServerNet(netID, listenPort, maxClientNetCount, crypto, compressor);
        }

        public ServerNetListener CreateServerNet(int netID,int listenPort,int maxClientNetCount = DEFAULT_MAX_CLIENT_COUNT,IMessageCrypto crypto = null ,IMessageCompressor compressor = null)
        {
            if (clientNetDic == null)
            {
                serverNetListenerDic = new Dictionary<int, ServerNetListener>();
            }
            if (serverNetListenerDic.ContainsKey(netID))
            {
                LogUtil.LogError(typeof(NetManager), $"NetMananger::CreateServerNet->the net has been created.netID={netID}");
                return null;
            }

            ServerNetListener serverNet = new ServerNetListener(netID, crypto, compressor);
            serverNet.Startup("127.0.0.1", listenPort, maxClientNetCount);

            serverNetListenerDic.Add(netID, serverNet);

            return serverNet;
        }


        public ServerNetListener GetServerNet(int netID)
        {
            if (serverNetListenerDic != null && serverNetListenerDic.TryGetValue(netID, out ServerNetListener net))
            {
                return net;
            }
            return null;
        }

        public bool HasServerNet(int netID)
        {
            return serverNetListenerDic != null && serverNetListenerDic.ContainsKey(netID);
        }

        public void DestroyServerNet(int netID)
        {
            if (serverNetListenerDic.TryGetValue(netID, out ServerNetListener net))
            {
                net.Dispose();
                serverNetListenerDic.Remove(netID);
            }
        }

        public void DoUpdateServer(float deltaTime)
        {
            if(serverNetListenerDic!=null && serverNetListenerDic.Count>0)
            {
                foreach(var kvp in serverNetListenerDic)
                {
                    kvp.Value.DoUpdate(deltaTime);
                }
            }
        }

        public void DoLateUpdateServer()
        {
            if (serverNetListenerDic != null && serverNetListenerDic.Count > 0)
            {
                foreach (var kvp in serverNetListenerDic)
                {
                    kvp.Value.DoLateUpdate();
                }
            }
        }
    }
}
