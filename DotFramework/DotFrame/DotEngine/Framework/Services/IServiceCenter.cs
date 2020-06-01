namespace DotEngine.Framework.Services
{
    public interface IServiceCenter : IUpdate, IUnscaleUpdate, ILateUpdate, IFixedUpdate
    {
        bool Has(string name);
        void Register(string name, IService service);
        IService Retrieve(string name);
        T Retrieve<T>(string name) where T : IService;
        void Remove(string name);
        void Clear();
    }
}
