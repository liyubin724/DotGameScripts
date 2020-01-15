using Dot.UI;
using UnityEditor;

namespace DotEditor.UI
{
    [CustomEditor(typeof(UIRoot))]
    public class UIRootEditor : Editor
    {
        SerializedProperty uiCamera = null;
        SerializedProperty uiCanvas = null;

        private void OnEnable()
        {
            uiCamera = serializedObject.FindProperty("uiCamera");
            uiCanvas = serializedObject.FindProperty("uiCanvas");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(uiCamera);
            EditorGUILayout.PropertyField(uiCanvas);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
