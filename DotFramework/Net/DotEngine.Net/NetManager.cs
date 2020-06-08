﻿using DotEngine.Generic;

namespace DotEngine.Net
{
    public partial class NetManager : Singleton<NetManager>
    {
        private UniqueIntID idCreator = new UniqueIntID();

        public void DoUpdate(float deltaTime)
        {
            DoUpdate_Client(deltaTime);
            DoUpdate_Server(deltaTime);
        }

        public void DoLateUpdate()
        {
            DoLateUpdate_Client();
            DoLateUpdate_Server();
        }

        public override void DoDispose()
        {
            DoDispose_Client();
            DoDispose_Server();

            base.DoDispose();
        }
    }
}
