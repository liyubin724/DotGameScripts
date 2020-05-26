using UnityEditor;

namespace DotEditor.NativeDrawer
{
    public class NativeDrawerEditor : Editor
    {
        private NativeDrawerObject drawerObject = null;

        void OnEnable()
        {
            drawerObject = new NativeDrawerObject(target)
            {
                IsShowScroll = true,
            };
        }

        public override void OnInspectorGUI()
        {
            drawerObject.OnGUILayout();
        }
    }
}
