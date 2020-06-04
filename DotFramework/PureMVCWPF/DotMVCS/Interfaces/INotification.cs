namespace DotEngine.Framework
{
    public interface INotification
    {
        string Name { get; }
        object Body { get; set; }
    }
}
