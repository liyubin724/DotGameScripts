using DotEngine.Context;
using DotTool.ETD.Data;
using DotTool.ETD.Log;
using System;

namespace DotTool.ETD.Validation
{
    public class BoolValidation : IFieldValidation
    {
#pragma warning disable CS0649
        [ContextField(typeof(LogHandler))]
        private LogHandler logHandler;
        [ContextField(typeof(Field))]
        private Field field;
        [ContextField(typeof(Cell))]
        private Cell cell;
#pragma warning restore CS0649

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
