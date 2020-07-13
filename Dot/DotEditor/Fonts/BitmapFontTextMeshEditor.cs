using DotEngine.Fonts;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Fonts
{
    [CustomEditor(typeof(BitmapFontTextMesh))]
    public class BitmapFontTextMeshEditor : BitmapFontTextEditor
    {
        SerializedProperty textMeshProperty;

        protected override void OnEnable()
        {
            textMeshProperty = serializedObject.FindProperty("textMesh");
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                if (textMeshProperty.objectReferenceValue == null)
                {
                    textMeshProperty.objectReferenceValue = (target as BitmapFontTextMesh).GetComponent<TextMesh>();
                }
                EditorGUILayout.PropertyField(textMeshProperty);
            }
            serializedObject.ApplyModifiedProperties();
            
            if(textMeshProperty.objectReferenceValue !=null)
            {
                base.OnInspectorGUI();
            }
        }
    }
}
