namespace DotEngine.Framework.Service
{ 
    public interface IServicerCenter
    {
        void Register(string name, IServicer member);
        IServicer Retrieve(string name);
        T Retrieve<T>(string name) where T : IServicer;
        void Remove(string name);
        bool Has(string name);
        void Clear();
    }
}
