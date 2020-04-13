using System;

namespace Dot.GUI.Attributes.Verify
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class NotNullAttribute : VerifyAttribute
    {
    }
}
