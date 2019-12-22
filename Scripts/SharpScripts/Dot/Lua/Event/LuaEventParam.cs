using System;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Lua.Event
{
    public enum LuaEventParamType
    {
        SystemValue = 0,
        UnityValue,
    }

    public enum LuaEventSystemParamType
    {
        Int = 0,
        Float,
        Bool,
        String,
    }

    [Serializable]
    public class LuaEventParam
    {
        public LuaEventParamType paramType = LuaEventParamType.SystemValue;

        public LuaEventSystemParamType systemParamType = LuaEventSystemParamType.Int;
        public int intValue = 0;
        public float floatValue = 0.0f;
        public bool boolValue = false;
        public string stringValue = string.Empty;

        public UnityObject unityObject = null;
        public string unityTypeName = string.Empty;

        public SystemObject GetValue()
        {
            if(paramType == LuaEventParamType.UnityValue)
            {
                return unityObject;
            }else
            {
                if(systemParamType == LuaEventSystemParamType.Int)
                {
                    return intValue;
                }else if(systemParamType == LuaEventSystemParamType.Float)
                {
                    return floatValue;
                }else if(systemParamType == LuaEventSystemParamType.Bool)
                {
                    return boolValue;
                }else if(systemParamType == LuaEventSystemParamType.String)
                {
                    return stringValue;
                }
                return null;
            }
        }
    }
}
