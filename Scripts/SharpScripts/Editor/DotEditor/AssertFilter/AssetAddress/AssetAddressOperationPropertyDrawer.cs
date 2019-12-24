using UnityEditor;
using UnityEngine;

namespace DotEditor.AssertFilter.AssetAddress
{
    [CustomPropertyDrawer(typeof(AssetAddressOperation))]
    public class AssetAddressOperationPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (property.CountInProperty() + 1) * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect curRect = position;
            curRect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(curRect, label, EditorStyles.boldLabel);

            curRect.x += 16;
            curRect.width -= 16;

            SerializedProperty packModeType = property.FindPropertyRelative("packModeType");
            SerializedProperty addressMode = property.FindPropertyRelative("addressMode");
            SerializedProperty isMD5ForBundleName = property.FindPropertyRelative("isMD5ForBundleName");
            SerializedProperty labels = property.FindPropertyRelative("labels");
            SerializedProperty compressionType = property.FindPropertyRelative("compressionType");

            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, packModeType);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, addressMode);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, isMD5ForBundleName);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, labels);
            curRect.y += curRect.height;
            EditorGUI.PropertyField(curRect, compressionType);
        }
    }
}
