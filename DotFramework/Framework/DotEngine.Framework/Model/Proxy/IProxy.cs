
namespace DotEngine.Framework
{
    public interface IProxy: INotifier
    {
        void OnRegister();
        void OnRemove();
    }
}
