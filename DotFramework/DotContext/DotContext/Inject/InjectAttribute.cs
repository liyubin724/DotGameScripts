using System;

namespace Dot.Context
{
    public enum InjectUsage
    {
        InOut = 0,
        In,
        Out,
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class StringInjectField : Attribute
    {
        public InjectUsage Usage { get; set; } = InjectUsage.InOut;
        public bool Optional { get; set; } = false;
        public string InjectName { get; set; } = string.Empty;

        public StringInjectField(string name)
        {
            InjectName = name;
        }

        public StringInjectField(string name,InjectUsage usage)
        {
            InjectName = name;
            Usage = usage;
        }

        public StringInjectField(string name, InjectUsage usage,bool optional)
        {
            InjectName = name;
            Usage = usage;
            Optional = optional;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class TypeInjectField : Attribute
    {
        public InjectUsage Usage { get; set; } = InjectUsage.InOut;
        public bool Optional { get; set; } = false;

        public TypeInjectField()
        { }

        public TypeInjectField(InjectUsage usage)
        {
            Usage = usage;
        }

        public TypeInjectField(InjectUsage usage, bool optional)
        {
            Usage = usage;
            Optional = optional;
        }
    }
}
