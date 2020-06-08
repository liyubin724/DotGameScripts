using DotEngine.Generic;

namespace DotEngine.Net
{
    public partial class NetManager : Singleton<NetManager>
    {
        private UniqueIntID idCreator = new UniqueIntID();

        public void DoUpdate(float deltaTime)
        {
            DoUpdateClient(deltaTime);
            DoUpdateServer(deltaTime);
        }

        public void DoLateUpdate()
        {
            DoLateUpdateClient();
            DoLateUpdateServer();
        }
    }
}
