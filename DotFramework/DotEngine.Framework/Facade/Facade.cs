using System;

namespace DotEngine.Framework
{
    public class Facade : IFacade
    {
        protected static IFacade instance;

        protected IServiceCenter serviceCenter;
        protected IObserverCenter observerCenter;
        protected IModelCenter modelCenter;
        protected ICommandCenter commandCenter;
        protected IViewControllerCenter viewControllerCenter;

        public static IFacade GetInstance()
        {
            if(instance == null)
            {
                instance = new Facade();
            }
            return instance;
        }

        protected Facade()
        {
            instance = this;
            InitializeFacade();
        }

        #region Initialize
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
        #endregion

        #region Command
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
        #endregion

        #region Proxy
        public virtual void RegisterProxy(string name,IProxy proxy)
        {
            modelCenter.RegisterProxy(name,proxy);
        }
        public virtual T RetrieveProxy<T>(string name) where T:IProxy
        {
            return (T)modelCenter.RetrieveProxy(name);
        }

        public virtual IProxy RemoveProxy(string name)
        {
            return modelCenter.RemoveProxy(name);
        }
        public virtual bool HasProxy(string name)
        {
            return modelCenter.HasProxy(name);
        }
        #endregion

        #region ViewController
        public virtual void RegisterViewController(string name,IViewController viewController)
        {
            viewControllerCenter.RegisterViewController(name, viewController);
        }
        public virtual T RetrieveViewController<T>(string name) where T:IViewController
        {
            return (T)viewControllerCenter.RetrieveViewController(name);
        }
        public virtual IViewController RemoveViewController(string name)
        {
            return viewControllerCenter.RemoveViewController(name);
        }
        public virtual bool HasViewController(string name)
        {
            return viewControllerCenter.HasViewController(name);
        }
        #endregion

        #region Notification && Observer
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
        #endregion

        #region Service
        public virtual void RegisterService(string name, IService service)
        {
            serviceCenter.RegisterService(name, service);
        }

        public virtual T RetrieveService<T>(string name)where T:IService
        {
            return (T)serviceCenter.RetrieveService(name);
        }

        public virtual void RemoveService(string name)
        {
            serviceCenter.RemoveService(name);
        }

        public virtual bool HasService(string name)
        {
            return serviceCenter.HasService(name);
        }

        #endregion

        #region Update
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
        #endregion
    }
}
