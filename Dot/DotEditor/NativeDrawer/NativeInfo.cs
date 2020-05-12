using System.Reflection;

namespace DotEditor.NativeDrawer
{
    public abstract class NativeInfo
    {
        public object Target { get; private set; }
        public FieldInfo Field { get; private set; }

        protected NativeInfo(object target,FieldInfo field)
        {
            Target = target;
            Field = field;
        }

        public abstract void OnLayoutGUI();
    }
}
