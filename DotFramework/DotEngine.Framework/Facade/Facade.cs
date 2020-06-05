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

        public virtual void RegisterCommand(string name, ICommand command)
        {
            commandCenter.RegisterCommand(name, command);
        }
        public virtual void RemoveCommand(string name)
        {
            commandCenter.RemoveCommand(name);
        }
        public virtual bool HasCommand(string name)
        {
            return commandCenter.HasCommand(name);
        }

        public virtual void RegisterProxy(string name,IProxy proxy)
        {
            modelCenter.RegisterProxy(name,proxy);
        }
        public virtual IProxy RetrieveProxy(string name)
        {
            return modelCenter.RetrieveProxy(name);
        }
        public virtual IProxy RemoveProxy(string name)
        {
            return modelCenter.RemoveProxy(name);
        }
        public virtual bool HasProxy(string name)
        {
            return modelCenter.HasProxy(name);
        }

        public virtual void RegisterViewController(string name,IViewController viewController)
        {
            viewControllerCenter.RegisterViewController(name, viewController);
        }
        public virtual IViewController RetrieveViewController(string name)
        {
            return viewControllerCenter.RetrieveViewController(name);
        }
        public virtual IViewController RemoveViewController(string name)
        {
            return viewControllerCenter.RemoveViewController(name);
        }
        public virtual bool HasViewController(string name)
        {
            return viewControllerCenter.HasViewController(name);
        }

        public virtual void SendNotification(string name, object body = null)
        {
            observerCenter.NotifyObservers(new Notification(name, body));
        }

        public virtual void RegisterObserver(string name, Action<INotification> notifyMethod)
        {
            observerCenter.RegisterObserver(name, notifyMethod);
        }

        public virtual void RemoveObserver(string name, Action<INotification> notifyMethod = null)
        {
            observerCenter.RemoveObserver(name, notifyMethod);
        }

        public virtual void RegisterService(string name, IService service)
        {
            serviceCenter.RegisterService(name, service);
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
