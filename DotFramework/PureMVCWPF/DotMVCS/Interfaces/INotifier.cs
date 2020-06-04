namespace DotEngine.Interfaces
{
    public interface INotifier
    {
        void SendNotification(string notificationName, object body = null);
    }
}
