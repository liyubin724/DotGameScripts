using DotEngine.Context;
using DotTool.ETD.Data;
using DotTool.ETD.Log;

namespace DotTool.ETD.Validation
{
    public class UniqueValidation : IFieldValidation
    {
#pragma warning disable CS0649
        [ContextField(typeof(LogHandler))]
        private LogHandler logHandler;
        [ContextField(typeof(Field))]
        private Field field;
        [ContextField(typeof(Cell))]
        private Cell cell;
        [ContextField(typeof(Sheet))]
        private Sheet sheet;
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
            for (int i = 0; i < sheet.LineCount; ++i)
            {
                Line line = sheet.GetLineByIndex(i);
                if (line.Row != cell.Row)
                {
                    Cell tempCell = line.GetCellByCol(field.Col);
                    string tempContent = tempCell.GetValue(field);
                    if (tempContent == content)
                    {
                        logHandler.Log(LogType.Error, LogMessage.LOG_VALIDATION_CONTENT_REPEAT_ERROR, cell.ToString(), tempCell.ToString());
                        return FieldValidationResult.ContentRepeatError;
                    }
                }
            }

            return FieldValidationResult.Success;
        }
    }
}
