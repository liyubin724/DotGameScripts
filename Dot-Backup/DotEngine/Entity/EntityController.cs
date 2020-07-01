using Dot.Dispatch;
using System.Collections.Generic;
using SystemObject = System.Object;

namespace Dot.Entity
{
    public abstract class EntityController
    {
        protected EntityObject entity;

        private bool isEnable = true;
        public bool Enable
        {
            get
            {
                return isEnable;
            }
            set
            {
                isEnable = value;
                entity.SetControllerUpdateState(this);
                entity.SetControllerLateUpdateState(this);
                entity.SetControllerFixedUpdateState(this);
            }
        }

        private bool isUpdateEnable = false;
        private bool isLateUpdateEnable = false;
        private bool isFixedUpdateEnable = false;

        public bool EnableUpdate
        {
            get
            {
                return isUpdateEnable;
            }
            set
            {
                if(value!=isUpdateEnable)
                {
                    isUpdateEnable = value;
                    entity.SetControllerUpdateState(this);
                }
            }
        }
        public bool EnableLateUpdate
        {
            get
            {
                return isLateUpdateEnable;
            }
            set
            {
                if(isLateUpdateEnable!=value)
                {
                    isLateUpdateEnable = value;
                    entity.SetControllerLateUpdateState(this);
                }
            }
        }
        public bool EnableFixedUpdate
        {
            get
            {
                return isFixedUpdateEnable;
            }
            set
            {
                if(isFixedUpdateEnable!=value)
                {
                    isFixedUpdateEnable = value;
                    entity.SetControllerFixedUpdateState(this);
                }
            }
        }

        protected EntityController()
        {
        }

        internal void Initialize(EntityObject entity)
        {
            this.entity = entity;
            OnInitialized();
        }

        protected abstract void OnInitialized();

        protected internal virtual void DoUpdate(float deltaTime)
        {

        }

        protected internal virtual void DoLateUpdate(float deltaTime)
        {

        }

        protected internal virtual void DoFixedUpdate(float deltaTime)
        {
            
        }

        protected internal virtual void DoDestroy()
        {

        }
        #region Event

        private Dictionary<int, List<EventHandler>> eventHandlerDic = new Dictionary<int, List<EventHandler>>();

        protected virtual void DoRegisterEvent() { }
        protected virtual void DoUnregisterEvent() { }

        protected void RegisterEvent(int eventID, EventHandler handler)
        {
            if(!eventHandlerDic.TryGetValue(eventID,out List<EventHandler> handlers))
            {
                handlers = new List<EventHandler>();
                eventHandlerDic.Add(eventID,handlers);
            }

            handlers.Add(handler);

            entity.EntityDispatcher.RegisterEvent(eventID, handler);
        }

        protected void UnregisterEvent(int eventID,EventHandler handler)
        {
            if(eventHandlerDic.TryGetValue(eventID,out List<EventHandler> handlers))
            {
                for(int i =handlers.Count-1;i>=0;--i)
                {
                    if(handlers[i] == handler)
                    {
                        handlers.RemoveAt(i);
                    }
                }
            }

            entity.EntityDispatcher.UnregisterEvent(eventID, handler);
        }

        protected void UnregisterAllEvent()
        {
            foreach(var kvp in eventHandlerDic)
            {
                if(kvp.Value.Count>0)
                {
                    foreach(var handler in kvp.Value)
                    {
                        entity.EntityDispatcher.UnregisterEvent(kvp.Key, handler);
                    }
                }
                kvp.Value.Clear();
            }
        }

        protected void SendEvent(int eventID,params SystemObject[] values)
        {
            entity.EntityDispatcher.TriggerEvent(eventID, values);
        }
        #endregion
    }
}
