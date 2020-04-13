using System;

namespace Dot.GUI.Attributes.Condition
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowAttribute : ConditionAttribute
    {
    }
}
