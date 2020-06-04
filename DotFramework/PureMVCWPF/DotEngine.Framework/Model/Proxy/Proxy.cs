namespace DotEngine.Framework
{ 
    public abstract class Proxy: Notifier, IProxy
    {
        public const string NAME = "Proxy";

        public string ProxyName { get; protected set; }

        public Proxy(string proxyName)
        {
            ProxyName = proxyName ?? NAME;
        }

        public virtual void OnRegister()
        { 
        }

        public virtual void OnRemove()
        {
        }
    }
}
