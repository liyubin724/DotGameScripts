using System;

namespace Dot.Env
{
    public enum ContextFieldUsage
    {
        InOut =0,
        In,
        Out,
    }

    public class ContextField : Attribute
    {
        public ContextFieldUsage Usage { get; set; } = ContextFieldUsage.InOut;
        public bool Optional { get; set; } = false;

        public ContextField()
        { }

        public ContextField(ContextFieldUsage usage)
        {
            Usage = usage;
        }

        public ContextField(ContextFieldUsage usage,bool optional)
        {
            Usage = usage;
            Optional = optional;
        }
    }
}
