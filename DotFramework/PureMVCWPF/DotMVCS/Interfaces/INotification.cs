namespace DotMVCS.Interfaces
{
    public interface INotification
    {
        string Name { get; }
        object Body { get; set; }
    }
}
