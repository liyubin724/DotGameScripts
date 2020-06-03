using System;

namespace DotMVCS.Interfaces
{
    public interface IController
    {
        void RegisterCommand(string notificationName, ICommand command);
        void ExecuteCommand(INotification notification);
        void RemoveCommand(string notificationName);
        bool HasCommand(string notificationName);
    }
}
