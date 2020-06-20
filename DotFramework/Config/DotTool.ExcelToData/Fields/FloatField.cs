using DotTool.ETD.Data;

namespace DotTool.ETD.Fields
{
    public class FloatField : Field
    {
        public FloatField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
            if(string.IsNullOrEmpty(defaultValue))
            {
                defaultValue = "0.0f";
            }
        }

        protected override string GetDefaultValidation()
        {
            return "float";
        }
    }
}
