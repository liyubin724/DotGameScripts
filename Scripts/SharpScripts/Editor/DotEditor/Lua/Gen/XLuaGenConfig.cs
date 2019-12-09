using CSObjectWrapEditor;
using DotEditor.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace DotEditor.Lua.Gen
{
    public static class XLuaGenConfig
    {
        [GenPath]
        public static string GetGenPath
        {
            get
            {
                return "Assets/Scripts/SharpScripts/XLuaGen";
            }
        }

        [LuaCallCSharp]
        public static List<Type> GetLuaCallCSharpTypeList
        {
            get
            {
                List<Type> callTypes = new List<Type>();
                GenConfig genConfig = GenConfigUtil.LoadGenConfig(false);
                if (genConfig != null)
                {
                    foreach(var typeFullName in genConfig.callCSharpTypeNames)
                    {
                        Type t = AssemblyUtil.GetTypeByFullName(typeFullName);
                        if (t != null)
                        {
                            callTypes.Add(t);
                        }
                        else
                        {
                            Debug.LogError("Type Not Found");
                        }
                    }

                    foreach(var generic in genConfig.callCSharpGenericTypeNames)
                    {
                        Type t = GetGenericType(generic);
                        if(t!=null)
                        {
                            callTypes.Add(t);
                        }else
                        {
                            Debug.LogError("Type Not Found");
                        }
                    }

                }
                return callTypes;
            }
        }

        private static Type GetGenericType(string genericTypeName)
        {
            if(string.IsNullOrEmpty(genericTypeName))
            {
                return null;
            }
            string[] types = genericTypeName.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (types == null || types.Length < 2)
            {
                return null;
            }
            string genericType = types[0];
            string[] paramTypes = new string[types.Length - 1];
            Array.Copy(types, 1, paramTypes, 0, paramTypes.Length);

            return AssemblyUtil.GetGenericType(genericType, paramTypes);
        }

        [CSharpCallLua]
        public static List<Type> GetCSharpCallLuaTypeList
        {
            get
            {
                List<Type> callTypes = new List<Type>();
                GenConfig genConfig = GenConfigUtil.LoadGenConfig(false);
                if (genConfig != null)
                {
                    foreach (var typeFullName in genConfig.callLuaTypeNames)
                    {
                        Type t = AssemblyUtil.GetTypeByFullName(typeFullName);
                        if (t != null)
                        {
                            callTypes.Add(t);
                        }
                        else
                        {
                            Debug.LogError("Type Not Found");
                        }
                    }

                    foreach (var generic in genConfig.callLuaGenericTypeNames)
                    {
                        Type t = GetGenericType(generic);
                        if (t != null)
                        {
                            callTypes.Add(t);
                        }
                        else
                        {
                            Debug.LogError("Type Not Found");
                        }
                    }
                }
                return callTypes;
            }
        }

        [GCOptimize]
        public static List<Type> GetGCOptimizeTypeList
        {
            get
            {
                List<Type> callTypes = new List<Type>();
                GenConfig genConfig = GenConfigUtil.LoadGenConfig(false);
                if (genConfig != null)
                {
                    foreach (var typeFullName in genConfig.optimizeTypeNames)
                    {
                        callTypes.Add(AssemblyUtil.GetTypeByFullName(typeFullName));
                    }
                }
                return callTypes;
            }
        }

        [BlackList]

        public static List<List<string>> GetBlackList
        {
            get
            {
                List<List<string>> result = new List<List<string>>();
                GenConfig genConfig = GenConfigUtil.LoadGenConfig(false);
                if(genConfig != null)
                {
                    foreach (var blackStr in genConfig.blackDatas)
                    {
                        List<string> list = new List<string>();
                        list.AddRange(blackStr.Split(new char[] { '@', '$' }, StringSplitOptions.RemoveEmptyEntries));
                        result.Add(list);
                    }
                }

                return result;
            }
        }
    }
}
