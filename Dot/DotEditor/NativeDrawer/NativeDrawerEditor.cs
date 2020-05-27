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
                IsShowScroll = IsShowScroll(),
            };
        }

        protected virtual bool IsShowScroll()
        {
            return true;
        }

        public override void OnInspectorGUI()
        {
            drawerObject.OnGUILayout();
        }
    }
}
