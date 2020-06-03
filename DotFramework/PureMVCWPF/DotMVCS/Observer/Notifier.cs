using DotMVCS.Interfaces;

namespace DotMVCS.Patterns.Observer
{
    public class Notifier : INotifier
    {
        public virtual void SendNotification(string notificationName, object body = null)
        {
            Facade.SendNotification(notificationName, body);
        }

        protected IFacade Facade
        {
            get
            {
                return Patterns.Facade.Facade.GetInstance(() => new Facade.Facade());
            }
        }
    }
}
