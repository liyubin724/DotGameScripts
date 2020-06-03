using DotMVCS.Interfaces;

namespace DotMVCS.Patterns.Observer
{
    public class Notification: INotification
    {
        public string Name { get; }
        public object Body { get; set; }

        public Notification(string name, object body = null)
        {
            Name = name;
            Body = body;
        }
    }
}
