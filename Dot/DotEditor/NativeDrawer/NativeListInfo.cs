using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.NativeDrawer
{
    public class NativeListInfo : NativeInfo
    {
        public NativeListInfo(object target) : this(target, null)
        {
        }

        public NativeListInfo(object target, FieldInfo field) : base(target, field)
        {
        }
    }
}
