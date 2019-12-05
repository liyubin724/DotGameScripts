using Dot.Core.Logger;
using System;
using UnityEngine;
using XLua;

namespace Dot.Lua.Register
{
    public partial class LuaRegisterBehaviour : MonoBehaviour
    {
        public LuaEnvType envType = LuaEnvType.Game;

        public LuaAsset luaAsset;

        public RegisterObjectData[] regLuaObject = new RegisterObjectData[0];
        public RegisterObjectArrayData[] regLuaObjectArr = new RegisterObjectArrayData[0];
        public RegisterBehaviourData[] regLuaBehaviour = new RegisterBehaviourData[0];
        public RegisterBehaviourArrayData[] regLuaBehaviourArr = new RegisterBehaviourArrayData[0];

        private LuaEnv luaEnv;
        private LuaTable objTable = null;

        public LuaTable ObjTable { get => objTable; }

        private bool isInited = false;
        public void InitLua()
        {
            if (isInited)
                return;

            isInited = true;

            luaEnv = LuaManager.GetInstance()[envType];
            if(luaEnv == null)
            {
                DebugLogger.LogError($"LuaRegisterBehaviour::InitLua->LuaEnv is null. envType = {envType}");
                return;
            }
            
            if (luaAsset != null && !luaAsset.IsValid())
            {
                objTable = luaAsset.DoRequireAndInstance(luaEnv);
                if(objTable!=null)
                {
                    objTable.Set("gameObject", gameObject);
                    objTable.Set("transform", transform);

                    RegisterLuaObject();
                    RegisterLuaObjectArr();

                    RegisterLuaBehaviour();
                    RegisterLuaBehaviourArr();
                }else
                {
                    DebugLogger.LogError($"LuaRegisterBehaviour::InitLua->objTable is null.");
                    return;
                }
            }
        }

        private void Awake()
        {
            InitLua();
            objTable.Get<Action<LuaTable>>(LuaConfig.AWAKE_FUNCTION_NAME)?.Invoke(objTable);
        }

        void RegisterLuaBehaviour()
        {
            if (regLuaBehaviour != null && regLuaBehaviour.Length > 0)
            {
                for (int i = 0; i < regLuaBehaviour.Length; i++)
                {
                    if (regLuaBehaviour[i].behaviour == null)
                    {
                        DebugLogger.LogError("LuaBehaviour::RegisterLuaBehaviour->behaviour is null.objName = " + name + "  index = " + i);
                        continue;
                    }
                    regLuaBehaviour[i].behaviour.InitLua();

                    objTable.Set<string, LuaTable>(regLuaBehaviour[i].name, regLuaBehaviour[i].behaviour.ObjTable);
                }
            }
        }

        void RegisterLuaBehaviourArr()
        {
            if (regLuaBehaviourArr != null && regLuaBehaviourArr.Length > 0)
            {
                for (int i = 0; i < regLuaBehaviourArr.Length; i++)
                {
                    if (string.IsNullOrEmpty(regLuaBehaviourArr[i].name))
                    {
                        DebugLogger.LogError("LuaBehaviour::RegisterLuaBehaviourArr->Group Name is Null, index = " + i);
                        continue;
                    }

                    LuaTable behTable = luaEnv.NewTable();

                    LuaRegisterBehaviour[] behs = regLuaBehaviourArr[i].behaviours;
                    if (behs != null && behs.Length > 0)
                    {
                        for (int j = 0; j < behs.Length; j++)
                        {
                            if (behs[j] == null)
                            {
                                DebugLogger.LogError("LuaBehaviour::RegisterLuaBehaviourArr->behaviour is Null, index = " + j);
                                continue;
                            }
                            behs[j].InitLua();
                            behTable.Set<int, LuaTable>(j + 1, behs[j].ObjTable);
                        }
                    }

                    objTable.Set<string, LuaTable>(regLuaBehaviourArr[i].name, behTable);
                    behTable.Dispose();
                    behTable = null;
                }
            }
        }

        void RegisterLuaObject()
        {
            for (int i = 0; i < regLuaObject.Length; i++)
            {
                if (regLuaObject[i].obj == null || regLuaObject[i].regObj == null)
                {
                    DebugLogger.LogError("LuaBehaviour::RegisterLuaObjects->obj or regObj is Null");
                    continue;
                }
                string regName = regLuaObject[i].name;
                if (string.IsNullOrEmpty(regName))
                {
                    regName = regLuaObject[i].regObj.name;
                }

                objTable.Set(regName, regLuaObject[i].regObj);
            }
        }

        void RegisterLuaObjectArr()
        {
            if (regLuaObjectArr != null && regLuaObjectArr.Length > 0)
            {
                for (int i = 0; i < regLuaObjectArr.Length; i++)
                {
                    if (string.IsNullOrEmpty(regLuaObjectArr[i].name))
                    {
                        DebugLogger.LogError("LuaBehaviour::RegisterLuaObjectArr->Group Name is Null, index = " + i);
                        continue;
                    }

                    LuaTable regTable = luaEnv.NewTable();

                    RegisterObjectData[] luaObjs = regLuaObjectArr[i].objects;
                    if (luaObjs != null && luaObjs.Length > 0)
                    {
                        for (int j = 0; j < luaObjs.Length; j++)
                        {
                            if (luaObjs[j].regObj == null)
                            {
                                DebugLogger.LogError("LuaBehaviour::RegisterLuaObjectArr->obj or regObj is Null");
                                continue;
                            }

                            regTable.Set(j + 1, luaObjs[j].regObj);
                        }
                    }
                    objTable.Set<string, LuaTable>(regLuaObjectArr[i].name, regTable);
                    regTable.Dispose();
                    regTable = null;
                }
            }
        }

        void Start()
        {
            if (objTable != null)
            {
                objTable.Get<Action<LuaTable>>(LuaConfig.START_FUNCTION_NAME)?.Invoke(objTable);
            }
        }

        void OnEnable()
        {
            if (objTable != null)
            {
                objTable.Get<Action<LuaTable>>(LuaConfig.ENABLE_FUNCTION_NAME)?.Invoke(objTable);
            }
        }

        void OnDisable()
        {
            if (objTable != null)
            {
                objTable.Get<Action<LuaTable>>(LuaConfig.DISABLE_FUNCTION_NAME)?.Invoke(objTable);
            }
        }

        void OnDestroy()
        {
            if (luaEnv == null || objTable == null)
            {
                return;
            }
            objTable.Get<Action<LuaTable>>(LuaConfig.DESTROY_FUNCTION_NAME)?.Invoke(objTable);

            objTable.Dispose();
            objTable = null;
            luaEnv = null;
        }
    }
}
