using Dot.Pool;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dot.Context
{
    internal class ContextPoolItem<T> : IObjectPoolItem
    {
        public T key = default;
        public object value = null;
        public bool isNeverClear = false;

        public void OnGet()
        {
        }

        public void OnNew()
        {
        }

        public virtual void OnRelease()
        {
            key = default;
            value = null;
            isNeverClear = false;
        }
    }

    public class AContext<T> : IContext<T>
    {
        private ObjectPool<ContextPoolItem<T>> poolItems = new ObjectPool<ContextPoolItem<T>>();
        private Dictionary<T, ContextPoolItem<T>> itemDic = new Dictionary<T, ContextPoolItem<T>>();

        public void Add(T key, object value, bool isNeverClear)
        {
            if (Contains(key))
            {
                throw new Exception($"AContext::Add->The key has been saved into dictionry.you can use 'AddOrUpdate' to changed it.key = {key.ToString()}");
            }

            ContextPoolItem<T> item = poolItems.Get();
            item.key = key;
            item.value = value;
            item.isNeverClear = isNeverClear;

            itemDic.Add(key, item);
        }

        public void AddOrUpdate(T key, object value, bool isNeverClear)
        {
            ContextPoolItem<T> item = null;
            if (Contains(key))
            {
                item = itemDic[key];
            }else
            {
                item = poolItems.Get();
                itemDic.Add(key, item);
            }
            item.key = key;
            item.value = value;
            item.isNeverClear = isNeverClear;
        }

        public void Clear(bool isForce)
        {
            T[] keys = itemDic.Keys.ToArray();
            foreach(var key in keys)
            {
                ContextPoolItem<T> item = itemDic[key];
                if(!isForce && item.isNeverClear)
                {
                    continue;
                }
                itemDic.Remove(key);
                poolItems.Release(item);
            }
        }

        public bool Contains(T key)
        {
            return itemDic.ContainsKey(key);
        }

        public object Get(T key)
        {
            if(TryGet(key,out object v))
            {
                return v;
            }
            return null;
        }

        public V Get<V>(T key)
        {
            object obj = Get(key);
            if(obj!=null)
            {
                return (V)obj;
            }
            return default;
        }

        public void Remove(T key)
        {
            if(itemDic.TryGetValue(key,out ContextPoolItem<T> item))
            {
                itemDic.Remove(key);
                poolItems.Release(item);
            }else
            {
                throw new Exception($"AContext::Remove->Key not found.key = {key}");
            }
        }

        public bool TryGet(T key, out object value)
        {
            if (itemDic.TryGetValue(key, out ContextPoolItem<T> item))
            {
                value = item.value;
                return true;
            }

            value = null;
            return false;
        }

        public bool TryGet<V>(T key, out V value)
        {
            if(itemDic.TryGetValue(key,out ContextPoolItem<T> item))
            {
                value = (V)item.value;
                return true;
            }
            value = default;
            return false;
        }

        public void Update(T key, object value)
        {
            if (itemDic.TryGetValue(key, out ContextPoolItem<T> item))
            {
                item.value = value;
            }else
            {
                throw new Exception($"AContext::Update->Key not found.key = {key}");
            }
        }
    }
}
