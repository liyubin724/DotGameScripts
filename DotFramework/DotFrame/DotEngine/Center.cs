using System.Collections.Generic;

namespace DotEngine
{
    public class Center<IInterface> : ICenter<IInterface>
    {
        private Dictionary<string, IInterface> memberDic = new Dictionary<string, IInterface>();

        public bool Has(string name)
        {
            return memberDic.ContainsKey(name);
        }

        public void Register(string name, IInterface member)
        {
            if (!memberDic.ContainsKey(name))
            {
                memberDic.Add(name, member);
            }
        }

        public void Remove(string name)
        {
            if (memberDic.ContainsKey(name))
            {
                memberDic.Remove(name);
            }
        }

        public IInterface Retrieve(string name)
        {
            if (memberDic.TryGetValue(name, out IInterface servicer))
            {
                return servicer;
            }
            return default;
        }

        public T Retrieve<T>(string name) where T : IInterface
        {
            object servicer = Retrieve(name);
            if (servicer != null)
            {
                return (T)servicer;
            }
            return default;
        }

        public void Clear()
        {
            memberDic.Clear();
        }
    }
}
