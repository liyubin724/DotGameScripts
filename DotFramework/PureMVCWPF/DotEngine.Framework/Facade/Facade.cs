using System;

namespace DotEngine.Framework
{
    public class Facade : IFacade
    {
        protected const string SingletonMsg = "Facade Singleton already constructed!";

        protected static IFacade instance;

        protected IServiceCenter serviceCenter;
        protected IObserverCenter observerCenter;
        protected IModelCenter modelCenter;
        protected ICommandCenter commandCenter;

        protected IViewControllerCenter viewControllerCenter;

        public static IFacade GetInstance(Func<IFacade> func = null)
        {
            if(instance == null)
            {
                if(func == null)
                {
                    instance = new Facade();
                }
                else
                {
                    instance = func();
                }
            }
            return instance;
        }

        protected Facade()
        {
            if (instance != null) throw new Exception(SingletonMsg);

            instance = this;
            InitializeFacade();
        }

        protected virtual void InitializeFacade()
        {
            observerCenter = new ObserverCenter();

            InitializeService();
            InitializeModel();
            InitializeView();
            InitializeController();
        }

        protected virtual void InitializeController()
        {
            commandCenter = new CommandCenter ();
        }

        protected virtual void InitializeModel()
        {
            modelCenter = new ModelCenter ();
        }

        protected virtual void InitializeView()
        {
            viewControllerCenter = new ViewControllerCenter ();
        }

        protected virtual void InitializeService()
        {
            serviceCenter = new ServiceCenter();
        }

        public virtual void RegisterCommand(string notificationName, ICommand command)
        {
            commandCenter.RegisterCommand(notificationName, command);
        }
        public virtual void RemoveCommand(string notificationName)
        {
            commandCenter.RemoveCommand(notificationName);
        }
        public virtual bool HasCommand(string notificationName)
        {
            return commandCenter.HasCommand(notificationName);
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            modelCenter.RegisterProxy(proxy);
        }
        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return modelCenter.RetrieveProxy(proxyName);
        }
        public virtual IProxy RemoveProxy(string proxyName)
        {
            return modelCenter.RemoveProxy(proxyName);
        }
        public virtual bool HasProxy(string proxyName)
        {
            return modelCenter.HasProxy(proxyName);
        }

        public virtual void RegisterViewController(IViewController mediator)
        {
            viewControllerCenter.RegisterViewController(mediator);
        }
        public virtual IViewController RetrieveViewController(string mediatorName)
        {
            return viewControllerCenter.RetrieveViewController(mediatorName);
        }
        public virtual IViewController RemoveViewController(string mediatorName)
        {
            return viewControllerCenter.RemoveViewController(mediatorName);
        }
        public virtual bool HasViewController(string mediatorName)
        {
            return viewControllerCenter.HasViewController(mediatorName);
        }

        public virtual void SendNotification(string notificationName, object body = null)
        {
            observerCenter.NotifyObservers(new Notification(notificationName, body));
        }

        public virtual void RegisterObserver(string notificationName, Action<INotification> notifyMethod)
        {
            observerCenter.RegisterObserver(notificationName, notifyMethod);
        }

        public virtual void RemoveObserver(string notificationName, Action<INotification> notifyMethod = null)
        {
            observerCenter.RemoveObserver(notificationName, notifyMethod);
        }

        public virtual void RegisterService(IService service)
        {
            serviceCenter.RegisterService(service);
        }

        public virtual IService RetrieveService(string name)
        {
            return serviceCenter.RetrieveService(name);
        }

        public virtual void RemoveService(string name)
        {
            serviceCenter.RemoveService(name);
        }

        public virtual bool HasService(string name)
        {
            return serviceCenter.HasService(name);
        }

        public virtual void DoUpdate(float deltaTime)
        {
            serviceCenter.DoUpdate(deltaTime);
        }

        public virtual void DoUnscaleUpdate(float deltaTime)
        {
            serviceCenter.DoUnscaleUpdate(deltaTime);
        }

        public virtual void DoLateUpdate(float deltaTime)
        {
            serviceCenter.DoLateUpdate(deltaTime);
        }

        public virtual void DoFixedUpdate(float deltaTime)
        {
            serviceCenter.DoFixedUpdate(deltaTime);
        }
    }
}
