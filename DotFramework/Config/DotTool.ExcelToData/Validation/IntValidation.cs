using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTool.ETD.Validation
{
    [FieldValidationTargetTypeAttribute(new Type[] { typeof(int) })]
    public class IntValidation : IFieldValidation
    {
        public string Rule { get; set; }

        public FieldValidationResult Verify()
        {
            throw new NotImplementedException();
        }
    }
}
