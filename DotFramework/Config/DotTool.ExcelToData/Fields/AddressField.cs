using DotTool.ETD.Data;
using DotTool.ETD.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTool.ETD.Fields
{
    public class AddressField : Field
    {
        public AddressField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
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
