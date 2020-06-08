using System.Collections.Generic;
using System;

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

        public virtual void RegisterProxy(string proxyName,IProxy proxy)
        {
            if(proxy == null || string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("The proxy or the name of proxy is empty");
            }

            if(proxyDic.ContainsKey(proxyName))
            {
                throw new Exception($"The name of proxy has been added.name = {proxyName}.");
            }

            proxyDic.Add(proxyName, proxy);
            proxy.OnRegister();
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
