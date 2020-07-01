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

        public virtual void RegisterViewController(string name,IViewController viewController)
        {
            if(viewController == null || string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("ViewControllerCenter::RegisterViewController->The controller or the name of controller is null");
            }
            if(viewControllerDic.ContainsKey(name))
            {
                throw new Exception($"ViewControllerCenter::RegisterViewController->The name of controller has been added.Name = {name}");
            }

            viewControllerDic.Add(name, viewController);

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

        public virtual IViewController RetrieveViewController(string name)
        {
            return viewControllerDic.TryGetValue(name, out var viewController) ? viewController : null;
        }

        public virtual IViewController RemoveViewController(string name)
        {
            if(viewControllerDic.TryGetValue(name,out IViewController viewController))
            {
                viewControllerDic.Remove(name);

                var interests = viewController.ListNotificationInterests();
                foreach (var interest in interests)
                {
                    Facade.RemoveObserver(interest, viewController.HandleNotification);
                }

                viewController.OnRemove();
                return viewController;
            }else
            {
                throw new KeyNotFoundException($"ViewControllerCenter::RemoveViewController->The name of controller was not found.name = {name}");
            }
        }

        public virtual bool HasViewController(string name)
        {
            return viewControllerDic.ContainsKey(name);
        }
    }
}
