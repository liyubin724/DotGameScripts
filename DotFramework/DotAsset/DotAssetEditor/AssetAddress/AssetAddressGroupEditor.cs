using DotEditor.Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Asset.AssetAddress
{
    [CustomEditor(typeof(AssetAddressGroup))]
    public class AssetAddressGroupEditor : Editor
    {
        SerializedProperty groupName = null;
        SerializedProperty isEnable = null;
        SerializedProperty isMain = null;
        SerializedProperty isPreload = null;
        SerializedProperty isNeverDestroy = null;
        SerializedProperty operation = null;
        SerializedProperty filters = null;

        ReorderableList filterRList = null;
        List<string> filterResList = new List<string>();
        ReorderableList filterResRList = null;
        private void OnEnable()
        {
            groupName = serializedObject.FindProperty("groupName");
            isEnable = serializedObject.FindProperty("isEnable");
            isMain = serializedObject.FindProperty("isMain");
            isPreload = serializedObject.FindProperty("isPreload");
            isNeverDestroy = serializedObject.FindProperty("isNeverDestroy");
            operation = serializedObject.FindProperty("operation");
            
            filters = serializedObject.FindProperty("filters");
            filterRList = new ReorderableList(serializedObject, filters,true,true,true,true);
            filterRList.elementHeight = 3 * EditorGUIUtility.singleLineHeight;
            filterRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, new GUIContent("Filters"));
            };
            filterRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty property = filters.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, property);
            };
            filterRList.onAddCallback = (list) =>
            {
                filters.InsertArrayElementAtIndex(filters.arraySize);
            };
            filterRList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) =>
              {
                  if(isActive)
                  {
                      EditorGUI.DrawRect(rect, Color.blue);
                  }else
                  {
                      if (index % 2 == 0)
                      {
                          EditorGUI.DrawRect(rect, EGUIResources.BackgroundColor);
                      }
                      else
                      {
                          EditorGUI.DrawRect(rect, EGUIResources.BorderColor);
                      }
                  }
                  
              };

            filterResRList = new ReorderableList(filterResList, typeof(string), false, true, false, false);
            filterResRList.elementHeight = EditorGUIUtility.singleLineHeight;
            filterResRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, new GUIContent("Filter Res List"));
            };
            filterResRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                string assetPath = filterResList[index];
                EGUI.BeginLabelWidth(40);
                {
                    EditorGUI.TextField(rect, "" + index, assetPath);
                }
                EGUI.EndLableWidth();
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EGUILayout.DrawScript(target);

            EditorGUILayout.PropertyField(groupName);
            EditorGUILayout.PropertyField(isEnable);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(isMain);
            EditorGUILayout.PropertyField(isPreload);
            EditorGUILayout.PropertyField(isNeverDestroy);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(operation);

            EditorGUILayout.Space();

            filterRList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Execute",GUILayout.Height(40)))
            {
                AssetAddressUtil.UpdateConfigByGroup(target as AssetAddressGroup);
                EditorUtility.DisplayDialog("Finished", "Finished", "OK");
            }

            if (GUILayout.Button("Filter", GUILayout.Height(40)))
            {
                filterResList.Clear();
                AssetAddressGroup group = target as AssetAddressGroup;
                foreach (var filter in group.filters)
                {
                    filterResList.AddRange(filter.Filter());
                }
            }

            filterResRList.DoLayoutList();
        }
    }
}
