using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTool.ETD.Validation
{
    [FieldValidationTargetType(new Type[] { typeof(bool) })]
    public class BoolValidation : IFieldValidation
    {
        public string Rule { get; set; }

        public FieldValidationResult Verify()
        {
            throw new NotImplementedException();
        }
    }
}
