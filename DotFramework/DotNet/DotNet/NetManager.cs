using Dot.Core;
using Dot.Core.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Net
{
    public partial class NetManager : Singleton<NetManager>
    {
        protected override void DoInit()
        {
            UpdateProxy.GetInstance().DoUpdateHandle += DoUpdate;
            UpdateProxy.GetInstance().DoLateUpdateHandle += DoLateUpdate;
        }

        private void DoLateUpdate()
        {
            DoLateUpdateClient();
        }

        private void DoUpdate(float deltaTime)
        {
            DoUpdateClient(deltaTime);
        }
    }
}
