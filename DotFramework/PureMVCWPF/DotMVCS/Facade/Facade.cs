using System;

namespace DotEngine.Framework
{
    public class Facade : IFacade
    {
        protected const string SingletonMsg = "Facade Singleton already constructed!";

        protected static IFacade instance;

        protected IServiceCenter serviceCenter;

        protected IController controller;
        protected IModel model;
        protected IView view;

        public static IFacade GetInstance()
        {
            return instance;
        }

        public static IFacade InitInstance(Func<IFacade> func = null)
        {
            if(func == null)
            {
                instance = new Facade();
            }else
            {
                instance = func();
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
            InitializeService();
            InitializeModel();
            InitializeView();
            InitializeController();
        }

        protected virtual void InitializeController()
        {
            controller = new Controller(view);
        }

        protected virtual void InitializeModel()
        {
            model = new Model();
        }

        protected virtual void InitializeView()
        {
            view = new View();
        }

        protected virtual void InitializeService()
        {
            serviceCenter = new ServiceCenter();
        }

        public virtual void RegisterCommand(string notificationName, ICommand command)
        {
            controller.RegisterCommand(notificationName, command);
        }
        public virtual void RemoveCommand(string notificationName)
        {
            controller.RemoveCommand(notificationName);
        }
        public virtual bool HasCommand(string notificationName)
        {
            return controller.HasCommand(notificationName);
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            model.RegisterProxy(proxy);
        }
        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return model.RetrieveProxy(proxyName);
        }
        public virtual IProxy RemoveProxy(string proxyName)
        {
            return model.RemoveProxy(proxyName);
        }
        public virtual bool HasProxy(string proxyName)
        {
            return model.HasProxy(proxyName);
        }

        public virtual void RegisterMediator(IMediator mediator)
        {
            view.RegisterMediator(mediator);
        }
        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return view.RetrieveMediator(mediatorName);
        }
        public virtual IMediator RemoveMediator(string mediatorName)
        {
            return view.RemoveMediator(mediatorName);
        }
        public virtual bool HasMediator(string mediatorName)
        {
            return view.HasMediator(mediatorName);
        }
        public virtual void SendNotification(string notificationName, object body = null)
        {
            NotifyObservers(new Notification(notificationName, body));
        }

        public virtual void NotifyObservers(INotification notification)
        {
            view.NotifyObservers(notification);
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
