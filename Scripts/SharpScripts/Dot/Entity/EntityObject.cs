using Dot.Dispatch;
using Dot.Log;
using Dot.Lua;
using System;
using System.Collections.Generic;
using XLua;
using EventHandler = Dot.Dispatch.EventHandler;

namespace Dot.Entity
{
    public class EntityObject
    {
        private static readonly string DO_INIT_NAME = "DoInit";
        private static readonly string DO_DESTROY_NAME = "DoDestroy";

        public long UniqueID { get; private set; }
        public int Category { get; private set; }
        public string Name { get; private set; }

        private LuaTable objTable = null;
        public LuaTable ObjTable { get => objTable; }

        private LuaTable controllerTable = null;

        public EntityObject()
        {
            eventDispatcher = new EventDispatcher();
        }

        public LuaTable InitEntity(long id,int category,string name,string script)
        {
            UniqueID = id;
            Category = category;
            Name = name;

            LuaEnv luaEnv = LuaManager.GetInstance().LuaEnv;
            objTable = LuaRequire.Instance(luaEnv, script);

            objTable.Get<Action<LuaTable>>(DO_INIT_NAME)?.Invoke(objTable);
            return objTable;
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

        public void DoDestroy()
        {

        }

        #region operation for event
        private EventDispatcher eventDispatcher = null;
        internal EventDispatcher Dispatcher { get => eventDispatcher; }
        public void SendEvent(int eventID, params object[] values) => eventDispatcher.TriggerEvent(eventID, 0, values);
        public void RegisterEvent(int eventID, EventHandler handler) => eventDispatcher.RegisterEvent(eventID, handler);
        public void UnregisterEvent(int eventID, EventHandler handler) => eventDispatcher.UnregisterEvent(eventID, handler);
        #endregion

        #region operation for controller
        private Dictionary<EntityControllerType, EntityController> controllerDic = new Dictionary<EntityControllerType, EntityController>();
        public T GetController<T>(EntityControllerType controllerType) where T:EntityController
        {
            if(controllerDic.TryGetValue(controllerType,out EntityController controller))
            {
                return (T)controller;
            }
            return null;
        }

        public bool HasController(EntityControllerType controllerType)
        {
            return controllerDic.ContainsKey(controllerType);
        }

        public void AddController(EntityControllerType controllerType, EntityController controller)
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

        public void ReplaceController(EntityControllerType controllerType, EntityController controller)
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
                //kvp.Value.ResetController();
            }
            controllerDic.Clear();
        }
        #endregion
    }
}
