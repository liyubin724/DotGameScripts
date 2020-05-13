using DotEditor.NativeDrawer.DefaultTypeDrawer;
using System;
using System.Reflection;

namespace DotEditor.NativeDrawer
{
    public abstract class NativeInfo
    {
        public object Target { get; private set; }
        public FieldInfo Field { get; private set; }
        public object Value
        {
            get
            {
                if (Field == null)
                {
                    return Target;
                } else
                {
                    return Field.GetValue(Target);
                }
            }
            set
            {
                if(Field == null)
                {
                    Target = value;
                }else
                {
                    Field.SetValue(Target, value);
                }
            }
        }
        public Type ValueType
        {
            get
            {
                if(Field == null)
                {
                    return Target.GetType();
                }else
                {
                    return Field.FieldType;
                }
            }
        }

        protected NativeTypeDrawer defaultTypeDrawer = null;

        protected NativeInfo(object target,FieldInfo field)
        {
            Target = target;
            Field = field;

            if(field!=null)
            {
                defaultTypeDrawer = NativeDrawerUtility.CreateDefaultTypeDrawer(target, field);
            }
        }

        public abstract void OnLayoutGUI();
    }
}
