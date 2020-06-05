namespace DotEngine.Framework
{
    public interface IViewController: INotifier
    {
        string ControllerName { get; }
        object ViewComponent { get; set; }

        string[] ListNotificationInterests();

        void HandleNotification(INotification notification);

        void OnRegister();
        void OnRemove();
    }
}
