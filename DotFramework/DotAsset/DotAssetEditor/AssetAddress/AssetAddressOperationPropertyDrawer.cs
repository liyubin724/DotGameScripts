using UnityEditor;
using UnityEngine;

namespace DotEditor.Asset.AssetAddress
{
    [CustomPropertyDrawer(typeof(AssetAddressOperation))]
    public class AssetAddressOperationPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 6 * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect curRect = position;
            curRect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(curRect, label, EditorStyles.boldLabel);

            curRect.x += 16;
            curRect.width -= 16;

            SerializedProperty packMode = property.FindPropertyRelative("packMode");
            SerializedProperty addressMode = property.FindPropertyRelative("addressMode");
            SerializedProperty bundleNameType = property.FindPropertyRelative("bundleNameType");
            SerializedProperty labels = property.FindPropertyRelative("labels");
            SerializedProperty compressionType = property.FindPropertyRelative("compressionType");

            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, packMode);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, addressMode);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, bundleNameType);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, labels);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, compressionType);
        }
    }
}
