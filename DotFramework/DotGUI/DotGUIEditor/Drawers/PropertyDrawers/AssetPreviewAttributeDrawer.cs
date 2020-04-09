using Dot.GUI.Attributes;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.Drawers
{
    [CustomPropertyDrawer(typeof(AssetPreviewAttribute))]
    public class AssetPreviewAttributeDrawer : EGUIPropertyDrawer
    {
        private AssetPreviewAttribute Attribute => GetAttribute<AssetPreviewAttribute>();

        protected override void OnGUISafe(Rect position, SerializedProperty property, GUIContent label)
        {
            if(Attribute.UseLabel)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            else
            {
                position.y -= DEGUIUtility.singleLineHeight;
            }

            if(property.objectReferenceValue !=null)
            {
                var previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);

                if (!previewTexture) return;
                var indent = position.width - EditorGUI.IndentedRect(position).width;

                var width = Mathf.Clamp(Attribute.Width, 0, previewTexture.width);
                var height = Mathf.Clamp(Attribute.Height, 0, previewTexture.height);

                //set additional height as preview + 2x spacing + 2x frame offset
                position.height = height + DEGUIUtility.boxFrameSize;
                position.width = width + DEGUIUtility.boxFrameSize + indent;
                position.y += DEGUIUtility.singleLineHeight + DEGUIUtility.standSpacing;
                //draw frame
                EditorGUI.LabelField(position, GUIContent.none, DEGUIStyles.BoxStyle);

                position.height = height;
                position.width = width + indent;
                //adjust image to frame center
                position.y += DEGUIUtility.boxFrameSize / 2;
                position.x += DEGUIUtility.boxFrameSize / 2;
                //draw preview texture
                EditorGUI.LabelField(position, GUIContent.none, DEGUIStyles.GetTextureStyle(previewTexture));
            }
        }

        public override bool IsPropertyValid(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.ObjectReference;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!IsPropertyValid(property) || property.objectReferenceValue == null)
            {
                return DEGUIUtility.singleLineHeight;
            }
            var additionalHeight = Attribute.Height + DEGUIUtility.boxFrameSize * 2 + DEGUIUtility.standSpacing * 2;
            if (!Attribute.UseLabel)
            {
                additionalHeight -= DEGUIUtility.singleLineHeight;
            }

            return DEGUIUtility.singleLineHeight + additionalHeight;
        }
    }
}
