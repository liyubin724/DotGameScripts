using DotEngine.Interfaces;
using DotEngine.Patterns.Observer;
using System.Collections.Generic;

namespace DotEngine.Command
{
    public class MacroCommand : Notifier, ICommand
    {
        private readonly IList<ICommand> subcommands;

        public MacroCommand()
        {
            subcommands = new List<ICommand>();
            InitializeMacroCommand();
        }

        protected virtual void InitializeMacroCommand()
        {
        }

        protected void AddSubCommand(ICommand command)
        {
            subcommands.Add(command);
        }

        public virtual void Execute(INotification notification)
        {
            for (int i = 0; i < subcommands.Count; ++i)
            {
                subcommands[i].Execute(notification);
            }
        }
    }
}
