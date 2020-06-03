using DotMVCS.Interfaces;
using DotMVCS.Patterns.Observer;

namespace DotMVCS.Command
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
