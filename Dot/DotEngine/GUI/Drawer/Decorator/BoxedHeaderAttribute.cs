using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.GUI.Drawer.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class BoxedHeaderAttribute : DecoratorAttribute
    {
        public string Header { get; private set; }

        public BoxedHeaderAttribute(string header)
        {
            Header = header;
        }
    }
}
