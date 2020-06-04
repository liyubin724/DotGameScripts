using System;

namespace DotEngine.Interfaces
{
    public interface IObserver
    {
        Action<INotification> NotifyMethod { set; }
        object NotifyContext { set; }

        void NotifyObserver(INotification notification);

        bool CompareNotifyContext(object obj);
    }
}
