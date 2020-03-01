using Dot.Util;

namespace Dot
{
    public delegate void UpdateHandle(float deltaTime);
    public delegate void LateUpdateHandle();

    public class UpdateProxy : Singleton<UpdateProxy>
    {
        private UpdateHandle updateHandle;
        private UpdateHandle unscaleUpdateHandle;
        private LateUpdateHandle lateUpdateHandle;

        public event UpdateHandle DoUpdateHandle
        {
            add { updateHandle += value; }
            remove { updateHandle -= value; }
        }

        public event UpdateHandle DoUnscaleUpdateHandle
        {
            add { unscaleUpdateHandle += value; }
            remove { unscaleUpdateHandle -= value; }
        }

        public event LateUpdateHandle DoLateUpdateHandle
        {
            add { lateUpdateHandle += value; }
            remove { lateUpdateHandle -= value; }
        }


        internal void DoUpdate(float deltaTime)
        {
            updateHandle?.Invoke(deltaTime);
        }

        internal void DoUnscaleUpdate(float unscaleDeltaTime)
        {
            unscaleUpdateHandle?.Invoke(unscaleDeltaTime);
        }

        internal void DoLateUpdate()
        {
            lateUpdateHandle?.Invoke();
        }

        public override void DoDispose()
        {
            updateHandle = null;
            unscaleUpdateHandle = null;
            lateUpdateHandle = null;

            base.DoDispose();
        }
    }
}
