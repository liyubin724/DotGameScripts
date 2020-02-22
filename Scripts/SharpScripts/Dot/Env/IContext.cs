using System;

namespace Dot.Env
{
    public interface IContext
    {
        bool Contains<T>() where T : IContextObject;
        bool Contains(Type type);

        T Get<T>() where T : IContextObject;
        IContextObject Get(Type type);

        void Add(IContextObject obj);
        void Add(Type type, IContextObject obj);
        void Add<T>(T obj) where T : IContextObject;

        void Remove(Type type);
        void Remove(IContextObject obj);
        void Remove<T>() where T : IContextObject;

        bool TryGet<T>(out T obj) where T : IContextObject;
        bool TryGet(Type type, out IContextObject obj);

        void Clear();
    }
}
