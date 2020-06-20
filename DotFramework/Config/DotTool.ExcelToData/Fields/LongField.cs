using DotTool.ETD.Data;

namespace DotTool.ETD.Fields
{
    public class LongField : Field
    {
        public LongField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        protected override string GetDefaultValidation()
        {
            return "long";
        }
    }
}
