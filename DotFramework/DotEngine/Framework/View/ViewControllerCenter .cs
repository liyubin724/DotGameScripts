using System.Collections.Generic;
using System;

namespace DotEngine.Framework
{
    public class ViewControllerCenter : IViewControllerCenter
    {
        protected readonly Dictionary<string, IViewController> viewControllerDic;
        
        protected IFacade Facade
        {
            get
            {
                return Framework.Facade.GetInstance();
            }
        }

        public ViewControllerCenter ()
        {
            viewControllerDic = new Dictionary<string, IViewController>();
            InitializeView();
        }

        protected virtual void InitializeView()
        {
        }

        public virtual void RegisterViewController(IViewController viewController)
        {
            if(viewController == null || string.IsNullOrEmpty(viewController.ControllerName))
            {
                throw new ArgumentNullException("ViewControllerCenter::RegisterViewController->The controller or the name of controller is null");
            }
            if(viewControllerDic.ContainsKey(viewController.ControllerName))
            {
                throw new Exception($"ViewControllerCenter::RegisterViewController->The name of controller has been added.Name = {viewController.ControllerName}");
            }

            viewControllerDic.Add(viewController.ControllerName, viewController);

            var interests = viewController.ListNotificationInterests();
            if (interests != null && interests.Length > 0)
            {
                foreach (var interest in interests)
                {
                    Facade.RegisterObserver(interest, viewController.HandleNotification);
                }
            }
            viewController.OnRegister();
        }

        public virtual IViewController RetrieveViewController(string viewControllerName)
        {
            return viewControllerDic.TryGetValue(viewControllerName, out var viewController) ? viewController : null;
        }

        public virtual IViewController RemoveViewController(string viewControllerName)
        {
            if(viewControllerDic.TryGetValue(viewControllerName,out IViewController viewController))
            {
                viewControllerDic.Remove(viewControllerName);

                var interests = viewController.ListNotificationInterests();
                foreach (var interest in interests)
                {
                    Facade.RemoveObserver(interest, viewController.HandleNotification);
                }

                viewController.OnRemove();
                return viewController;
            }else
            {
                throw new KeyNotFoundException($"ViewControllerCenter::RemoveViewController->The name of controller was not found.name = {viewControllerName}");
            }
        }

        public virtual bool HasViewController(string viewControllerName)
        {
            return viewControllerDic.ContainsKey(viewControllerName);
        }
    }
}
