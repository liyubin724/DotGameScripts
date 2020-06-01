using Dot.Generic;
using Dot.Proxy;

namespace Dot.Net
{
    public partial class NetManager : Singleton<NetManager>
    {
        private UniqueIntID idCreator = new UniqueIntID();

        protected override void DoInit()
        {
            UpdateProxy.GetInstance().DoUpdateHandle += DoUpdate;
            UpdateProxy.GetInstance().DoLateUpdateHandle += DoLateUpdate;
        }

        private void DoUpdate(float deltaTime)
        {
            DoUpdateClient(deltaTime);
            DoUpdateServer(deltaTime);
        }

        private void DoLateUpdate()
        {
            DoLateUpdateClient();
            DoLateUpdateServer();
        }

    }
}
