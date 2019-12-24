using Rotorz.Games.Collections;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AssetFilter.AssetAddress
{
    [CustomEditor(typeof(AssetAddressGroup))]
    public class AssetAddressGroupEditor : Editor
    {
        SerializedProperty groupName = null;
        SerializedProperty isEnable = null;
        SerializedProperty isMain = null;
        SerializedProperty isPreload = null;
        SerializedProperty isNeverDestroy = null;
        SerializedProperty finders = null;
        SerializedProperty operation = null;

        private void OnEnable()
        {
            groupName = serializedObject.FindProperty("groupName");
            isEnable = serializedObject.FindProperty("isEnable");
            isMain = serializedObject.FindProperty("isMain");
            isPreload = serializedObject.FindProperty("isPreload");
            isNeverDestroy = serializedObject.FindProperty("isNeverDestroy");
            finders = serializedObject.FindProperty("finders");
            operation = serializedObject.FindProperty("operation");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(groupName);
            EditorGUILayout.PropertyField(isEnable);
            
            EditorGUILayout.PropertyField(isMain);
            EditorGUILayout.PropertyField(isPreload);
            EditorGUILayout.PropertyField(isNeverDestroy);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(operation);

            EditorGUILayout.Space();
            ReorderableListGUI.Title("Finders");
            ReorderableListGUI.ListField(finders, ReorderableListGUI.DefaultItemHeight * 5, () =>
            {
                GUILayout.Label("List is empty!", EditorStyles.miniLabel);
            });

            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("Execute",GUILayout.Height(40)))
            {

            }
        }
    }
}
