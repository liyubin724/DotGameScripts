using Dot.Lua.Register;
using UnityEditor;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(LuaScriptBindBehaviour), true)]
    public class LuaScriptBindBehaviourEditor : Editor
    {
        SerializedProperty luaAsset = null;

        protected virtual void OnEnable()
        {
            luaAsset = serializedObject.FindProperty("luaAsset");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptInfo();

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawScriptInfo()
        {
            EditorGUILayout.PropertyField(luaAsset);
        }
    }
}
