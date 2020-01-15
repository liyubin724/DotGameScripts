using Dot.UI;
using UnityEditor;

namespace DotEditor.UI
{
    [CustomEditor(typeof(UIRoot))]
    public class UIRootEditor : Editor
    {
        SerializedProperty uiCamera = null;
        SerializedProperty uiCanvas = null;
        SerializedProperty uiMgr = null;

        private void OnEnable()
        {
            uiCamera = serializedObject.FindProperty("uiCamera");
            uiCanvas = serializedObject.FindProperty("uiCanvas");
            uiMgr = serializedObject.FindProperty("uiMgr");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(uiCamera);
            EditorGUILayout.PropertyField(uiCanvas);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(uiMgr);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
