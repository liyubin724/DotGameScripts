using DotTool.ETD.Data;

namespace DotTool.ETD.Fields
{
    public class RefField : Field
    {
        public RefField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
            if(string.IsNullOrEmpty(defaultValue))
            {
                defaultValue = "-1";
            }
        }

        protected override string GetDefaultValidation()
        {
            return "int";
        }
    }
}
