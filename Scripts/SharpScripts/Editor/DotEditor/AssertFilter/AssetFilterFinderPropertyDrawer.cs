using DotEditor.Core.EGUI;
using Rotorz.Games.Collections;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AssertFilter
{
    [CustomPropertyDrawer(typeof(AssetFilterFinder))]
    public class AssetFilterFinderPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.CountInProperty() * ReorderableListGUI.DefaultItemHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty assetFolder = property.FindPropertyRelative("assetFolder");
            SerializedProperty isIncludeSubfolder = property.FindPropertyRelative("isIncludeSubfolder");
            SerializedProperty fileNameRegex = property.FindPropertyRelative("fileNameRegex");
            SerializedProperty inAnyFolderNames = property.FindPropertyRelative("inAnyFolderNames");
            SerializedProperty inParentFolderNames = property.FindPropertyRelative("inParentFolderNames");

            Rect curRect = position;
            curRect.height = ReorderableListGUI.DefaultItemHeight;

            assetFolder.stringValue = EditorGUIUtil.DrawAssetFolderSelection(curRect, "Asset Folder", assetFolder.stringValue);
            
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, isIncludeSubfolder);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, fileNameRegex);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, inAnyFolderNames);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, inParentFolderNames);
        }
    }
}
