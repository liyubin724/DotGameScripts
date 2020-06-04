using DotEngine.Interfaces;
using System.Collections.Generic;

namespace DotEngine.Core
{
    public class Model: IModel
    {
        protected readonly Dictionary<string, IProxy> proxyMap;

        public Model()
        {
            proxyMap = new Dictionary<string, IProxy>();
            InitializeModel();
        }

        protected virtual void InitializeModel()
        {
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            proxyMap[proxy.ProxyName] = proxy;
            proxy.OnRegister();
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
