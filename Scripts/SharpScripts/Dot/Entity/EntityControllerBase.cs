using Dot.Dispatch;
using Dot.Log;
using Dot.Lua;
using System;
using XLua;
using EventHandler = Dot.Dispatch.EventHandler;

namespace Dot.Entity
{
    [Flags]
    public enum EntityControllerType
    {
        VirtualView = 1<<0,
        Avatar = 1<<1,
        Move = 1<<2,
        Animator = 1<<3,
    }

    public abstract class EntityControllerBase
    {
        internal static readonly string LOGGER_NAME = "EntityController";
        private static readonly string LUA_CONTROLLER_NAME = "controller";

        public bool Enable { get; set; } = true;

        #region
        private EntityObject entityObj = null;
        protected EntityObject Entity { get => entityObj; }
        public void InitController(EntityObject entity)
        {
            entityObj = entity;

            DoInit();
            RegisterEvents();
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

        #endregion

        #region Operation for Lua
        private LuaTable objTable = null;
        protected LuaTable ObjTable
        {
            get => objTable;
        }

        public void BindLuaScript(string luaScript)
        {
            if(string.IsNullOrEmpty(luaScript))
            {
                LogUtil.LogError(LOGGER_NAME, "EntityControllerBase::BindLuaScript->the name of script is empty");
                return;
            }
            int index = luaScript.LastIndexOf("/");
            string scriptFileName = luaScript;
            if(index>0)
            {
                scriptFileName = luaScript.Substring(index + 1);
            }
            if(string.IsNullOrEmpty(scriptFileName))
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
                if(classTable == null)
                {
                    LogUtil.LogError(LOGGER_NAME, $"EntityControllerBase::BindLuaScript->Require lua Script failed.luaScript = {luaScript}");
                    return;
                }
            }

            LuaFunction callFun = classTable.Get<LuaFunction>("__call");
            objTable = callFun.Func<LuaTable, LuaTable>(classTable);

            objTable.Set(LUA_CONTROLLER_NAME, this);
            OnLuaBinded();

            callFun.Dispose();
            classTable.Dispose();
        }

        protected virtual void OnLuaBinded()
        {
        }

        protected void CallAction(string funcName)
        {
            if(objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallAction->objTable is null");
                return;
            }

            objTable.Get<Action<LuaTable>>(funcName)?.Invoke(objTable);
        }

        protected void CallActoin<T>(string funcName,T param1)
        {
            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallAction->objTable is null");
                return;
            }

            objTable.Get<Action<LuaTable,T>>(funcName)?.Invoke(objTable,param1);
        }

        protected void CallAction<T,K>(string funcName,T param1,K param2)
        {
            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallAction->objTable is null");
                return;
            }

            objTable.Get<Action<LuaTable, T,K>>(funcName)?.Invoke(objTable, param1,param2);
        }

        protected R CallFunc<R>(string funcName)
        {
            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallFunc->objTable is null");
                return default(R);
            }

            Func<LuaTable, R> result = objTable.Get<Func<LuaTable, R>>(funcName);
            if(result !=null)
            {
                return result(objTable);
            }
            return default(R);
        }

        protected R CallFunc<T,R>(string funcName,T param1)
        {
            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallFunc->objTable is null");
                return default(R);
            }

            Func<LuaTable, T,R> result = objTable.Get<Func<LuaTable, T,R>>(funcName);
            if (result != null)
            {
                return result(objTable,param1);
            }
            return default(R);
        }

        protected R CallFunc<T, K,R>(string funcName, T param1, K param2)
        {
            if (objTable == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityContrllerBase::CallFunc->objTable is null");
                return default(R);
            }

            Func<LuaTable, T, K,R> result = objTable.Get<Func<LuaTable, T,K, R>>(funcName);
            if (result != null)
            {
                return result(objTable, param1,param2);
            }
            return default(R);
        }
        #endregion

        #region Operation for event
        private EventDispatcher eventDispatcher = null;
        protected void SendEvent(int eventID, params object[] values) => eventDispatcher.TriggerEvent(eventID, 0, values);
        protected void RegisterEvent(int eventID, EventHandler handler) => eventDispatcher.RegisterEvent(eventID, handler);
        protected void UnregisterEvent(int eventID, EventHandler handler) => eventDispatcher.UnregisterEvent(eventID, handler);

        protected virtual void RegisterEvents() { }
        protected virtual void UnregisterEvents() { }
        #endregion
    }
}
