namespace DotEngine.Framework.Notify
{
    public interface INotification
    {
        string Name { get; set; }
        object Body { get; set; }
    }
}
