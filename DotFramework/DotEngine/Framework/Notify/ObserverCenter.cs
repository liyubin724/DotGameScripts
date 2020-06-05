using System;
using System.Collections.Generic;

namespace DotEngine.Framework
{
    public class ObserverCenter : IObserverCenter
    {
        private Dictionary<string, IList<Action<INotification>>> observerDic = null;

        public ObserverCenter()
        {
            observerDic = new Dictionary<string, IList<Action<INotification>>>();
        }

        public void NotifyObservers(INotification notification)
        {
            if(notification!=null &&!string.IsNullOrEmpty(notification.Name))
            {
                if(observerDic.TryGetValue(notification.Name,out IList<Action<INotification>> observers))
                {
                    var tempObservers = new List<Action<INotification>>(observers);
                    foreach(var observer in tempObservers)
                    {
                        observer?.Invoke(notification);
                    }
                }
            }
        }

        public void RegisterObserver(string notificationName, Action<INotification> notifyMethod)
        {
            if (!observerDic.TryGetValue(notificationName, out IList<Action<INotification>> observers))
            {
                observers = new List<Action<INotification>>();
                observerDic.Add(notificationName, observers);
            }
            observers.Add(notifyMethod);
        }

        public void RemoveObserver(string notificationName)
        {
            if(observerDic.ContainsKey(notificationName))
            {
                observerDic.Remove(notificationName);
            }
        }

        public void RemoveObserver(string notificationName, Action<INotification> notifyMethod)
        {
            if (observerDic.TryGetValue(notificationName, out IList<Action<INotification>> observers))
            {
                if(notifyMethod == null)
                {
                    observerDic.Remove(notificationName);
                }else
                {
                    for (int i = observers.Count - 1; i >= 0; --i)
                    {
                        if (observers[i] == notifyMethod)
                        {
                            observers.RemoveAt(i);
                        }
                    }

                    if (observers.Count == 0)
                    {
                        observerDic.Remove(notificationName);
                    }
                }
            }
        }
    }
}
