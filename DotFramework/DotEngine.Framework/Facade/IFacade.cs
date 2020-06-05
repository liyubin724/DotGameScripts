using System;

namespace DotEngine.Framework
{
    public interface IFacade: INotifier,IUpdate,IUnscaleUpdate,ILateUpdate,IFixedUpdate
    {
        void RegisterService(IService service);
        IService RetrieveService(string name);
        void RemoveService(string name);
        bool HasService(string name);

        void RegisterProxy(string proxyName,IProxy proxy);
        IProxy RetrieveProxy(string proxyName);
        IProxy RemoveProxy(string proxyName);
        bool HasProxy(string proxyName);

        void RegisterCommand(string notificationName, ICommand command);
        void RemoveCommand(string notificationName);
        bool HasCommand(string notificationName);

        bool HasViewController(string viewControllerName);
        void RegisterViewController(IViewController viewController);
        IViewController RetrieveViewController(string viewControllerName);
        IViewController RemoveViewController(string viewControllerName);

        void RegisterObserver(string notificationName, Action<INotification> notifyMethod);
        void RemoveObserver(string notificationName, Action<INotification> notifyMethod);
    }
}
