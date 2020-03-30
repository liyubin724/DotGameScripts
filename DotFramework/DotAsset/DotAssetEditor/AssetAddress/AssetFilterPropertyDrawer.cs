using DotEditor.Core.EGUI;
using Rotorz.Games.Collections;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Asset.AssetAddress
{
    [CustomPropertyDrawer(typeof(AssetFilter))]
    public class AssetFilterPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.CountInProperty() * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty assetFolder = property.FindPropertyRelative("assetFolder");
            SerializedProperty isIncludeSubfolder = property.FindPropertyRelative("isIncludeSubfolder");
            SerializedProperty fileRegex = property.FindPropertyRelative("fileRegex");

            Rect curRect = position;
            curRect.height = EditorGUIUtility.singleLineHeight;

            assetFolder.stringValue = DotEditorGUI.DrawAssetFolderSelection(curRect, "Asset Folder", assetFolder.stringValue);
            
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, isIncludeSubfolder);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, fileRegex);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, inAnyFolderNames);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, inParentFolderNames);
        }

    }


}
