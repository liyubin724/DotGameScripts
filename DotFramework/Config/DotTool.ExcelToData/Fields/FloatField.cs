using DotTool.ETD.Data;

namespace DotTool.ETD.Fields
{
    public class FloatField : Field
    {
        public FloatField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        public override string GetDefaultValue()
        {
            return string.IsNullOrEmpty(defaultValue) ? "0.0f" : defaultValue;
        }

        protected override string GetDefaultValidation()
        {
            return "float";
        }
    }
}
