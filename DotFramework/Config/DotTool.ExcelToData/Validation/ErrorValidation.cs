using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTool.ETD.Validation
{
    public class ErrorValidation : IFieldValidation
    {
        public string Rule { get; set; }

        public FieldValidationResult Verify()
        {
            return FieldValidationResult.Success;
        }
    }
}
