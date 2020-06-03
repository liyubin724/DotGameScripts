using System;
using DotMVCS.Interfaces;
using DotMVCS.Core;
using DotMVCS.Patterns.Observer;

namespace DotMVCS.Patterns.Facade
{
    public class Facade : IFacade
    {
        protected const string SingletonMsg = "Facade Singleton already constructed!";

        protected static IFacade instance;

        protected IController controller;
        protected IModel model;
        protected IView view;

        public static IFacade GetInstance(Func<IFacade> facadeFunc)
        {
            if (instance == null)
            {
                instance = facadeFunc();
            }
            return instance;
        }

        public Facade()
        {
            if (instance != null) throw new Exception(SingletonMsg);
            instance = this;
            InitializeFacade();
        }

        protected virtual void InitializeFacade()
        {
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

    }
}
