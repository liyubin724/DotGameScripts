//using Dot.Lua.Register;
//using UnityEditor;
//using UnityEngine;

//namespace DotEditor.Lua.Register
//{
//    [CustomPropertyDrawer(typeof(RegisterBehaviourData))]
//    public class RegisterBehaviourDataPropertyDrawer : PropertyDrawer
//    {
//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            return EditorGUIUtility.singleLineHeight * 2+10;
//        }

//        public override bool CanCacheInspectorGUI(SerializedProperty property)
//        {
//            return false;
//        }

//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            Rect propertyRect = position;
//            EditorGUI.LabelField(propertyRect, "", EditorStyles.helpBox);

//            propertyRect.height = EditorGUIUtility.singleLineHeight;

//            SerializedProperty name = property.FindPropertyRelative("name");
//            SerializedProperty behaviour = property.FindPropertyRelative("behaviour");

//            propertyRect.x += 4;
//            propertyRect.width -= 8;
//            EditorGUI.PropertyField(propertyRect, name);

//            propertyRect.y += propertyRect.height;
//            EditorGUI.PropertyField(propertyRect, behaviour);
//            if (behaviour.objectReferenceValue != null)
//            {
//                if (string.IsNullOrEmpty(name.stringValue))
//                {
//                    name.stringValue = behaviour.objectReferenceValue.name;
//                }
//            }
//        }
//    }
//}
