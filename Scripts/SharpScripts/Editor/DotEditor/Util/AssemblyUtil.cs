using Dot.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                DebugLogger.LogError("AssemblyUtil::GetGenericType->Arg is Null");
                return null;
            }

            Type genericType = GetTypeByFullName(genericTypeFullName);
            if(genericType == null)
            {
                DebugLogger.LogError($"AssemblyUtil::GetGenericType->Type Not Found.Type = {genericTypeFullName}");
                return null;
            }

            Type[] types = new Type[paramTypeFullNames.Length];
            for(int i =0;i<paramTypeFullNames.Length;i++)
            {
                string typeStr = paramTypeFullNames[i];
                if (string.IsNullOrEmpty(typeStr))
                {
                    DebugLogger.LogError("AssemblyUtil::GetGenericType->Param Type Is NUll");
                    return null;
                }
                Type t = GetTypeByFullName(paramTypeFullNames[i]);
                if(t == null)
                {
                    DebugLogger.LogError($"AssemblyUtil::GetGenericType->Param Type Not Found.Type = {paramTypeFullNames[i]}");
                }
                types[i] = t;
            }

            Type result = genericType.MakeGenericType(types);
            return result;
        }
    }
}
