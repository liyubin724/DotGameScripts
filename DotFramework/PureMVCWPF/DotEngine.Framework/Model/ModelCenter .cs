using System.Collections.Generic;

namespace DotEngine.Framework
{
    public class ModelCenter : IModelCenter
    {
        protected readonly Dictionary<string, IProxy> proxyMap;

        public ModelCenter ()
        {
            proxyMap = new Dictionary<string, IProxy>();
            InitializeModel();
        }

        protected virtual void InitializeModel()
        {
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            if(proxy!=null && !string.IsNullOrEmpty(proxy.ProxyName) && !proxyMap.ContainsKey(proxy.ProxyName))
            {
                proxyMap.Add(proxy.ProxyName, proxy);
                
                proxy.OnRegister();
            }
        }

        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return proxyMap.TryGetValue(proxyName, out var proxy) ? proxy : null;
        }

        public virtual IProxy RemoveProxy(string proxyName)
        {
            if(proxyMap.TryGetValue(proxyName,out IProxy proxy))
            {
                proxyMap.Remove(proxyName);
                proxy.OnRemove();
            }

            return proxy;
        }

        public virtual bool HasProxy(string proxyName)
        {
            return proxyMap.ContainsKey(proxyName);
        }
    }
}
