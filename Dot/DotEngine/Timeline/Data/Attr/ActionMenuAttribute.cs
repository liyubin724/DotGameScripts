using System;

namespace DotEngine.Timeline.Data
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class ActionMenuAttribute : Attribute
    {
        public string Name { get; set; }
        public string Prefix { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;

        public ActionMenuAttribute(string name)
        {
            Name = name;
        }
    }
}
