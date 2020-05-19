using Dot.NativeDrawer.Decorator;
using UnityEditor;

namespace DotEditor.NativeDrawer.Decorator
{
    [CustomAttributeDrawer(typeof(SpaceLineAttribute))]
    public class SpaceLineDrawer : DecoratorDrawer
    {
        public SpaceLineDrawer(DecoratorAttribute attr) : base(attr)
        {
        }

        public override void OnGUILayout()
        {
            EditorGUILayout.Space();
        }
    }
}
