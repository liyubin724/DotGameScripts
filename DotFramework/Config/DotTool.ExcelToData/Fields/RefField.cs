using DotTool.ETD.Data;
using DotTool.ETD.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTool.ETD.Fields
{
    public class RefField : Field
    {
        public RefField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        public override string GetDefaultValue()
        {
            if (string.IsNullOrEmpty(defaultValue))
            {
                return "-1";
            }
            return defaultValue;
        }

        protected override void AppendDefaultValidation(List<IFieldValidation> validations)
        {
            validations.Add(new IntValidation() { Rule = "Int" });
        }
    }
}
