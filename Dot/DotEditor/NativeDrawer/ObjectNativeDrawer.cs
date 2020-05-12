using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.NativeDrawer
{
    public class ObjectNativeDrawer
    {
        public object Target { get; private set; }
        public bool IsDeclaredOnly { get; private set; }
        public Type PreventType { get; private set; }

        public ObjectNativeDrawer(object target,bool isDeclaredOnly = false,Type preventType = null)
        {
            Target = target;
            IsDeclaredOnly = isDeclaredOnly;
            PreventType = preventType;
        }

        public void DoLayoutGUI()
        {

        }
    }
}
