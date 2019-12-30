using Dot.FieldDrawer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DotEditor.EGUI.FieldDrawer
{
    public static class FieldDrawerUtil
    {
        private static readonly Dictionary<Type, Type> drawerTypeDic = new Dictionary<Type, Type>();
        private static bool isInit = false;
        private static void InitFieldDrawer()
        {
            if (isInit) return;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(AFieldDrawer)))
                    {
                        TargetFieldType targetFieldType = type.GetCustomAttribute<TargetFieldType>();
                        if (targetFieldType != null)
                        {
                            drawerTypeDic.Add(targetFieldType.TargetType, type);
                        }
                    }
                }
            }
            isInit = true;
        }

        private static Type GetDrawerType(Type fieldType)
        {
            InitFieldDrawer();

            if (!drawerTypeDic.TryGetValue(fieldType, out Type drawerType))
            {
                if (fieldType.IsValueType && fieldType.IsEnum)
                {
                    drawerType = drawerTypeDic[typeof(Enum)];
                }else if(fieldType.IsClass && !fieldType.IsArray && !typeof(IList).IsAssignableFrom(fieldType))
                {
                    drawerType = drawerTypeDic[typeof(System.Object)];
                }
            }

            return drawerType;
        }

        private static FieldInfo[] GetFieldInfos(Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            List<FieldInfo> list = new List<FieldInfo>();

            foreach (var fieldInfo in fieldInfos)
            {
                if (fieldInfo.IsPublic)
                {
                    FieldHide fieldHide = fieldInfo.GetCustomAttribute<FieldHide>();
                    if (fieldHide == null)
                    {
                        list.Add(fieldInfo);
                    }
                }
                else
                {
                    FieldShow fieldShow = fieldInfo.GetCustomAttribute<FieldShow>();
                    if (fieldShow != null)
                    {
                        list.Add(fieldInfo);
                    }
                }
            }

            list.Sort((item1, item2) =>
            {
                var item1Attr = item1.GetCustomAttribute<FieldOrder>();
                var item2Attr = item2.GetCustomAttribute<FieldOrder>();
                int order1 = item1Attr != null ? item1Attr.Order : 9999;
                int order2 = item2Attr != null ? item2Attr.Order : 9999;
                return order1.CompareTo(order2);
            });
            return list.ToArray();
        }

        public static FieldData[] GetTypeFieldDrawer(Type type)
        {
            List<FieldData> fieldDatas = new List<FieldData>();
            foreach (var fieldInfo in GetFieldInfos(type))
            {
                FieldData fieldData = new FieldData()
                {
                    name = fieldInfo.Name
                };

                Type drawerType = GetDrawerType(fieldInfo.FieldType);
                if (drawerType!=null)
                {
                    fieldData.drawer = (AFieldDrawer)Activator.CreateInstance(drawerType, fieldInfo);
                }

                fieldDatas.Add(fieldData);
            }
            return fieldDatas.ToArray();
        }
    }
}
