using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Framework.Proxy
{
    public interface IProxyCenter
    {
        bool Has(string name);
        void Register(string name, IProxy service);
        IProxy Retrieve(string name);
        T Retrieve<T>(string name) where T : IProxy;
        void Remove(string name);
        void Clear();
    }
}
