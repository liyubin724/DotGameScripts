using Dot.Core;
using Dot.Net.Message;
using Dot.Net.Server;

namespace Dot.Net
{
    public partial class NetManager : Singleton<NetManager>, IServerNetCreator
    {
        private ServerNetListener netListener = null;

        public int ServerNetMaxCount { get; set; } = 10;

        public void StartupAsServer(int port)
        {
            netListener = new ServerNetListener(this);
            netListener.Startup(port, ServerNetMaxCount);
        }

        public IMessageReader GetMessageReader()
        {
            return GetReader();
        }

        public IMessageWriter GetMessageWriter()
        {
            return GetWriter();
        }

        public void DoUpdateServer(float deltaTime)
        {
            netListener?.DoUpdate(deltaTime);
        }

        public void DoLateUpdateServer()
        {
            netListener?.DoLateUpdate();
        }
    }
}
