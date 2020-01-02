using System.Collections.Generic;

namespace Dot.Generic
{

    public class ListDictionary<K,V>
    {
        private List<K> keyList = new List<K>();
        private Dictionary<K, V> dataDic = new Dictionary<K, V>();

        public int Count { get => keyList.Count; }

        public bool Contains(K key)
        {
            return dataDic.ContainsKey(key);
        }

        public V GetByIndex(int index)
        {
            if (index < 0 || index > keyList.Count)
            {
                return default(V);
            }
            K key = keyList[index];
            return GetByKey(key);
        }

        public V GetByKey(K key)
        {
            if (dataDic.TryGetValue(key, out V value))
            {
                return value;
            }
            return default(V);
        }

        public void AddOrUpdate(K key,V value)
        {
            if(Contains(key))
            {
                dataDic[key] = value;
            }else
            {
                keyList.Add(key);
                dataDic.Add(key, value);
            }
        }

        public void DeleteByIndex(int index)
        {
            if (index < 0 || index > keyList.Count)
            {
                return;
            }
            K key = keyList[index];

            keyList.RemoveAt(index);
            dataDic.Remove(key);
        }

        public void DeleteByKey(K key)
        {
            int index = keyList.IndexOf(key);
            if(index>=0 && index < keyList.Count)
            {
                keyList.RemoveAt(index);
                dataDic.Remove(key);
            }
        }

        public void Clear()
        {
            keyList.Clear();
            dataDic.Clear();
        }
    }
}
