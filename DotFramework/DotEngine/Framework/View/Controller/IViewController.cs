namespace DotEngine.Framework
{
    public interface IViewController: INotifier
    {
        string ControllerName { get; }

        string[] ListNotificationInterests();

        void HandleNotification(INotification notification);

        void OnRegister();
        void OnRemove();
    }
}
