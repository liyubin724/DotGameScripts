using System;

namespace DotEngine.Interfaces
{
    public interface IController
    {
        void RegisterCommand(string notificationName, ICommand command);
        void ExecuteCommand(INotification notification);
        void RemoveCommand(string notificationName);
        bool HasCommand(string notificationName);
    }
}
