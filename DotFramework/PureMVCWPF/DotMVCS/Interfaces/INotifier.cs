namespace DotMVCS.Interfaces
{
    public interface INotifier
    {
        void SendNotification(string notificationName, object body = null);
    }
}
