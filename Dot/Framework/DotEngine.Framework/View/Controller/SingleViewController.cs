using System;

namespace DotEngine.Framework
{
    public abstract class SingleViewController : Notifier, IViewController
    {
        public SingleViewController()
        {
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
