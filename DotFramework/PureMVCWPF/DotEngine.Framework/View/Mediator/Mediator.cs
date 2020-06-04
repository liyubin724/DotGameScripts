﻿namespace DotEngine.Framework
{
    public class Mediator : Notifier, IMediator
    {
        public string MediatorName { get; protected set; }
        public object ViewComponent { get; set; }

        public static string NAME = "Mediator";

        public Mediator(string mediatorName, object viewComponent = null)
        {
            MediatorName = mediatorName ?? NAME;
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
