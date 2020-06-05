using System.Collections.Generic;

namespace DotEngine.Framework
{
    public class CommandCenter : ICommandCenter
    {
        protected readonly Dictionary<string, ICommand> commandDic;
        protected IFacade Facade
        {
            get
            {
                return Framework.Facade.GetInstance();
            }
        }

        public CommandCenter ()
        {
            commandDic = new Dictionary<string, ICommand>();
            InitializeController();
        }

        protected virtual void InitializeController()
        {
        }

        public virtual void ExecuteCommand(INotification notification)
        {
            if (commandDic.TryGetValue(notification.Name, out var command))
            {
                command.Execute(notification);
            }
        }

        public virtual void RegisterCommand(string notificationName, ICommand command)
        {
            if (commandDic.TryGetValue(notificationName, out _) == false)
            {
                Facade.RegisterObserver(notificationName, ExecuteCommand);
            }
            commandDic[notificationName] = command;
        }

        public virtual void RemoveCommand(string notificationName)
        {
            if(commandDic.ContainsKey(notificationName))
            {
                commandDic.Remove(notificationName);

                Facade.RemoveObserver(notificationName, ExecuteCommand);
            }
        }

        public virtual bool HasCommand(string notificationName)
        {
            return commandDic.ContainsKey(notificationName);
        }
    }
}
