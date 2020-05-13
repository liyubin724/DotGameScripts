using Dot.NativeDrawer;
using Dot.NativeDrawer.Decorator;
using Dot.NativeDrawer.Layout;
using Dot.NativeDrawer.Property;
using Dot.NativeDrawer.Verification;
using Dot.NativeDrawer.Visible;
using DotEditor.NativeDrawer.Decorator;
using DotEditor.NativeDrawer.DefaultTypeDrawer;
using DotEditor.NativeDrawer.Layout;
using DotEditor.NativeDrawer.Property;
using DotEditor.NativeDrawer.Verification;
using DotEditor.NativeDrawer.Visible;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotEditor.NativeDrawer
{
    public static class NativeDrawerUtility
    {
        private static Dictionary<Type, Type> attrDrawerDic = new Dictionary<Type, Type>();

        private static Dictionary<Type, Type> defaultTypeDrawerDic = new Dictionary<Type, Type>();

        [UnityEditor.InitializeOnLoadMethod]
        public static void OnDrawerInited()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                Type[] types = (
                                from type in assembly.GetTypes() 
                                where !type.IsAbstract && !type.IsInterface && typeof(AttrNativeDrawer).IsAssignableFrom(type) 
                                select type
                                ).ToArray();
                foreach(var type in types)
                {
                    CustomAttDrawerLinkAttribute attr = type.GetCustomAttribute<CustomAttDrawerLinkAttribute>();
                    if(attr!=null)
                    {
                        attrDrawerDic.Add(attr.AttrType, type);
                    }
                }

                types = (
                                from type in assembly.GetTypes()
                                where !type.IsAbstract && !type.IsInterface && typeof(NativeTypeDrawer).IsAssignableFrom(type)
                                select type
                                ).ToArray();
                foreach(var type in types)
                {
                    CustomTypeDrawerAttribute attr = type.GetCustomAttribute<CustomTypeDrawerAttribute>();
                    if(attr!=null)
                    {
                        defaultTypeDrawerDic.Add(attr.Target, type);
                    }
                }
            }
        }

        public static NativeTypeDrawer CreateDefaultTypeDrawer(object target,FieldInfo field)
        {
            Type fieldType = field.FieldType;
            if(fieldType.IsEnum)
            {
                fieldType = typeof(Enum);
            }

            if(defaultTypeDrawerDic.TryGetValue(fieldType, out Type drawerType))
            {
                return (NativeTypeDrawer)Activator.CreateInstance(drawerType, new object[] { target, field });
            }
            return null;
        }

        public static DecoratorDrawer CreateDecoratorDrawer(DecoratorAttribute attr)
        {
            if(attrDrawerDic.TryGetValue(attr.GetType(),out Type drawerType))
            {
                return (DecoratorDrawer)Activator.CreateInstance(drawerType, new object[] { attr });
            }
            return null;
        }

        public static LayoutDrawer CreateLayoutDrawer(LayoutAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (LayoutDrawer)Activator.CreateInstance(drawerType, new object[] { attr });
            }
            return null;
        }

        public static VerificationDrawer CreateVerificationDrawer(object target, VerificationCompareAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (VerificationDrawer)Activator.CreateInstance(drawerType, new object[] { target,attr });
            }
            return null;
        }

        public static VisibleDrawer CreateVisibleDrawer(VisibleAtrribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (VisibleDrawer)Activator.CreateInstance(drawerType, new object[] {attr });
            }
            return null;
        }

        public static VisibleCompareDrawer CreateVisibleCompareDrawer(object target, VisibleCompareAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (VisibleCompareDrawer)Activator.CreateInstance(drawerType, new object[] { target,attr });
            }
            return null;
        }

        public static PropertyDrawer CreatePropertyDrawer(object target, FieldInfo field, NativeDrawerAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (PropertyDrawer)Activator.CreateInstance(drawerType, new object[] { target, field,attr });
            }
            return null;
        }

        public static PropertyLabelDrawer CreatePropertyLabelDrawer(PropertyLabelAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (PropertyLabelDrawer)Activator.CreateInstance(drawerType, new object[] { attr });
            }
            return null;
        }

        public static PropertyControlDrawer CreatePropertyControlDrawer(PropertyControlAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (PropertyControlDrawer)Activator.CreateInstance(drawerType, new object[] { attr });
            }
            return null;
        }
    }
}
