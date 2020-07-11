using DotEditor.Utilities;
using DotEngine.BMFont;
using System;
using System.IO;
using UnityEditor;

namespace DotEditor.BMFont
{
    public class BMFontTextEditor : Editor
    {
        SerializedProperty fontDataProperty;
        SerializedProperty fontNameProperty;
        SerializedProperty textProperty;

        private BMFontData[] fontDatas = null;
        private string[] fontDataNames = null;
        protected virtual void Awake()
        {
            fontDataProperty = serializedObject.FindProperty("m_FontData");
            fontNameProperty = serializedObject.FindProperty("m_FontName");
            textProperty = serializedObject.FindProperty("m_Text");

            string[] fontPaths = AssetDatabaseUtility.FindAssets<BMFontData>();
            fontDataNames = new string[fontPaths.Length + 1];
            fontDataNames[0] = "--None--";
            fontDatas = new BMFontData[fontPaths.Length + 1];
            fontDatas[0] = null;

            for(int i =0;i<fontPaths.Length;++i)
            {
                fontDataNames[i + 1] = Path.GetFileNameWithoutExtension(fontPaths[i]);
                fontDatas[i + 1] = AssetDatabase.LoadAssetAtPath<BMFontData>(fontPaths[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            OnDrawFont();
            OnDrawFontNames();

            EditorGUILayout.PropertyField(textProperty);

            serializedObject.ApplyModifiedProperties();
        }

        protected void OnDrawFont()
        {
            BMFontData fontData = (BMFontData)fontDataProperty.objectReferenceValue;
            int selectedIndex = Array.IndexOf(fontDatas, fontData);
            int newSelectedIndex = EditorGUILayout.Popup("Font Data",selectedIndex, fontDataNames);
            if(newSelectedIndex!=selectedIndex)
            {
                selectedIndex = newSelectedIndex;
                fontDataProperty.objectReferenceValue = fontDatas[selectedIndex];
            }
            if(fontData!=null)
            {
                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUILayout.PropertyField(fontDataProperty);
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        protected void OnDrawFontNames()
        {
            BMFontData fontData = (BMFontData)fontDataProperty.objectReferenceValue;
            if(fontData!=null)
            {
                string[] names = fontData.fontNames;
                int selectedIndex = Array.IndexOf(names, fontNameProperty.stringValue);
                int newSelectedIndex = EditorGUILayout.Popup("Font Name", selectedIndex,names);
                if(newSelectedIndex!=selectedIndex)
                {
                    selectedIndex = newSelectedIndex;
                    fontNameProperty.stringValue = names[selectedIndex];
                }
            }else  if(!string.IsNullOrEmpty(fontNameProperty.stringValue))
            {
                fontNameProperty.stringValue = null;
            }
        }
    }
}
