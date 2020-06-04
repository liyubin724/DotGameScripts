namespace DotEngine.Framework
{
    public interface ICommandCenter
    {
        void RegisterCommand(string notificationName, ICommand command);
        void ExecuteCommand(INotification notification);
        void RemoveCommand(string notificationName);
        bool HasCommand(string notificationName);
    }
}
