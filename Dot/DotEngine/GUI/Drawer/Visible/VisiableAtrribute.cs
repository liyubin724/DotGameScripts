using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.GUI.Drawer.Visible
{
    public abstract class VisiableAtrribute : DrawerAttribute
    {
    }

    public abstract class VisiableCompareAttribute : VisiableAtrribute
    {
        public string ComparedName { get; private set; }
        public object ComparedValue { get; private set; }

        protected VisiableCompareAttribute(string comparedName, object comparedValue)
        {
            ComparedName = comparedName;
            ComparedValue = comparedValue;
        }
    }
}
