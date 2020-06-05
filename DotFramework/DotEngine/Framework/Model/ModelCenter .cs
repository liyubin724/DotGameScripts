using System.Collections.Generic;

namespace DotEngine.Framework
{
    public class ModelCenter : IModelCenter
    {
        protected readonly Dictionary<string, IProxy> proxyDic;

        public ModelCenter ()
        {
            proxyDic = new Dictionary<string, IProxy>();
            InitializeModel();
        }

        protected virtual void InitializeModel()
        {
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            if(proxy!=null && !string.IsNullOrEmpty(proxy.ProxyName) && !proxyDic.ContainsKey(proxy.ProxyName))
            {
                proxyDic.Add(proxy.ProxyName, proxy);
                
                proxy.OnRegister();
            }
        }

        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return proxyDic.TryGetValue(proxyName, out var proxy) ? proxy : null;
        }

        public virtual IProxy RemoveProxy(string proxyName)
        {
            if(proxyDic.TryGetValue(proxyName,out IProxy proxy))
            {
                proxyDic.Remove(proxyName);
                proxy.OnRemove();
            }

            return proxy;
        }

        public virtual bool HasProxy(string proxyName)
        {
            return proxyDic.ContainsKey(proxyName);
        }
    }
}
