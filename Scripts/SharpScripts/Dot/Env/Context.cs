using System;
using System.Collections.Generic;

namespace Dot.Env
{
    public class Context : IContext
    {
        private Dictionary<Type, IContextObject> objectDic = new Dictionary<Type, IContextObject>();
        
        public Context() { }

        public Context(params IContextObject[] objs)
        {
            if(objs!=null && objs.Length>0)
            {
                foreach(var obj in objs)
                {
                    if(obj!=null)
                    {
                        Add(obj);
                    }
                }
            }
        }

        public void Add(IContextObject obj)
        {
            if(obj!=null)
            {
                throw new ArgumentNullException("Context::AddObject->obj is null");
            }
            Add(obj.GetType(), obj);
        }

        public void Add(Type type, IContextObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Context::AddObject->obj is null");
            }

            if (!type.IsInstanceOfType(obj))
            {
                throw new InvalidOperationException($"'{obj.GetType()}' is not of passed in type '{type}'.");
            }
            if(objectDic.ContainsKey(type))
            {
                throw new Exception($"Context::AddObject->The key is repeated.type = {type.FullName}");
            }

            objectDic.Add(type, obj);
        }

        public void Add<T>(T obj) where T : IContextObject
        {
            Add(typeof(T), obj);
        }

        public void Clear()
        {
            objectDic.Clear();
        }

        public bool Contains<T>() where T : IContextObject
        {
            return Contains(typeof(T));
        }

        public bool Contains(Type type)
        {
            return objectDic.ContainsKey(type);
        }

        public void Remove(Type type)
        {
            if(objectDic.ContainsKey(type))
            {
                objectDic.Remove(type);
            }
        }

        public void Remove(IContextObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Context::Remove->obj is null");
            }
            Remove(obj.GetType());
        }

        public void Remove<T>() where T : IContextObject
        {
            Remove(typeof(T));
        }

        public T Get<T>() where T : IContextObject
        {
            return (T)Get(typeof(T));
        }

        public IContextObject Get(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Context:GetObject->Argument is Null");

            if (TryGet(type, out IContextObject obj))
            {
                return obj;
            }

            return null;
        }

        public bool TryGet<T>(out T obj) where T : IContextObject
        {
            obj = default;
            if(TryGet(typeof(T),out IContextObject cObj))
            {
                obj = (T)cObj;
                return obj == null;
            }
            return false;
        }

        public bool TryGet(Type type, out IContextObject obj)
        {
            obj = null;
            if(objectDic.TryGetValue(type,out obj))
            {
                return true;
            }
            return false;
        }
    }
}
