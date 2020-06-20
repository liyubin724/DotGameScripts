using DotTool.ETD.Data;

namespace DotTool.ETD.Fields
{
    public class RefField : Field
    {
        public RefField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        public override string GetDefaultValue()
        {
            return string.IsNullOrEmpty(defaultValue) ? "-1" : defaultValue;
        }

        protected override string GetDefaultValidation()
        {
            return "int";
        }
    }
}
