using DotEngine.Context;
using DotTool.ETD.Data;
using DotTool.ETD.Log;
using System;

namespace DotTool.ETD.Validation
{
    [FieldValidationTargetType(new Type[] { typeof(bool) })]
    public class BoolValidation : IFieldValidation
    {
        [ContextField(typeof(LogHandler))]
        private LogHandler logHandler;
        [ContextField(typeof(Field))]
        private Field field;
        [ContextField(typeof(Cell))]
        private Cell cell;

        public string Rule { get; set; }

        public FieldValidationResult Verify()
        {
            if (field == null || cell == null)
            {
                logHandler.Log(LogType.Error, LogMessage.LOG_ARG_IS_NULL);

                return FieldValidationResult.ArgIsNull;
            }

            string content = cell.GetValue(field);
            if (!bool.TryParse(content, out bool value))
            {
                logHandler.Log(LogType.Error, LogMessage.LOG_VALIDATION_CONVERT_ERROR, "bool", cell.ToString());
                return FieldValidationResult.ParseContentFailed;
            }
            return FieldValidationResult.Success;
        }
    }
}
