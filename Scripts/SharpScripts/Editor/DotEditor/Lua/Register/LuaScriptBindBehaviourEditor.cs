using Dot.Lua.Register;
using UnityEditor;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(LuaScriptBindBehaviour))]
    public class LuaScriptBindBehaviourEditor : Editor
    {
        SerializedProperty envType = null;
        SerializedProperty luaAsset = null;

        protected virtual void OnEnable()
        {
            envType = serializedObject.FindProperty("envType");
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
            EditorGUILayout.PropertyField(envType);
            EditorGUILayout.PropertyField(luaAsset);
        }
    }
}
