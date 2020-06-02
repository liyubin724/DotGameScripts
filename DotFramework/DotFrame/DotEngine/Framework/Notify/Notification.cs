namespace DotEngine.Framework.Notify
{
    public class Notification : INotification
    {
        public string Name { get; set; }
        public object Body { get; set; }

        public Notification(string name):this(name,null)
        {
        }

        public Notification(string name,object body)
        {
            Name = name;
            Body = body;
        }

        public T GetBody<T>() where T: INotification
        {
            return (T)Body;
        }
    }
}
