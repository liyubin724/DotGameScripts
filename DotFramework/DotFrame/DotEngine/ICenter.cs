namespace DotEngine
{
    public interface ICenter<IInterface>
    {
        void Register(string name, IInterface member);
        IInterface Retrieve(string name);
        T Retrieve<T>(string name) where T : IInterface;
        void Remove(string name);
        bool Has(string name);
        void Clear();
    }
}
