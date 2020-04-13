using DotEditor.Core.Utilities;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.Utilities
{
    public static class PropertyUtility
    {
        internal const string DEFAULT_SCRIPT_PROPERTY_TYPE_NAME = "PPtr<MonoScript>";
        internal const string DEFAULT_SCRIPT_PROPERTY_PATH = "m_Script";

        public static bool IsDefaultScriptProperty(SerializedProperty property)
        {
            return property.propertyPath == DEFAULT_SCRIPT_PROPERTY_PATH;
        }

        public static bool IsDefaultScriptPropertyByPath(string propertyPath)
        {
            return propertyPath == DEFAULT_SCRIPT_PROPERTY_PATH;
        }

        public static bool IsDefaultScriptPropertyByTypeName(string propertyType)
        {
            return propertyType == DEFAULT_SCRIPT_PROPERTY_TYPE_NAME;
        }

        public static string GetPropertyKey(SerializedProperty property)
        {
            return property.serializedObject.GetHashCode() + "$" + property.propertyPath;
        }

        public static bool IsPropertyHasCustomDrawer(SerializedProperty property)
        {
            GetPropertyFieldInfo(property, out Type propertyType);
            return IsPropertyHasCustomDrawer(property, propertyType);
        }

        public static bool IsPropertyHasCustomDrawer(SerializedProperty property,Type drawerType)
        {
            if (IsBuiltInNumericProperty(property))
            {
                return true;
            }

            if(GetDrawerTypeForType == null)
            {
                ReflectionMethod();
            }

            var parameters = new object[] { drawerType };
            var result = GetDrawerTypeForType.Invoke(null, parameters) as Type;
            return result != null && typeof(PropertyDrawer).IsAssignableFrom(result);
        }

        public static FieldInfo GetPropertyFieldInfo(SerializedProperty property, out Type propertyType)
        {
            if (GetFieldInfoFromProperty == null)
            {
                ReflectionMethod();
            }

            var paramters = new object[] { property, null };
            var result = GetFieldInfoFromProperty.Invoke(null, paramters);
            propertyType = paramters[1] as Type;
            return result as FieldInfo;
        }

        private static bool IsBuiltInNumericProperty(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Bounds:
                case SerializedPropertyType.BoundsInt:
                case SerializedPropertyType.Rect:
                case SerializedPropertyType.RectInt:
                case SerializedPropertyType.Quaternion:
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector2Int:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector3Int:
                case SerializedPropertyType.Vector4:
                    return true;
            }

            return false;
        }

        private static MethodInfo GetDrawerTypeForType = null;
        private static MethodInfo GetFieldInfoFromProperty = null;
        private static void ReflectionMethod()
        {
            Type scriptAttributeUtility = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ScriptAttributeUtility");
            GetFieldInfoFromProperty = ReflectionUtility.GetMethod(scriptAttributeUtility, "GetFieldInfoFromProperty");
            GetDrawerTypeForType = ReflectionUtility.GetMethod(scriptAttributeUtility, "GetDrawerTypeForType");
            if(GetFieldInfoFromProperty == null || GetDrawerTypeForType == null)
            {
                Debug.LogError("PropertyUtility::RelectionMethod->Method not found");
            }
        }

    }
}
