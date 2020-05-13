using System.Reflection;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    public abstract class NativeTypeDrawer
    {
        public object Target { get; private set; }
        public FieldInfo Field { get; private set; }

        protected NativeTypeDrawer(object target,FieldInfo field)
        {
            Target = target;
            Field = field;
        }

        public abstract void OnLayoutGUI(string label);
    }
}
