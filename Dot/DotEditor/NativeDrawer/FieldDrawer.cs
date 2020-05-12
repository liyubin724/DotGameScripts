using Dot.NativeDrawer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.NativeDrawer
{
    public class FieldDrawer
    {
        public Type TargetType { get; private set; }
        public FieldInfo Field { get; private set; }

        public FieldDrawer(FieldInfo field)
        {
            TargetType = field.FieldType;
            Field = field;

            IEnumerable<NativeDrawerAttribute> attrs = Field.GetCustomAttributes<NativeDrawerAttribute>();
            foreach(var attr in attrs)
            {

            }
        }

        public void DoLayoutGUI(object target)
        {

        }
    }
}
