using DotTool.ETD.Data;

namespace DotTool.ETD.Fields
{
    public class LongField : Field
    {
        public LongField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        public override string GetDefaultValue()
        {
            return string.IsNullOrEmpty(defaultValue) ? "0L" : defaultValue;
        }

        protected override string GetDefaultValidation()
        {
            return "long";
        }
    }
}
