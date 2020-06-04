using System.Collections.Generic;

namespace DotEngine.Framework
{
    public class Controller: IController
    {
        protected IView view;
        protected readonly Dictionary<string, ICommand> commandMap;

        public Controller(IView view)
        {
            this.view = view;
            commandMap = new Dictionary<string, ICommand>();
            InitializeController();
        }

        protected virtual void InitializeController()
        {
        }

        public virtual void ExecuteCommand(INotification notification)
        {
            if (commandMap.TryGetValue(notification.Name, out var command))
            {
                command.Execute(notification);
            }
        }

        public virtual void RegisterCommand(string notificationName, ICommand command)
        {
            if (commandMap.TryGetValue(notificationName, out _) == false)
            {
                view.RegisterObserver(notificationName, new Observer(ExecuteCommand, this));
            }
            commandMap[notificationName] = command;
        }

        public virtual void RemoveCommand(string notificationName)
        {
            if(commandMap.ContainsKey(notificationName))
            {
                commandMap.Remove(notificationName);
                view.RemoveObserver(notificationName, this);
            }
        }

        public virtual bool HasCommand(string notificationName)
        {
            return commandMap.ContainsKey(notificationName);
        }
    }
}
