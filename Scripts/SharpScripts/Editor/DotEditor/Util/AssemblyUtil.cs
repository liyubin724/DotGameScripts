using Dot.Log;
using System;
using System.Reflection;

namespace DotEditor.Util
{
    public static class AssemblyUtil
    {
        public static Type GetTypeByFullName(string typeFullName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach(var type in types)
                {
                    if(type.FullName == typeFullName)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        public static Type GetGenericType(string genericTypeFullName,params string[] paramTypeFullNames)
        {
            if(string.IsNullOrEmpty(genericTypeFullName) || paramTypeFullNames == null || paramTypeFullNames.Length ==0)
            {
                LogUtil.LogError(typeof(AssemblyUtil), "AssemblyUtil::GetGenericType->Arg is Null");
                return null;
            }

            Type genericType = GetTypeByFullName(genericTypeFullName);
            if(genericType == null)
            {
                LogUtil.LogError(typeof(AssemblyUtil), $"AssemblyUtil::GetGenericType->Type Not Found.Type = {genericTypeFullName}");
                return null;
            }

            Type[] types = new Type[paramTypeFullNames.Length];
            for(int i =0;i<paramTypeFullNames.Length;i++)
            {
                string typeStr = paramTypeFullNames[i];
                if (string.IsNullOrEmpty(typeStr))
                {
                    LogUtil.LogError(typeof(AssemblyUtil), "AssemblyUtil::GetGenericType->Param Type Is NUll");
                    return null;
                }
                Type t = GetTypeByFullName(paramTypeFullNames[i]);
                if(t == null)
                {
                    LogUtil.LogError(typeof(AssemblyUtil), $"AssemblyUtil::GetGenericType->Param Type Not Found.Type = {paramTypeFullNames[i]}");
                }
                types[i] = t;
            }

            Type result = genericType.MakeGenericType(types);
            return result;
        }
    }
}
