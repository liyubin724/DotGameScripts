namespace Dot.Context
{
    public interface IContext<T>
    {
        bool Contains(T key);
        object Get(T key);
        V Get<V>(T key);
        void Add(T key, object value,bool isNeverClear);
        void Update(T key, object value);
        void AddOrUpdate(T key, object value, bool isNeverClear);
        void Remove(T key);
        bool TryGet(T key, out object value);
        bool TryGet<V>(T key, out V value);
        void Clear(bool isForce);
    }
}
