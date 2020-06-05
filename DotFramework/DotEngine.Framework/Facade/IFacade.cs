using System;

namespace DotEngine.Framework
{
    public interface IFacade: INotifier,IUpdate,IUnscaleUpdate,ILateUpdate,IFixedUpdate
    {
        void RegisterService(string name,IService service);
        IService RetrieveService(string name);
        void RemoveService(string name);
        bool HasService(string name);

        void RegisterProxy(string name, IProxy proxy);
        IProxy RetrieveProxy(string name);
        IProxy RemoveProxy(string name);
        bool HasProxy(string name);

        void RegisterCommand(string name, ICommand command);
        void RemoveCommand(string name);
        bool HasCommand(string name);

        bool HasViewController(string name);
        void RegisterViewController(string name, IViewController viewController);
        IViewController RetrieveViewController(string name);
        IViewController RemoveViewController(string name);

        void RegisterObserver(string name, Action<INotification> notifyMethod);
        void RemoveObserver(string name, Action<INotification> notifyMethod);
    }
}
