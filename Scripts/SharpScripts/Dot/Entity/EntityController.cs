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
        private static readonly string INIT_NAME = "DoInit";
        private static readonly string DESTROY_NAME = "DoDestroy";
        protected static readonly string UPDATE_NAME = "DoUpdate";

        private static readonly string CONTROLLER_REGISTER_NAME = "csController";
        private static readonly string ENTITY_REGISTER_NAME = "entityObject";

        public bool Enable { get; set; } = true;

        protected EntityObject entityObj = null;
        protected LuaTable objTable = null;

        public void InitController(EntityObject entity, string luaScript)
        {
            entityObj = entity;

            LuaEnv luaEnv = LuaManager.GetInstance().Env;
            objTable = LuaRequire.Instance(luaEnv, luaScript);

            if (objTable == null)
            {
                LogUtil.LogError(GetType(), "EntityControllerBase::InitController->Lua Script has not binded!");
                return;
            }

            objTable.Set(CONTROLLER_REGISTER_NAME, this);
            objTable.Set(ENTITY_REGISTER_NAME, entity.ObjTable);

            DoInit();
            RegisterEvents();

            objTable.Get<Action<LuaTable>>(INIT_NAME)?.Invoke(objTable);
        }

        public void ResetController()
        {
            UnregisterEvents();

            DoReset();

            entityObj = null;
            if(objTable!=null)
            {
                objTable.Dispose();
                objTable = null;
            }
        }

        public void DestroyController()
        {
            ResetController();
            DoDestroy();
        }

        protected abstract void DoInit();
        protected abstract void DoReset();
        protected abstract void DoDestroy();

        protected virtual void RegisterEvents() { }
        protected virtual void UnregisterEvents() { }
        protected internal virtual void DoUpdate(float deltaTime) { }
    }
}
