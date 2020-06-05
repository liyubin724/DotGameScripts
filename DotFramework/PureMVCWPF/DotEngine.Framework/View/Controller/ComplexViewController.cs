using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Framework
{
    public abstract class ComplexViewController : Notifier, IViewController
    {
        public string ControllerName { get; private set; }

        public object ViewComponent { get; set; }

        protected IList<IViewController> subControllers = null;

        public ComplexViewController(string name = null, object viewComponent = null)
        {
            subControllers = new List<IViewController>();

            if (string.IsNullOrEmpty(name))
            {
                ControllerName = Guid.NewGuid().ToString();
            }
            else
            {
                ControllerName = name;
            }

            ViewComponent = viewComponent;
        }

        public void AddSubController(IViewController subViewController)
        {
            subControllers.Add(subViewController);

            Facade.RegisterViewController(subViewController);
        }

        public void RemoveSubController(IViewController subViewController)
        {
            if(subControllers.Remove(subViewController))
            {
                Facade.RemoveViewController(subViewController.ControllerName);
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
            
        }
    }
}
