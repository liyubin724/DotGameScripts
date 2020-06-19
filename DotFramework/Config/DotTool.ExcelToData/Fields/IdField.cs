using DotTool.ETD.Data;
using DotTool.ETD.Validation;
using System;
using System.Collections.Generic;

namespace DotTool.ETD.Fields
{
    public class IdField : Field
    {
        public IdField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        public override string GetDefaultValue()
        {
            throw new Exception();
        }

        protected override void AppendDefaultValidation(List<IFieldValidation> validations)
        {
            validations.Add(new IntValidation() { Rule = "Int" });
            validations.Add(new UniqueValidation() { Rule = "Unique" });
        }
    }
}
