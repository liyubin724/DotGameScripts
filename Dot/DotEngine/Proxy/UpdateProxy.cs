namespace Dot.Proxy
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

        public void DoUpdate(float deltaTime)
        {
            updateHandle?.Invoke(deltaTime);
        }

        public void DoUnscaleUpdate(float unscaleDeltaTime)
        {
            unscaleUpdateHandle?.Invoke(unscaleDeltaTime);
        }

        public void DoLateUpdate()
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
