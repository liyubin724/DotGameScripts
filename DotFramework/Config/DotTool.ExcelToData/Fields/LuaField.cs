using DotTool.ETD.Data;

namespace DotTool.ETD.Fields
{
    public class LuaField : Field
    {
        public LuaField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        public override string GetDefaultValue()
        {
            return defaultValue;
        }
    }
}
