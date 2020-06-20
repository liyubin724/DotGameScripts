using System;

namespace DotTool.ETD.Validation
{
    public enum FieldValidationResult
    {
        Success = 0,
        Pass = 1,

        ValidationFormatError = -1,
        ArgIsNull = -2,
        ContentIsNull = -3,
        ParseContentFailed = -4,
        NumberRangeError = -5,
        MaxLenError = -6,
        ContentRepeatError = -7,
        LuaFunctionError = -8,
    }

    public interface IFieldValidation
    {
        string Rule { get; set; }
        FieldValidationResult Verify();
    }
}
