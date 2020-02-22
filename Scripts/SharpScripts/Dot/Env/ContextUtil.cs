using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dot.Env
{
    public static class ContextUtil
    {
        private static Dictionary<Type, Dictionary<ContextFieldUsage, List<FieldInfo>>> cachedTypeFieldDic = new Dictionary<Type, Dictionary<ContextFieldUsage, List<FieldInfo>>>();

        private static void CacheFields(Type type)
        {
            if (cachedTypeFieldDic.ContainsKey(type))
            {
                return;
            }

            List<FieldInfo> inFields = new List<FieldInfo>();
            List<FieldInfo> outFields = new List<FieldInfo>();
            Dictionary<ContextFieldUsage, List<FieldInfo>> fieldDic = new Dictionary<ContextFieldUsage, List<FieldInfo>>();
            fieldDic.Add(ContextFieldUsage.In, inFields);
            fieldDic.Add(ContextFieldUsage.Out, outFields);
            cachedTypeFieldDic.Add(type, fieldDic);

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach(var field in fields)
            {
                var attrs = field.GetCustomAttributes(typeof(ContextField), true);
                if (attrs == null || attrs.Length == 0)
                {
                    continue;
                }
                if (!(attrs[0] is ContextField attr))
                    continue;

                if(attr.Usage != ContextFieldUsage.In)
                {
                    outFields.Add(field);
                }
                if(attr.Usage != ContextFieldUsage.Out)
                {
                    inFields.Add(field);
                }
            }
        }

        public static List<FieldInfo> GetFields(Type type,ContextFieldUsage usage)
        {
            if(!cachedTypeFieldDic.ContainsKey(type))
            {
                CacheFields(type);
            }

            if(cachedTypeFieldDic.TryGetValue(type,out Dictionary<ContextFieldUsage,List<FieldInfo>> usageFieldDic))
            {
                if(usageFieldDic.TryGetValue(usage,out List<FieldInfo> fields))
                {
                    return fields;
                }
            }
            return null;
        }

        public static void Extract(IContext context, object obj)
        {
            if (obj == null || context == null)
            {
                throw new ArgumentNullException("ContextUtil::Extract->argument is null");
            }
            List<FieldInfo> fields = GetFields(obj.GetType(), ContextFieldUsage.Out);
            if(fields!=null && fields.Count>0)
            {
                foreach(var field in fields)
                {
                    Type fieldType = field.FieldType;
                    if (typeof(IContext).IsAssignableFrom(fieldType))
                    {
                        throw new InvalidOperationException("Context can only be used with the ContextFieldUsage.In option.");
                    }

                    var attr = field.GetCustomAttributes(typeof(ContextField), true)[0] as ContextField;
                    IContextObject fieldValue = field.GetValue(obj) as IContextObject;
                    if (!attr.Optional)
                    {
                        context.Add(fieldType, fieldValue);
                    }
                    else if (fieldValue != null)
                    {
                        context.Add(fieldType, fieldValue);
                    }
                }
            }
        }

        public static void Inject(IContext context,object obj)
        {
            if (obj == null || context == null)
            {
                throw new ArgumentNullException("ContextUtil::Inject->argument is null.");
            }

            List<FieldInfo> fields = GetFields(obj.GetType(), ContextFieldUsage.In);
            if (fields != null && fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    var attr = field.GetCustomAttributes(typeof(ContextField), true)[0] as ContextField;

                    object fieldValue = null;
                    Type fieldType = field.FieldType;
                    if (typeof(IContext).IsAssignableFrom(fieldType))
                    {
                        fieldValue = context;
                    }
                    else
                    {
                        if (context.TryGet(fieldType, out IContextObject cachedValue))
                        {
                            fieldValue = cachedValue;
                        }

                        if (fieldValue == null && !attr.Optional)
                        {
                            throw new Exception($"ContextUtil::Inject->Data not found.type = {fieldType.FullName}");
                        }
                    }

                    field.SetValue(obj, fieldValue);
                }
            }
        }
    }
}
