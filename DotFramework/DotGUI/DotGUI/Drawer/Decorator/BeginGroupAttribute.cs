using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.GUI.Drawer.Decorator
{

    public class BeginGroupAttribute : DecoratorDrawerAttribute
    {
        public string Label { get; private set; }
        public BeginGroupAttribute(string label = null)
        {
            Label = label;
        }
    }
}
