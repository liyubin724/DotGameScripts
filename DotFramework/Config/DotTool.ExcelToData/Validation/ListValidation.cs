using DotEngine.Context;
using DotTool.ETD.Data;
using DotTool.ETD.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTool.ETD.Validation
{
    public class ListValidation : IFieldValidation
    {
#pragma warning disable CS0649,CS0169
        [ContextField(typeof(LogHandler))]
        private LogHandler logHandler;
        [ContextField(typeof(Field))]
        private Field field;
        [ContextField(typeof(Cell))]
        private Cell cell;
#pragma warning restore CS0649,CS0169

        public string Rule { get; set; }

        public FieldValidationResult Verify()
        {
            return FieldValidationResult.Success;
        }
    }
}
