using System;

namespace Dot.GUI.Drawer.Verification
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class NotNullAttribute : VerificationAttribute
    {
    }
}
