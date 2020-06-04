namespace DotEngine.Framework
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
                return Framework.Facade.GetInstance();
            }
        }
    }
}
