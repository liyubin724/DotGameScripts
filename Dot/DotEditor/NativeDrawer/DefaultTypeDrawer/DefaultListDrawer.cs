using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(IList))]
    public class DefaultListDrawer : NativeTypeDrawer
    {
        public DefaultListDrawer(object target, FieldInfo field) : base(target, field)
        {
        }

        protected override bool IsValid()
        {
            throw new NotImplementedException();
        }

        protected override void OnDraw(string label)
        {
            throw new NotImplementedException();
        }
    }
}
