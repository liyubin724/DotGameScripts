namespace DotEngine.Framework
{
    public class Service : Notifier,IService
    {
        public string Name { get; }

        public Service(string name)
        {
            Name = name;
        }

        public virtual void DoRegister()
        {
        }

        public virtual void DoRemove()
        {
        }
    }
}
