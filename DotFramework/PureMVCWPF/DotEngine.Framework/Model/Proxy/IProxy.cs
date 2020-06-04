
namespace DotEngine.Framework
{
    public interface IProxy: INotifier
    {
        string ProxyName { get; }

        void OnRegister();
        void OnRemove();
    }
}
