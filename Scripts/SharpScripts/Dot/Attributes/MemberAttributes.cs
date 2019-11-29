using System;

namespace Dot.Attributes
{
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property|AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MemberDesc : Attribute
    {
        public string Desc { get; set; }

        public MemberDesc(string desc)
        {
            Desc = desc;
        }
    }
}
