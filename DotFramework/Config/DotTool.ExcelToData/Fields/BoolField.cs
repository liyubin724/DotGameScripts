using DotTool.ETD.Data;
using DotTool.ETD.Validation;
using System.Collections.Generic;

namespace DotTool.ETD.Fields
{
    public class BoolField : Field
    {
        public BoolField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        public override string GetDefaultValue()
        {
            if(string.IsNullOrEmpty(defaultValue))
            {
                return "false";
            }
            return defaultValue;
        }

        protected override void AppendDefaultValidation(List<IFieldValidation> validations)
        {
            validations.Add(new BoolValidation() { Rule = "bool" });
        }
    }
}
