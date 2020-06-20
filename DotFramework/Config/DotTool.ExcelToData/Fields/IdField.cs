using DotTool.ETD.Data;
using System;

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

        protected override string GetDefaultValidation()
        {
            return "int;unique";
        }
    }
}
