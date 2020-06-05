namespace DotEngine.Framework
{
    public interface IViewControllerCenter
    {
        bool HasViewController(string mediatorName);
        void RegisterViewController(IViewController mediator);
        IViewController RetrieveViewController(string viewControllerName);
        IViewController RemoveViewController(string viewControllerName);
    }
}
