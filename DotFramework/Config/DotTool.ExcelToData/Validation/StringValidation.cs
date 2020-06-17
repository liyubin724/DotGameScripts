using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTool.ETD.Validation
{
    [FieldValidationTargetType(new Type[] { typeof(string) })]
    public class StringValidation : IFieldValidation
    {
        public string Rule { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public FieldValidationResult Verify()
        {
            throw new NotImplementedException();
        }
    }
}
