using DotEditor.Core;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Asset.AssetAddress
{
    [CustomPropertyDrawer(typeof(AssetFilter))]
    public class AssetFilterPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return AssetFilter.FIELD_COUNT * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty assetFolder = property.FindPropertyRelative("assetFolder");
            SerializedProperty isIncludeSubfolder = property.FindPropertyRelative("isIncludeSubfolder");
            SerializedProperty fileRegex = property.FindPropertyRelative("fileRegex");

            Rect curRect = position;
            curRect.height = EditorGUIUtility.singleLineHeight;

            EGUI.DrawAssetFolderSelection(curRect, assetFolder);
            
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, isIncludeSubfolder);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, fileRegex);
        }
    }
}
