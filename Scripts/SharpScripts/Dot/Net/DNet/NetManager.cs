using Dot.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Net
{
    public class NetManager : Singleton<NetManager>
    {
        protected override void DoInit()
        {
            DotProxy.proxy.DoUpdate += DoUpdate;
        }

        private void DoUpdate(float deltaTime)
        {
        }

        public override void DoDispose()
        {
            DotProxy.proxy.DoUpdate -= DoUpdate;

            base.DoDispose();
        }
    }
}
