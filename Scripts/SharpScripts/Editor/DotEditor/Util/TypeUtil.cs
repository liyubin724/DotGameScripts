﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotEditor.Util
{
    public static class TypeUtil
    {
        /// <summary>
        /// 扩展方法，用于判断能否从当前类型转换到指定的类型
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static bool IsCastableTo(this Type from, Type to)
        {
            if (to.IsAssignableFrom(from)) return true;
            var methods = from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(
                    m => m.ReturnType == to &&
                    (m.Name == "op_Implicit" ||
                        m.Name == "op_Explicit")
                );
            return methods.Count() > 0;
        }

        public static string NicifyVariableName(this Type type)
        {
            string typeName = type.Name;
            typeName = UnityEditor.ObjectNames.NicifyVariableName(typeName);
            return typeName;
        }

        /// <summary>
        /// 扩展方法，可以显示一个更具可读性的类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string PrettyName(this Type type)
        {
            if (type == null) return "null";
            if (type == typeof(System.Object)) return "object";
            if (type == typeof(float)) return "float";
            else if (type == typeof(int)) return "int";
            else if (type == typeof(long)) return "long";
            else if (type == typeof(double)) return "double";
            else if (type == typeof(string)) return "string";
            else if (type == typeof(bool)) return "bool";
            else if (type.IsGenericType)
            {
                string s = "";
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(List<>)) s = "List";
                else s = type.GetGenericTypeDefinition().ToString();

                Type[] types = type.GetGenericArguments();
                string[] stypes = new string[types.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    stypes[i] = types[i].PrettyName();
                }
                return s + "<" + string.Join(", ", stypes) + ">";
            }
            else if (type.IsArray)
            {
                string rank = "";
                for (int i = 1; i < type.GetArrayRank(); i++)
                {
                    rank += ",";
                }
                Type elementType = type.GetElementType();
                if (!elementType.IsArray) return elementType.PrettyName() + "[" + rank + "]";
                else
                {
                    string s = elementType.PrettyName();
                    int i = s.IndexOf('[');
                    return s.Substring(0, i) + "[" + rank + "]" + s.Substring(i);
                }
            }
            else return type.ToString();
        }


    }
}
