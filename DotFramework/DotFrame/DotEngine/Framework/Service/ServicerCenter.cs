using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Framework.Service
{
    public class ServicerCenter : IServicerCenter
    {
        private Dictionary<string, IServicer> servicerDic = new Dictionary<string, IServicer>();

        public ServicerCenter()
        {
        }

        public bool Has(string name)
        {
            return servicerDic.ContainsKey(name);
        }

        public void Register(string name, IServicer member)
        {
            if (!servicerDic.ContainsKey(name))
            {
                servicerDic.Add(name, member);

                member.DoStart();
            }
        }

        public void Remove(string name)
        {
            if(servicerDic.TryGetValue(name,out IServicer servicer))
            {
                servicer.DoDispose();

                servicerDic.Remove(name);
            }
        }

        public IServicer Retrieve(string name)
        {
            if (servicerDic.TryGetValue(name, out IServicer servicer))
            {
                return servicer;
            }
            return default;
        }

        public T Retrieve<T>(string name) where T : IServicer
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
            List<IServicer> servicers = servicerDic.Values.ToList();
            servicers.ForEach((servicer) =>
            {
                servicer.DoDispose();
            });

            servicerDic.Clear();
        }
    }
}
