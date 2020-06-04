using DotEngine.Interfaces;
using DotEngine.Patterns.Observer;

namespace DotEngine.Command
{
    public class SimpleCommand : Notifier, ICommand
    {
        public SimpleCommand()
        {
        }

        public virtual void Execute(INotification notification)
        {
        }
    }
}
