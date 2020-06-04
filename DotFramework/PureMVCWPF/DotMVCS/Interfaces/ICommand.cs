namespace DotEngine.Framework
{
    public interface ICommand: INotifier
    {
        void Execute(INotification notification);
    }
}
