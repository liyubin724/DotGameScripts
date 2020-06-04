using System.Collections.Generic;

namespace DotEngine.Framework
{
    public class Controller: IController
    {
        protected readonly Dictionary<string, ICommand> commandMap;
        protected IFacade Facade
        {
            get
            {
                return Framework.Facade.GetInstance();
            }
        }

        public Controller()
        {
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
                Facade.RegisterObserver(notificationName, ExecuteCommand);
            }
            commandMap[notificationName] = command;
        }

        public virtual void RemoveCommand(string notificationName)
        {
            if(commandMap.ContainsKey(notificationName))
            {
                commandMap.Remove(notificationName);

                Facade.RemoveObserver(notificationName, ExecuteCommand);
            }
        }

        public virtual bool HasCommand(string notificationName)
        {
            return commandMap.ContainsKey(notificationName);
        }
    }
}
