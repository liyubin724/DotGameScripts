namespace DotEngine.Framework
{
    public interface INotification
    {
        string Name { get; set; }
        object Body { get; set; }

        T GetBody<T>() where T : INotification;
    }
}
