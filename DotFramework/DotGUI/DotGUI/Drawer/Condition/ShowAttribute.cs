using System;

namespace Dot.GUI.Drawer.Condition
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowAttribute : ConditionDrawerAttribute
    {
    }
}
