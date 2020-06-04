namespace DotEngine.Framework
{ 
    public abstract class Proxy: Notifier, IProxy
    {
        public string ProxyName { get; protected set; }

        public Proxy(string proxyName)
        {
            ProxyName = proxyName;
        }

        public virtual void OnRegister()
        { 
        }

        public virtual void OnRemove()
        {
        }
    }
}
