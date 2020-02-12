using Dot.Dispatch;
using Dot.Log;
using System.Collections.Generic;
using EventHandler = Dot.Dispatch.EventHandler;

namespace Dot.Entity
{
    public class EntityObject
    {
        private static readonly string LOGGER_NAME = "EntityObject";

        public long UniqueID { get; set; }
        public int Category { get; set; }
        public string Name { get; set; }

        public EntityObject()
        {
            eventDispatcher = new EventDispatcher();
        }

        public void DoUpdate(float deltaTime)
        {
            foreach(var kvp in controllerDic)
            {
                if(kvp.Value.Enable)
                {
                    kvp.Value.DoUpdate(deltaTime);
                }
            }
        }

        public void DoReset()
        {
            foreach(var kvp in controllerDic)
            {
                kvp.Value.ResetController();
            }
            RemoveAllController();
        }

        #region operation for event
        private EventDispatcher eventDispatcher = null;
        public void SendEvent(int eventID, params object[] values) => eventDispatcher.TriggerEvent(eventID, 0, values);
        public void RegisterEvent(int eventID, EventHandler handler) => eventDispatcher.RegisterEvent(eventID, handler);
        public void UnregisterEvent(int eventID, EventHandler handler) => eventDispatcher.UnregisterEvent(eventID, handler);
        #endregion

        #region operation for controller
        private Dictionary<EntityControllerType, EntityControllerBase> controllerDic = new Dictionary<EntityControllerType, EntityControllerBase>();
        public T GetController<T>(EntityControllerType controllerType) where T:EntityControllerBase
        {
            if(controllerDic.TryGetValue(controllerType,out EntityControllerBase controller))
            {
                return (T)controller;
            }
            return null;
        }

        public bool HasController(EntityControllerType controllerType)
        {
            return controllerDic.ContainsKey(controllerType);
        }

        public void AddController(EntityControllerType controllerType, EntityControllerBase controller)
        {
            if (controller == null)
            {
                LogUtil.LogError(typeof(EntityObject), "EntityObject::this[index]-> value is null");
                return;
            }
            if (!controllerDic.ContainsKey(controllerType))
            {
                controllerDic.Add(controllerType, controller);
            }
            else
            {
                LogUtil.LogError(typeof(EntityObject), "EntityObject::this[index]->controller has been added.if you want to replace it,please use ReplaceController instead");
            }
        }

        public void ReplaceController(EntityControllerType controllerType, EntityControllerBase controller)
        {
            RemoveController(controllerType);
            AddController(controllerType, controller);
        }

        public void RemoveController(EntityControllerType controllerType)
        {
            if(HasController(controllerType))
            {
                controllerDic.Remove(controllerType);
            }
        }

        public void RemoveAllController()
        {
            foreach(var kvp in controllerDic)
            {
                kvp.Value.ResetController();
            }
            controllerDic.Clear();
        }
        #endregion
    }
}
