using System;
using System.Collections.Generic;

namespace DotEngine.Framework
{
    public abstract class ComplexViewController : Notifier, IViewController
    {
        protected Dictionary<string, IViewController> subControllerDic = null;

        public ComplexViewController()
        {
            subControllerDic = new Dictionary<string, IViewController>();
        }

        public virtual void AddSubViewController(IViewController viewController)
        {
            AddSubViewController(Guid.NewGuid().ToString(), viewController);
        }

        public virtual void AddSubViewController(string name,IViewController viewController)
        {
            if(string.IsNullOrEmpty(name) || viewController == null)
            {
                throw new ArgumentNullException("The viewController or the name of viewController is empty");
            }
            if(subControllerDic.ContainsKey(name))
            {
                throw new Exception($"The name of viewController has been added.name = {name}.");
            }

            subControllerDic.Add(name, viewController);
            Facade.RegisterViewController(name, viewController);
        }

        public virtual void RemoveSubViewController(IViewController viewController)
        {
            foreach(var kvp in subControllerDic)
            {
                if(kvp.Value == viewController)
                {
                    RemoveSubViewController(kvp.Key);
                    break;
                }
            }
        }

        public virtual void RemoveSubViewController(string name)
        {
            if(subControllerDic.ContainsKey(name))
            {
                Facade.RemoveViewController(name);
                subControllerDic.Remove(name);
            }
        }

        public virtual void HandleNotification(INotification notification)
        {
            
        }

        public virtual string[] ListNotificationInterests()
        {
            return new string[0];
        }

        public virtual void OnRegister()
        {
            
        }

        public virtual void OnRemove()
        {
            foreach(var kvp in subControllerDic)
            {
                Facade.RemoveViewController(kvp.Key);
            }
            subControllerDic.Clear();
        }
    }
}
