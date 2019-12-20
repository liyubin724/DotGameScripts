﻿using Dot.Log;
using System;
using XLua;

namespace Dot.Lua.Register
{
    [Serializable]
    public class RegisterBehaviourData
    {
        public BehaviourData[] behaviourDatas = new BehaviourData[0];
        public BehaviourArrayData[] behaviourArrayDatas = new BehaviourArrayData[0];

        public void RegisterToLua(LuaEnv luaEnv, LuaTable objTable)
        {
            if (luaEnv != null && objTable != null)
            {
                RegisterLuaBehaviour(objTable);
                RegisterLuaBehaviourArr(luaEnv, objTable);
            }
        }

        void RegisterLuaBehaviour(LuaTable objTable)
        {
            if (behaviourDatas != null && behaviourDatas.Length > 0)
            {
                for (int i = 0; i < behaviourDatas.Length; i++)
                {
                    if (behaviourDatas[i].behaviour == null)
                    {
                        LogUtil.LogError(typeof(RegisterBehaviourData), "LuaBehaviour::RegisterLuaBehaviour->behaviour is null.objName = " + behaviourDatas[i].name + "  index = " + i);
                        continue;
                    }
                    behaviourDatas[i].behaviour.InitLua();

                    objTable.Set<string, LuaTable>(behaviourDatas[i].name, behaviourDatas[i].behaviour.ObjTable);
                }
            }
        }

        void RegisterLuaBehaviourArr(LuaEnv luaEnv, LuaTable objTable)
        {
            if (behaviourArrayDatas != null && behaviourArrayDatas.Length > 0)
            {
                for (int i = 0; i < behaviourArrayDatas.Length; i++)
                {
                    if (string.IsNullOrEmpty(behaviourArrayDatas[i].name))
                    {
                        LogUtil.LogError(typeof(RegisterBehaviourData), "LuaBehaviour::RegisterLuaBehaviourArr->Group Name is Null, index = " + i);
                        continue;
                    }

                    LuaTable behTable = luaEnv.NewTable();

                    LuaScriptBindBehaviour[] behs = behaviourArrayDatas[i].behaviours;
                    if (behs != null && behs.Length > 0)
                    {
                        for (int j = 0; j < behs.Length; j++)
                        {
                            if (behs[j] == null)
                            {
                                LogUtil.LogError(typeof(RegisterBehaviourData), "LuaBehaviour::RegisterLuaBehaviourArr->behaviour is Null, index = " + j);
                                continue;
                            }
                            behs[j].InitLua();
                            behTable.Set<int, LuaTable>(j + 1, behs[j].ObjTable);
                        }
                    }

                    objTable.Set<string, LuaTable>(behaviourArrayDatas[i].name, behTable);
                    behTable.Dispose();
                    behTable = null;
                }
            }
        }
    }

    [Serializable]
    public class BehaviourData
    {
        public string name;
        public LuaScriptBindBehaviour behaviour;
    }

    [Serializable]
    public class BehaviourArrayData
    {
        public string name;
        public LuaScriptBindBehaviour[] behaviours = new LuaScriptBindBehaviour[0];
    }
}
