using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotEngine.Context.Inject
{
    public static class InjectUtil
    {
        private static Dictionary<Type, Dictionary<InjectUsage, List<FieldInfo>>> cachedTypeFieldDic = new Dictionary<Type, Dictionary<InjectUsage, List<FieldInfo>>>();

        private static void CacheFields(Type type)
        {
            if (cachedTypeFieldDic.ContainsKey(type))
            {
                return;
            }

            List<FieldInfo> inFields = new List<FieldInfo>();
            List<FieldInfo> outFields = new List<FieldInfo>();

            Dictionary<InjectUsage, List<FieldInfo>> fieldDic = new Dictionary<InjectUsage, List<FieldInfo>>();
            fieldDic.Add(InjectUsage.In, inFields);
            fieldDic.Add(InjectUsage.Out, outFields);

            cachedTypeFieldDic.Add(type, fieldDic);

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var field in fields)
            {
                var attrs = field.GetCustomAttributes(typeof(TypeInjectField), true);
                if (attrs == null || attrs.Length == 0)
                {
                    continue;
                }
                if (!(attrs[0] is TypeInjectField attr))
                    continue;

                if (attr.Usage != InjectUsage.In)
                {
                    outFields.Add(field);
                }
                if (attr.Usage != InjectUsage.Out)
                {
                    inFields.Add(field);
                }
            }
        }

        public static List<FieldInfo> GetFields(Type type, InjectUsage usage)
        {
            if (!cachedTypeFieldDic.ContainsKey(type))
            {
                CacheFields(type);
            }

            if (cachedTypeFieldDic.TryGetValue(type, out Dictionary<InjectUsage, List<FieldInfo>> usageFieldDic))
            {
                if (usageFieldDic.TryGetValue(usage, out List<FieldInfo> fields))
                {
                    return fields;
                }
            }
            return null;
        }

        public static void Extract(StringContext context, object extractObj)
        {
            if (extractObj == null || context == null)
            {
                throw new ArgumentNullException("ContextUtil::Extract->argument is null");
            }

            List<FieldInfo> fields = GetFields(extractObj.GetType(), InjectUsage.Out);
            if (fields != null && fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    if(field.FieldType == typeof(StringContext))
                    {
                        throw new InvalidOperationException("Context can only be used with the InjectUsage.In option.");
                    }

                    var attr = field.GetCustomAttributes(typeof(StringInjectField), true)[0] as StringInjectField;
                    object fieldValue = field.GetValue(extractObj);
                    if (!attr.Optional)
                    {
                        if (context.ContainsKey(attr.InjectName))
                        {
                            context.Update(attr.InjectName, fieldValue);
                        }
                        else
                        {
                            context.Add(attr.InjectName, fieldValue, false);
                        }
                    }
                    else if (fieldValue != null)
                    {
                        if (context.ContainsKey(attr.InjectName))
                        {
                            context.Update(attr.InjectName, fieldValue);
                        }
                        else
                        {
                            context.Add(attr.InjectName, fieldValue, false);
                        }
                    }
                }
            }
        }

        public static void Inject(StringContext context, object injectObj)
        {
            if (injectObj == null || context == null)
            {
                throw new ArgumentNullException("ContextUtil::Inject->argument is null.");
            }

            List<FieldInfo> fields = GetFields(injectObj.GetType(), InjectUsage.In);
            if (fields != null && fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    var attr = field.GetCustomAttributes(typeof(StringInjectField), true)[0] as StringInjectField;

                    if (!context.TryGet(attr.InjectName, out object fieldValue))
                    {
                        if (fieldValue == null && !attr.Optional)
                        {
                            throw new Exception($"ContextUtil::Inject->Data not found.fieldName = {field.Name},key = {attr.InjectName}");
                        }
                    }
                    field.SetValue(injectObj, fieldValue);
                }
            }
        }

        public static void Extract(TypeContext context, object obj)
        {
            if (obj == null || context == null)
            {
                throw new ArgumentNullException("ContextUtil::Extract->argument is null");
            }
            List<FieldInfo> fields = GetFields(obj.GetType(), InjectUsage.Out);
            if (fields != null && fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    Type fieldType = field.FieldType;
                    if (typeof(TypeContext) == fieldType)
                    {
                        throw new InvalidOperationException("Context can only be used with the InjectUsage.In option.");
                    }

                    var attr = field.GetCustomAttributes(typeof(TypeInjectField), true)[0] as TypeInjectField;
                    object fieldValue = field.GetValue(obj);
                    if (!attr.Optional)
                    {
                        if(context.ContainsKey(fieldType))
                        {
                            context.Update(fieldType, fieldValue);
                        }else
                        {
                            context.Add(fieldType, fieldValue, false);
                        }
                    }
                    else if (fieldValue != null)
                    {
                        if (context.ContainsKey(fieldType))
                        {
                            context.Update(fieldType, fieldValue);
                        }
                        else
                        {
                            context.Add(fieldType, fieldValue, false);
                        }
                    }
                }
            }
        }

        public static void Inject(TypeContext context, object obj)
        {
            if (obj == null || context == null)
            {
                throw new ArgumentNullException("ContextUtil::Inject->argument is null.");
            }

            List<FieldInfo> fields = GetFields(obj.GetType(), InjectUsage.In);
            if (fields != null && fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    var attr = field.GetCustomAttributes(typeof(TypeInjectField), true)[0] as TypeInjectField;

                    Type fieldType = field.FieldType;
                    if (!context.TryGet(fieldType, out object fieldValue))
                    {
                        if (!attr.Optional)
                        {
                            throw new Exception($"ContextUtil::Inject->Data not found.fieldName = {field.Name},type = {fieldType.FullName}");
                        }
                    }
                    field.SetValue(obj, fieldValue);
                }
            }
        }
    }
}
