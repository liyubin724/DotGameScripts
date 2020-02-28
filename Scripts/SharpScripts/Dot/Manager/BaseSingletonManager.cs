using Dot.Util;
using System.Collections.Generic;

namespace Dot.Manager
{
    public delegate void UpdateHandle(float deltaTime);
    public delegate void LateUpdateHandle();

    public class ManagerProxy : Singleton<ManagerProxy>
    {
        private List<IManager> managers = new List<IManager>();

        internal event UpdateHandle updateHandle;
        internal event UpdateHandle unscaleUpdateHandle;
        internal event LateUpdateHandle lateUpdateHandle;

        public ManagerProxy()
        {
        }

        public void AddManager(IManager mgr)
        {
            managers.Add(mgr);
        }

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            updateHandle?.Invoke(deltaTime);
            unscaleUpdateHandle?.Invoke(unscaleDeltaTime);
        }

        public void DoLateUpdate()
        {
            lateUpdateHandle?.Invoke();
        }

        public override void DoDispose()
        {
            foreach(var mgr in managers)
            {
                mgr.DoDispose();
            }
            managers.Clear();

            base.DoDispose();
        }
    }

    public interface IManager
    {
        void DoInit();
        void DoDispose();
    }

    public class BaseSingletonManager<T> : IManager where T:BaseSingletonManager<T>,new()
    {
        private static T instance = null;

        public static T GetInstance()
        {
            if(instance == null)
            {
                instance = new T();
                instance.DoInit();
            }

            return instance;
        }

        private bool isBindUpdate = false;
        private bool isBindUnscaleUpdate = false;
        private bool isBindLateUpdate = false;

        protected BaseSingletonManager()
        {
        }

        public virtual void DoInit()
        {
            ManagerProxy.GetInstance().AddManager(this);
        }

        protected void BindUpdate(bool isUpdate,bool isUnscaleUpdate,bool isLateUpdate)
        {
            ManagerProxy mgrProxy = ManagerProxy.GetInstance();
            if(isBindUpdate != isUpdate)
            {
                if(isBindUpdate)
                {
                    ManagerProxy.GetInstance().updateHandle -= DoUpdate;
                }else if(isUpdate)
                {
                    ManagerProxy.GetInstance().updateHandle += DoUpdate;
                }
                isBindUpdate = isUpdate;
            }

            if(isBindUnscaleUpdate != isUnscaleUpdate)
            {
                if (isBindUnscaleUpdate)
                {
                    ManagerProxy.GetInstance().unscaleUpdateHandle -= DoUnscaleUpdate;
                }
                else if (isUnscaleUpdate)
                {
                    ManagerProxy.GetInstance().unscaleUpdateHandle += DoUnscaleUpdate;
                }
                isBindUnscaleUpdate = isUnscaleUpdate;
            }

            if (isBindLateUpdate != isLateUpdate)
            {
                if (isBindLateUpdate)
                {
                    ManagerProxy.GetInstance().lateUpdateHandle -= DoLateUpdate;
                }
                else if (isLateUpdate)
                {
                    ManagerProxy.GetInstance().lateUpdateHandle += DoLateUpdate;
                }
                isBindLateUpdate = isLateUpdate;
            }
        }

        protected virtual void DoUpdate(float deltaTime)
        {

        }

        protected virtual void DoUnscaleUpdate(float unscaleDeltaTime)
        {

        }

        protected virtual void DoLateUpdate()
        {

        }

        public virtual void DoDispose()
        {
            if(isBindUpdate)
            {
                ManagerProxy.GetInstance().updateHandle -= DoUpdate;
            }
            if(isBindUnscaleUpdate)
            {
                ManagerProxy.GetInstance().unscaleUpdateHandle -= DoUnscaleUpdate;
            }
            if(isBindLateUpdate)
            {
                ManagerProxy.GetInstance().lateUpdateHandle -= DoLateUpdate;
            }
            instance = null;
        }
    }
}
