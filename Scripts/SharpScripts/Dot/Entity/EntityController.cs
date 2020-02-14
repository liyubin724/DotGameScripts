using Dot.Log;
using Dot.Lua;
using System;
using XLua;

namespace Dot.Entity
{
    public enum EntityControllerType
    {
        None = 0,
        View,
        Avatar,
        Move,
        Animator,
    }

    public abstract class EntityController
    {
        private static readonly string INIT_COMPLETE_NAME = "DoInit";

        public bool Enable { get; set; } = true;

        protected EntityObject entityObj = null;
        protected LuaTable objTable = null;

        public void InitController(EntityObject entity, string luaScript)
        {
            entityObj = entity;

            LuaEnv luaEnv = LuaManager.GetInstance().LuaEnv;
            objTable = LuaRequire.Instance(luaEnv, luaScript);

            if (objTable == null)
            {
                LogUtil.LogError(EntityConst.CONTROLLER_LOGGER_NAME, "EntityControllerBase::InitController->Lua Script has not binded!");
                return;
            }

            objTable.Set(EntityConst.CONTROLLER_REGISTER_NAME, this);
            objTable.Set(EntityConst.ENTITYOBJECT_REGISTER_NAME, entity.ObjTable);

            DoInit();
            RegisterEvents();

            objTable.Get<Action<LuaTable>>(EntityConst.DO_INIT_NAME)?.Invoke(objTable);
        }

        public void DestroyController()
        {

        }

        protected abstract void DoInit();

        protected internal virtual void DoUpdate(float deltaTime) { }

        protected abstract void DoDestroy();

        protected virtual void RegisterEvents() { }
        protected virtual void UnregisterEvents() { }

        public abstract EntityControllerType ControllerType { get; }
        public abstract string RegisterName { get; }
    }
}
