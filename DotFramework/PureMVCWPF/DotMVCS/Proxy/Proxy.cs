using DotEngine.Interfaces;
using DotEngine.Patterns.Observer;

namespace DotEngine.Patterns.Proxy
{
    public class Proxy: Notifier, IProxy
    {
        public const string NAME = "Proxy";
        public string ProxyName { get; protected set; }
        public object Data { get; set; }

        public Proxy(string proxyName, object data = null)
        {
            ProxyName = proxyName ?? NAME;
            if (data != null) Data = data;
        }

        public virtual void OnRegister()
        { 
        }

        public virtual void OnRemove()
        {
        }
    }
}
