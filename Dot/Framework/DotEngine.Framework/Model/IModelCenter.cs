namespace DotEngine.Framework
{
    public interface IModelCenter
    {
        void RegisterProxy(string proxyName,IProxy proxy);
        IProxy RetrieveProxy(string proxyName);
        IProxy RemoveProxy(string proxyName);
        bool HasProxy(string proxyName);
    }
}
