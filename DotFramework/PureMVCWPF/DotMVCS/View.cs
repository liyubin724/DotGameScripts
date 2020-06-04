using DotEngine.Interfaces;
using DotEngine.Patterns.Observer;
using System.Collections.Generic;

namespace DotEngine.Core
{
    public class View: IView
    {
        protected readonly Dictionary<string, IMediator> mediatorMap;
        protected readonly Dictionary<string, IList<IObserver>> observerMap;

        public View()
        {
            mediatorMap = new Dictionary<string, IMediator>();
            observerMap = new Dictionary<string, IList<IObserver>>();
            InitializeView();
        }

        protected virtual void InitializeView()
        {
        }

        public virtual void RegisterObserver(string notificationName, IObserver observer)
        {
            if(!observerMap.TryGetValue(notificationName,out IList<IObserver> observers))
            {
                observers = new List<IObserver>();
                observerMap.Add(notificationName,observers);
            }
            observers.Add(observer);
        }

        public virtual void NotifyObservers(INotification notification)
        {
            if (observerMap.TryGetValue(notification.Name, out var observersRef))
            {
                var observers = new List<IObserver>(observersRef);
                foreach (var observer in observers)
                {
                    observer.NotifyObserver(notification);
                }
            }
        }
        public virtual void RemoveObserver(string notificationName, object notifyContext)
        {
            if (observerMap.TryGetValue(notificationName, out var observers))
            {
                for (var i = 0; i < observers.Count; i++)
                {
                    if (observers[i].CompareNotifyContext(notifyContext))
                    {
                        observers.RemoveAt(i);
                        break;
                    }
                }

                if (observers.Count == 0)
                {
                    observerMap.Remove(notificationName);
                }
            }
        }

        public virtual void RegisterMediator(IMediator mediator)
        {
            if(mediator!=null && !string.IsNullOrEmpty(mediator.MediatorName) && !mediatorMap.ContainsKey(mediator.MediatorName))
            {
                mediatorMap.Add(mediator.MediatorName, mediator);

                var interests = mediator.ListNotificationInterests();
                if (interests!=null && interests.Length > 0)
                {
                    IObserver observer = new Observer(mediator.HandleNotification, mediator);
                    foreach (var interest in interests)
                    {
                        RegisterObserver(interest, observer);
                    }
                }
                mediator.OnRegister();
            }
        }

        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return mediatorMap.TryGetValue(mediatorName, out var mediator) ? mediator : null;
        }

        public virtual IMediator RemoveMediator(string mediatorName)
        {
            if(mediatorMap.TryGetValue(mediatorName,out IMediator mediator))
            {
                mediatorMap.Remove(mediatorName);

                var interests = mediator.ListNotificationInterests();
                foreach (var interest in interests)
                {
                    RemoveObserver(interest, mediator);
                }

                mediator.OnRemove();
            }
            return mediator;
        }

        public virtual bool HasMediator(string mediatorName)
        {
            return mediatorMap.ContainsKey(mediatorName);
        }
    }
}
