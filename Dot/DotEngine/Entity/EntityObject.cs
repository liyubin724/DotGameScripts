using Dot.Dispatch;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using SystemObject = System.Object;

namespace Dot.Entity
{
    public class EntityObject
    {
        public long UniqueID { get; private set; }
        public long OwerUniqueID { get; set; } = -1;

        public EventDispatcher EntityDispatcher { get; private set; }

        private Dictionary<Type, EntityController> controllerDic = new Dictionary<Type, EntityController>();

        private List<EntityController> updateControllers = new List<EntityController>();
        private List<EntityController> lateUpdateControllers = new List<EntityController>();
        private List<EntityController> fixedUpdateControllers = new List<EntityController>();

        public EntityObject(long id)
        {
            UniqueID = id;
            EntityDispatcher = new EventDispatcher();
        }

        public void DoUpdate(float deltaTime)
        {
            foreach(var controller in updateControllers)
            {
                controller.DoUpdate(deltaTime);
            }
        }

        public void DoLateUpdate(float deltaTime)
        {
            foreach(var controller in lateUpdateControllers)
            {
                controller.DoLateUpdate(deltaTime);
            }
        }

        public void DoFixedUpdate(float deltaTime)
        {
            foreach(var controller in fixedUpdateControllers)
            {
                controller.DoFixedUpdate(deltaTime);
            }
        }

        public void DoDestroy()
        {

        }

        internal void SetControllerUpdateState(EntityController controller)
        {
            int index = updateControllers.IndexOf(controller);
            if(index>=0)
            {
                if(!controller.Enable)
                {
                    updateControllers.RemoveAt(index);
                }else if(!controller.EnableUpdate)
                {
                    updateControllers.RemoveAt(index);
                }
            }else
            {
                if(controller.Enable && controller.EnableUpdate)
                {
                    updateControllers.Add(controller);
                }
            }
        }

        internal void SetControllerLateUpdateState(EntityController controller)
        {
            int index = lateUpdateControllers.IndexOf(controller);
            if (index >= 0)
            {
                if (!controller.Enable)
                {
                    lateUpdateControllers.RemoveAt(index);
                }
                else if (!controller.EnableLateUpdate)
                {
                    lateUpdateControllers.RemoveAt(index);
                }
            }
            else
            {
                if (controller.Enable && controller.EnableLateUpdate)
                {
                    lateUpdateControllers.Add(controller);
                }
            }
        }

        internal void SetControllerFixedUpdateState(EntityController controller)
        {
            int index = fixedUpdateControllers.IndexOf(controller);
            if (index >= 0)
            {
                if (!controller.Enable)
                {
                    fixedUpdateControllers.RemoveAt(index);
                }
                else if (!controller.EnableFixedUpdate)
                {
                    fixedUpdateControllers.RemoveAt(index);
                }
            }
            else
            {
                if (controller.Enable && controller.EnableFixedUpdate)
                {
                    fixedUpdateControllers.Add(controller);
                }
            }
        }

        #region Controller

        public bool HasController<T>() where T:EntityController
        {
            return controllerDic.ContainsKey(typeof(T));
        }

        public bool HasController(Type type)
        {
            Assert.IsTrue(typeof(EntityController).IsAssignableFrom(type), $"EntityObject::HasController->The type is not a subclass of EntityController.type = {type}");
            return controllerDic.ContainsKey(type);
        }

        public T AddController<T>() where T:EntityController
        {
            return (T)AddController(typeof(T));
        }

        public EntityController AddController(Type type)
        {
            Assert.IsTrue(typeof(EntityController).IsAssignableFrom(type), $"EntityObject::AddController->The type is not a subclass of EntityController.type = {type}");

            EntityController controller = Activator.CreateInstance(type) as EntityController;
            AddController(controller);
            return controller;
        }

        public void AddController(EntityController controller)
        {
            Type type = controller.GetType();
            Assert.IsFalse(controllerDic.ContainsKey(type), $"EntityObject->AddController::The controller has been created.type = {type}");
            controller.Initialize(this);
            controllerDic.Add(type, controller);
        }

        public EntityController RemoveController(Type type)
        {
            if (HasController(type))
            {
                EntityController controller = controllerDic[type];
                controllerDic.Remove(type);
                if(updateControllers.Contains(controller))
                {
                    updateControllers.Remove(controller);
                }
                if(lateUpdateControllers.Contains(controller))
                {
                    lateUpdateControllers.Remove(controller);
                }
                if(fixedUpdateControllers.Contains(controller))
                {
                    fixedUpdateControllers.Remove(controller);
                }
                return controller;
            }
            return null;
        }

        public T RemoveController<T>() where T : EntityController
        {
            EntityController controller = RemoveController(typeof(T));
            if(controller!=null)
            {
                return (T)controller;
            }
            return default;
        }

        public void RemoveController(EntityController controller)
        {
            RemoveController(controller.GetType());
        }

        public EntityController[] RemoveAllController()
        {
            EntityController[] result = controllerDic.Values.ToArray();
            
            controllerDic.Clear();
            updateControllers.Clear();
            lateUpdateControllers.Clear();
            fixedUpdateControllers.Clear();

            return result;
        }

        public EntityController GetController(Type type)
        {
            if (controllerDic.TryGetValue(type, out EntityController controller))
            {
                return controller;
            }
            return null;
        }

        public T GetController<T>() where T:EntityController
        {
            EntityController controller = GetController(typeof(T));
            if(controller != null)
            {
                return (T)controller;
            }    
            return default;
        }

        #endregion

        #region event

        protected virtual void RegisterGlobalEvent() { }
        protected virtual void UnregisterGlobalEvent() { }

        internal void DoSendGlobalEvent(int eventID,params SystemObject[] values)
        {
            EventManager.GetInstance().TriggerEvent(eventID, values);
        }

        #endregion

    }
}
