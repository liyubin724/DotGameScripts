using DotEngine.Log;
using DotEngine.Net.Client;
using DotEngine.Net.Message;
using System.Collections.Generic;

namespace DotEngine.Net
{
    public partial class NetManager : Singleton<NetManager>
    {
        private Dictionary<int, ClientNet> clientNetDic = null;

        public ClientNet CreateClientNet(IMessageCrypto crypto=null, IMessageCompressor compressor=null)
        {
            int netID = idCreator.NextID;
            return CreateClientNet(netID, crypto, compressor);
        }

        public ClientNet CreateClientNet(int netID,IMessageCrypto crypto=null,IMessageCompressor compressor=null)
        {
            if (clientNetDic == null)
            {
                clientNetDic = new Dictionary<int, ClientNet>();
            }
            if (clientNetDic.ContainsKey(netID))
            {
                LogUtil.LogError(NetConst.CLIENT_LOGGER_TAG, $"NetMananger::CreateClientNet->the net has been created.netID={netID}");
                return null;
            }
            ClientNet net = new ClientNet(netID, crypto,compressor);
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
