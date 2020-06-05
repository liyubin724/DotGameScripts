namespace DotEngine.Framework
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
