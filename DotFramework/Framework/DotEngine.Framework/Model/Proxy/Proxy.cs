namespace DotEngine.Framework
{ 
    public abstract class Proxy: Notifier, IProxy
    {
        public Proxy()
        {
        }

        public virtual void OnRegister()
        { 
        }

        public virtual void OnRemove()
        {
        }
    }
}
