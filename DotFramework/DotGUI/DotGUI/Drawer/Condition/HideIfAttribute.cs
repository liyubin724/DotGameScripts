using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.GUI.Drawer.Condition
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class HideIfAttribute : CompareAttribute
    {
        public HideIfAttribute(string comparedName, object comparedValue) : base(comparedName, comparedValue)
        {
        }
    }
}
