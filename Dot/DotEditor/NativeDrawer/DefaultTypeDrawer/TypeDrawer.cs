using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    public abstract class TypeDrawer
    {
        public object Target { get; private set; }
        public FieldInfo Field { get; private set; }

        protected TypeDrawer(object target,FieldInfo field)
        {
            Target = target;
            Field = field;
        }

        public abstract void OnLayoutGUI(string label);
    }
}
