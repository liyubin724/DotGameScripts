using Dot.Dispatch;
using Dot.Log;
using Dot.Lua;
using System;
using XLua;
using EventHandler = Dot.Dispatch.EventHandler;

namespace Dot.Entity
{
    public enum EntityControllerType
    {
        VirtualView = 0,
        Avatar,
        Move,
        Animator,
    }

    public abstract class EntityController
    {
        protected internal static readonly string LOGGER_NAME = "EntityController";
        private static readonly string CONTROLLER_REGISTER_NAME = "controller";

        private static readonly string LUA_INIT_NAME = "OnInit";
        private static readonly string LUA_DESTROY_NAME = "OnDestroy";
        private static readonly string LUA_RESET_NAME = "OnReset";
        private static readonly string LUA_UPDATE_NAME = "OnUpdate";

        public bool Enable { get; set; } = true;

        protected EntityObject entityObj = null;
        protected LuaTable objTable = null;

        private EventDispatcher eventDispatcher = null;

        public void InitController(EntityObject entity, string luaScript)
        {
            entityObj = entity;
            eventDispatcher = entity.Dispatcher;

            BindLuaScript(luaScript);

            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityControllerBase::InitController->Lua Script has not binded!");
                return;
            }

            DoInit();
            RegisterEvents();

            CallAction(LUA_INIT_NAME);
        }

        public void ResetController()
        {
            UnregisterEvents();
            DoReset();

            eventDispatcher = null;
            entityObj = null;
        }

        protected abstract void DoInit();

        protected internal virtual void DoUpdate(float deltaTime) { }

        protected abstract void DoReset();

        public void CallAction(string funcName)
        {
            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallAction->objTable is null");
                return;
            }

            objTable.Get<Action<LuaTable>>(funcName)?.Invoke(objTable);
        }

        public void CallActoin<T>(string funcName, T param1)
        {
            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallAction->objTable is null");
                return;
            }

            objTable.Get<Action<LuaTable, T>>(funcName)?.Invoke(objTable, param1);
        }

        public void CallAction<T, K>(string funcName, T param1, K param2)
        {
            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallAction->objTable is null");
                return;
            }

            objTable.Get<Action<LuaTable, T, K>>(funcName)?.Invoke(objTable, param1, param2);
        }

        public R CallFunc<R>(string funcName)
        {
            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallFunc->objTable is null");
                return default(R);
            }

            Func<LuaTable, R> result = objTable.Get<Func<LuaTable, R>>(funcName);
            if (result != null)
            {
                return result(objTable);
            }
            return default(R);
        }

        public R CallFunc<T, R>(string funcName, T param1)
        {
            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallFunc->objTable is null");
                return default(R);
            }

            Func<LuaTable, T, R> result = objTable.Get<Func<LuaTable, T, R>>(funcName);
            if (result != null)
            {
                return result(objTable, param1);
            }
            return default(R);
        }

        public R CallFunc<T, K, R>(string funcName, T param1, K param2)
        {
            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallFunc->objTable is null");
                return default(R);
            }

            Func<LuaTable, T, K, R> result = objTable.Get<Func<LuaTable, T, K, R>>(funcName);
            if (result != null)
            {
                return result(objTable, param1, param2);
            }
            return default(R);
        }

        protected virtual void RegisterEvents() { }
        protected virtual void UnregisterEvents() { }

        private void BindLuaScript(string luaScript)
        {
            if (string.IsNullOrEmpty(luaScript))
            {
                LogUtil.LogError(LOGGER_NAME, "EntityControllerBase::BindLuaScript->the name of script is empty");
                return;
            }

            string scriptFileName = luaScript;
            int index = luaScript.LastIndexOf("/");
            if (index > 0)
            {
                scriptFileName = luaScript.Substring(index + 1);
            }
            if (string.IsNullOrEmpty(scriptFileName))
            {
                LogUtil.LogError(LOGGER_NAME, "EntityControllerBase::BindLuaScript->the name of script is empty");
                return;
            }

            LuaEnv luaEnv = LuaManager.GetInstance().LuaEnv;
            LuaTable classTable = luaEnv.Global.Get<LuaTable>(scriptFileName);
            if (classTable == null)
            {
                luaEnv.DoString(string.Format("require (\"{0}\")", luaScript));
                classTable = luaEnv.Global.Get<LuaTable>(scriptFileName);
                if (classTable == null)
                {
                    LogUtil.LogError(LOGGER_NAME, $"EntityControllerBase::BindLuaScript->Require lua Script failed.luaScript = {luaScript}");
                    return;
                }
            }

            LuaFunction callFun = classTable.Get<LuaFunction>("__call");
            objTable = callFun.Func<LuaTable, LuaTable>(classTable);

            objTable.Set(CONTROLLER_REGISTER_NAME, this);

            callFun.Dispose();
            classTable.Dispose();
        }

        protected void SendEvent(int eventID, params object[] values)
        {
            eventDispatcher.TriggerEvent(eventID, 0, values);
        }

        protected void RegisterEvent(int eventID, EventHandler handler)
        {
            eventDispatcher.RegisterEvent(eventID, handler);
        }

        protected void UnregisterEvent(int eventID, EventHandler handler)
        {
            eventDispatcher.UnregisterEvent(eventID, handler);
        }
    }
}
