using DotTool.ETD.Data;
using DotTool.ETD.Validation;
using System.Collections.Generic;

namespace DotTool.ETD.Fields
{
    public class StringField : Field
    {
        public StringField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        public override string GetDefaultValue()
        {
            return defaultValue;
        }

        protected override void AppendDefaultValidation(List<IFieldValidation> validations)
        {
            
        }
    }
}
