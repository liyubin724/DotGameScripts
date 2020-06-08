using System;
using System.Collections.Generic;
using System.Linq;
using SystemObject = System.Object;

namespace DotEngine.Context
{
    public class EnvContext<K> : IContext<K>
    {
        private Dictionary<K, SystemObject> itemDic = new Dictionary<K, SystemObject>();
        private List<K> neverClearKeyList = new List<K>();

        public SystemObject this[K key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                AddOrUpdate(key, value);
            }
        }

        public void Add(K key, SystemObject value)
        {
            Add(key, value, false);
        }

        public void Add(K key, SystemObject value, bool isNeverClear)
        {
            if (ContainsKey(key))
            {
                throw new Exception($"AContext::Add->The key has been saved into dictionry.you can use 'AddOrUpdate' to changed it.key = {key.ToString()}");
            }

            itemDic.Add(key, value);
            if(isNeverClear)
            {
                neverClearKeyList.Add(key);
            }
        }

        public void AddOrUpdate(K key,SystemObject value)
        {
            if (ContainsKey(key))
            {
                itemDic[key] = value;
            }
            else
            {
                itemDic.Add(key, value);
            }
        }

        public void AddOrUpdate(K key, SystemObject value, bool isNeverClear)
        {
            bool cachedIsNeverClear = false;
            if(ContainsKey(key))
            {
                itemDic[key] = value;
                cachedIsNeverClear = neverClearKeyList.Contains(key);
            }else
            {
                itemDic.Add(key, value);
            }

            if(cachedIsNeverClear!=isNeverClear)
            {
                if(isNeverClear)
                {
                    neverClearKeyList.Add(key);
                }else
                {
                    neverClearKeyList.Remove(key);
                }
            }
        }

        public void Update(K key, SystemObject value)
        {
            if (itemDic.TryGetValue(key, out SystemObject item))
            {
                itemDic[key] = item;
            }
            else
            {
                throw new Exception($"AContext::Update->Key not found.key = {key}");
            }
        }

        public bool ContainsKey(K key)
        {
            return itemDic.ContainsKey(key);
        }

        public SystemObject Get(K key)
        {
            if(TryGet(key,out object v))
            {
                return v;
            }
            return null;
        }

        public V Get<V>(K key)
        {
            SystemObject obj = Get(key);
            if(obj!=null)
            {
                return (V)obj;
            }
            return default;
        }

        public void Remove(K key)
        {
            if(ContainsKey(key))
            {
                itemDic.Remove(key);
                if(neverClearKeyList.Contains(key))
                {
                    neverClearKeyList.Remove(key);
                }
            }
            else
            {
                throw new Exception($"AContext::Remove->Key not found.key = {key}");
            }
        }

        public bool TryGet(K key, out SystemObject value)
        {
            if (itemDic.TryGetValue(key, out value))
            {
                return true;
            }

            value = null;
            return false;
        }

        public bool TryGet<V>(K key, out V value)
        {
            if(itemDic.TryGetValue(key,out SystemObject item))
            {
                value = (V)item;
                return true;
            }
            value = default;
            return false;
        }

        public void Clear()
        {
            Clear(false);
        }

        public void Clear(bool isForce)
        {
            if (isForce)
            {
                neverClearKeyList.Clear();
                itemDic.Clear();
            }
            else
            {
                K[] keys = itemDic.Keys.ToArray();
                foreach (var key in keys)
                {
                    if (!neverClearKeyList.Contains(key))
                    {
                        itemDic.Remove(key);
                    }
                }
            }
        }

    }
}
