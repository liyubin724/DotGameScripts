using Game.Core.BMFont;
using System;
using UnityEditor;

namespace GameEditor.Core.BMFont
{
    [CustomEditor(typeof(BMFontBaseText),true)]
    public class BMFontBaseTextEditor : Editor
    {
        SerializedProperty dataProperty;
        SerializedProperty nameProperty;
        SerializedProperty textProperty;
        void Awake()
        {
            dataProperty = serializedObject.FindProperty("m_FontData");
            nameProperty = serializedObject.FindProperty("m_FontName");
            textProperty = serializedObject.FindProperty("m_Text");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(dataProperty);
            string[] names = new string[0];
            if (dataProperty.objectReferenceValue != null)
            {
                names = ((BMFontData)(dataProperty.objectReferenceValue)).fontNames;
            }
            if (names != null && names.Length > 0)
            {
                int index = Array.IndexOf(names, nameProperty.stringValue);
                if (index < 0)
                {
                    index = 0;
                }
                nameProperty.stringValue = names[EditorGUILayout.Popup("Font Name:", index, names)];
            }
            else
            {
                EditorGUILayout.HelpBox("No Font Name in fontData!", MessageType.Warning);
            }
            EditorGUILayout.PropertyField(textProperty);
            serializedObject.ApplyModifiedProperties();
            DoTextChanged();
        }

        protected virtual void DoTextChanged()
        {
        }
    }
}
