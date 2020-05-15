using Dot.NativeDrawer;
using Dot.Utilities;
using System;
using System.Reflection;

namespace DotEditor.NativeDrawer
{
    public abstract class AttrNativeDrawer
    {
        public NativeDrawerAttribute Attr { get; private set; }

        public T GetAttr<T>() where T:NativeDrawerAttribute
        {
            return (T)Attr;
        }

        protected AttrNativeDrawer(NativeDrawerAttribute attr)
        {
            Attr = attr;
        }
    }

    public abstract class CompareAttrNativeDrawer : AttrNativeDrawer
    {
        public object Target { get; private set; }
        protected CompareAttrNativeDrawer(object target,CompareDrawerAttribute attr) : base(attr)
        {
            Target = target;
        }

        public bool IsValid()
        {
            CompareDrawerAttribute attr = GetAttr<CompareDrawerAttribute>();
            if(string.IsNullOrEmpty(attr.MemberName))
            {
                return true;
            }

            FieldInfo fieldInfo = Target.GetType().GetField(attr.MemberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            object comparedValue;
            if (fieldInfo!=null)
            {
                comparedValue = fieldInfo.GetValue(Target);
            }else
            {
                PropertyInfo propertyInfo = Target.GetType().GetProperty(attr.MemberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if(propertyInfo!=null)
                {
                    comparedValue = propertyInfo.GetValue(Target);
                }else
                {
                    MethodInfo methodInfo = Target.GetType().GetMethod(attr.MemberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if(methodInfo!=null)
                    {
                        comparedValue = methodInfo.Invoke(Target,null);
                    }else
                    {
                        return false;
                    }
                }
            }

            if(comparedValue==null && attr.Value == null)
            {
                return true;
            }else if(comparedValue !=null && attr.Value == null)
            {
                return false;
            }else if(comparedValue==null && attr.Value !=null)
            {
                return false;
            }

            if(comparedValue.GetType()!= attr.Value.GetType())
            {
                return false;
            }

            if (TypeUtility.IsCastableTo(comparedValue.GetType(), typeof(IComparable)))
            {
                int compared = ((IComparable)comparedValue).CompareTo((IComparable)attr.Value);
                if (compared == 0 && (attr.Symbol == CompareSymbol.Gte || attr.Symbol == CompareSymbol.Lte || attr.Symbol == CompareSymbol.Eq))
                {
                    return true;
                }
                else if (compared > 0 && (attr.Symbol == CompareSymbol.Lt || attr.Symbol == CompareSymbol.Lte))
                {
                    return true;
                }
                else if (compared < 0 && (attr.Symbol == CompareSymbol.Gt || attr.Symbol == CompareSymbol.Gte))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return comparedValue == attr.Value;
            }
        }

    }
}
