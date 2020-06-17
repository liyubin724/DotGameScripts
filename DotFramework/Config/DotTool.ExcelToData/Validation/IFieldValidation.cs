using System;

namespace DotTool.ETD.Validation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FieldValidationTargetTypeAttribute : Attribute
    {
        public Type[] TargetTypes { get; set; }
        public FieldValidationTargetTypeAttribute(Type[] types)
        {
            TargetTypes = types;
        }
    }

    public enum FieldValidationResult
    {
        Success = 0,
        Pass = 1,
    }

    public interface IFieldValidation
    {
        string Rule { get; set; }
        FieldValidationResult Verify();
    }
}
