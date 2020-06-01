using DotEngine.Framework.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Framework.Proxy
{
    public interface IProxy : INotifier
    {
        void OnRegister();
        void OnRemove();
    }
}
