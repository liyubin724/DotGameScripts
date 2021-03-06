﻿using DotEditor.Util;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.Core.EGUI
{
    public static class DotEditorGUILayout
    {
        public static void PropertyField(SerializedObject sObj, string propertyName)
        {
            SerializedProperty sProperty = sObj.FindProperty(propertyName);
            EditorGUILayout.PropertyField(sProperty);
        }

        public static void PropertyInfoField(SystemObject target, PropertyInfo pInfo)
        {
            Type type = pInfo.PropertyType;
            if (type == typeof(Vector3))
            {
                EditorGUIPropertyInfoLayout.PropertyInfoVector3Field(target, pInfo);
            }
            else if (type.IsEnum)
            {
                EditorGUIPropertyInfoLayout.PropertyInfoEnumField(target, pInfo);
            }
            else if (type == typeof(bool))
            {
                EditorGUIPropertyInfoLayout.PropertyInfoBoolField(target, pInfo);
            }
            else if (type == typeof(int))
            {
                EditorGUIPropertyInfoLayout.PropertyInfoIntField(target, pInfo);
            }
            else if (type == typeof(float) || type == typeof(double))
            {
                EditorGUIPropertyInfoLayout.PropertyInfoFloatField(target, pInfo);
            }
            else if (type == typeof(string))
            {
                EditorGUIPropertyInfoLayout.PropertyInfoStringField(target, pInfo);
            }
            else
            {
                UnityEditor.EditorGUILayout.LabelField(pInfo.Name, "Unrecognized type!!");
            }
        }

        public static T DrawPopup<T>(string label,string[] contents,T[] values,T selectedValue)
        {
            int index = Array.IndexOf(values, selectedValue);
            if (index < 0) index = 0;
            int newIndex = EditorGUILayout.Popup(label, index, contents);

            return values[newIndex];
        }

        public static void DrawAssetFolderSelection(SerializedProperty property, bool isReadonly = true)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(isReadonly);
                {
                    EditorGUILayout.PropertyField(property);
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button(new GUIContent(DotEditorGUI.FolderIcon), GUILayout.Width(20), GUILayout.Height(20)))
                {
                    string folderPath = EditorUtility.OpenFolderPanel("folder", property.stringValue, "");
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        property.stringValue = PathUtil.GetAssetPath(folderPath);
                    }
                }
                if (GUILayout.Button("\u2716", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    property.stringValue = "";
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public static string DrawAssetFolderSelection(string label, string assetFolder, bool isReadonly = true)
        {
            string folder = assetFolder;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(isReadonly);
                {
                    folder = EditorGUILayout.TextField(label, assetFolder);
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button(new GUIContent(DotEditorGUI.FolderIcon), GUILayout.Width(20), GUILayout.Height(20)))
                {
                    string folderPath = EditorUtility.OpenFolderPanel("folder", folder, "");
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        folder = PathUtil.GetAssetPath(folderPath);
                    }
                }
                if (GUILayout.Button("\u2716", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    folder = "";
                }
            }
            EditorGUILayout.EndHorizontal();
            return folder;
        }

        public static string DrawDiskFolderSelection(string label, string diskFolder, bool isReadonly = true)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(isReadonly);
                {
                    EditorGUILayout.TextField(label, diskFolder);
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button(new GUIContent(DotEditorGUI.FolderIcon), GUILayout.Width(20), GUILayout.Height(20)))
                {
                    diskFolder = EditorUtility.OpenFolderPanel("folder", diskFolder, "");
                }
                if (GUILayout.Button("\u2716", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    diskFolder = "";
                }
            }
            EditorGUILayout.EndHorizontal();

            return diskFolder;
        }

        public static string StringPopup(string label,string selected,string[] optionValues)
        {
            if(optionValues == null)
            {
                optionValues = new string[0];
            }

            int selectedIndex = Array.IndexOf(optionValues, selected);

            int newSelectedIndex = EditorGUILayout.Popup(label, selectedIndex, optionValues);
            if(newSelectedIndex>=0 && newSelectedIndex<optionValues.Length)
            {
                return optionValues[newSelectedIndex];
            }
            return selected;
        }

        public static string StringPopup(GUIContent label, string selected, string[] optionValues)
        {
            if (optionValues == null)
            {
                optionValues = new string[0];
            }

            int selectedIndex = Array.IndexOf(optionValues, selected);

            int newSelectedIndex = EditorGUILayout.Popup(label, selectedIndex, optionValues);
            if (newSelectedIndex >= 0 && newSelectedIndex < optionValues.Length)
            {
                return optionValues[newSelectedIndex];
            }
            return selected;
        }
    }
}
