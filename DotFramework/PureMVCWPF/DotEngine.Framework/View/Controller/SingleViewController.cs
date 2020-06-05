using System;

namespace DotEngine.Framework
{
    public abstract class SingleViewController : Notifier, IViewController
    {
        public string ControllerName { get; protected set; }
        public object ViewComponent { get; set; }

        public SingleViewController(string name = null,object viewComponent = null)
        {
            if(string.IsNullOrEmpty(name))
            {
                ControllerName = Guid.NewGuid().ToString();
            }else
            {
                ControllerName = name;
            }
            ViewComponent = viewComponent;
        }

        public virtual string[] ListNotificationInterests()
        {
            return new string[0];
        }

        public virtual void HandleNotification(INotification notification)
        {
        }

        public virtual void OnRegister()
        {
        }

        public virtual void OnRemove()
        {
        }
    }
}
