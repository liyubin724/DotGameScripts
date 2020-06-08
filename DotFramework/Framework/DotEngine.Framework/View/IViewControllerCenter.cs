namespace DotEngine.Framework
{
    public interface IViewControllerCenter
    {
        bool HasViewController(string name);
        void RegisterViewController(string name,IViewController viewController);
        IViewController RetrieveViewController(string name);
        IViewController RemoveViewController(string name);
    }
}
