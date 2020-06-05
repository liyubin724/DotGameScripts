using System;

namespace DotEngine.Framework
{
    public interface IObserverCenter
    {
        void RegisterObserver(string notificationName, Action<INotification> notifyMethod);
        void RemoveObserver(string notificationName,Action<INotification> notifyMethod);

        void NotifyObservers(INotification notification);
    }
}
