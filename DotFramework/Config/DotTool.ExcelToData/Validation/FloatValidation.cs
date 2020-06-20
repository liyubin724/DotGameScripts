using DotEngine.Context;
using DotTool.ETD.Data;
using DotTool.ETD.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTool.ETD.Validation
{
    [FieldValidationTargetTypeAttribute(new Type[] { typeof(float) })]
    public class FloatValidation : IFieldValidation
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
            if (string.IsNullOrEmpty(content))
            {
                logHandler.Log(LogType.Warning, LogMessage.LOG_VALIDATION_SET_DEFAULT, "0.0", cell.Row, cell.Col);
                //content = cell.alue = "0.0";
            }

            if (!float.TryParse(content, out float value))
            {
                logHandler.Log(LogType.Error, LogMessage.LOG_VALIDATION_CONVERT_ERROR, "float", cell.ToString());
                return FieldValidationResult.ParseContentFailed;
            }

            return FieldValidationResult.Success;
        }
    }
}
