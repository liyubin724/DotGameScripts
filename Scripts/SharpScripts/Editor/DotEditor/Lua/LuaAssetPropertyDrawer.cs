using Dot.Lua;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua
{
    [CustomPropertyDrawer(typeof(LuaAsset))]
    public class LuaAssetPropertyDrawer : PropertyDrawer
    {
        private Dictionary<string, TextAsset> assetDic = new Dictionary<string, TextAsset>();
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty scriptPathProperty = property.FindPropertyRelative("scriptFilePath");

            string pPath = scriptPathProperty.propertyPath;
            TextAsset textAsset = null;
            if (!string.IsNullOrEmpty(scriptPathProperty.stringValue))
            {
                string scriptAssetPath = string.Format(LuaConfig.DEFAULT_ASSET_PATH_FORMAT, scriptPathProperty.stringValue);
                textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(scriptAssetPath);
            }

            if(assetDic.ContainsKey(pPath))
            {
                assetDic[pPath] = textAsset;
            }else
            {
                assetDic.Add(pPath, textAsset);
            }

            TextAsset newAsset = (TextAsset)EditorGUI.ObjectField(position, "Lua Script", textAsset, typeof(TextAsset), false);
            if (newAsset != textAsset)
            {
                if (newAsset == null)
                {
                    scriptPathProperty.stringValue = "";
                }
                else
                {
                    string assetPath = AssetDatabase.GetAssetPath(newAsset);
                    if (assetPath.StartsWith(LuaConfig.DEFAULT_ASSET_DIR) && assetPath.EndsWith(LuaConfig.DEFAULT_SCRIPT_EXTENSION))
                    {
                        assetPath = assetPath.Replace(LuaConfig.DEFAULT_ASSET_DIR+"/", "");
                        assetPath = assetPath.Substring(0, assetPath.LastIndexOf(LuaConfig.DEFAULT_SCRIPT_EXTENSION));
                        scriptPathProperty.stringValue = assetPath;
                    }
                    else
                    {
                        newAsset = null;
                        scriptPathProperty.stringValue = "";
                    }
                }

                assetDic[pPath] = newAsset;
            }
        }
    }
}
