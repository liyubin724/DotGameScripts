using Dot.Lua;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua
{
    [CustomPropertyDrawer(typeof(LuaAsset))]
    public class LuaAssetPropertyDrawer : PropertyDrawer
    {
        private TextAsset textAsset = null;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty scriptPathProperty = property.FindPropertyRelative("scriptFilePath");
            SerializedProperty scriptNameProperty = property.FindPropertyRelative("scriptFileName");

            if (!string.IsNullOrEmpty(scriptPathProperty.stringValue) && textAsset == null)
            {
                string scriptAssetPath = string.Format(LuaConfig.DEFAULT_ASSET_PATH_FORMAT, scriptPathProperty.stringValue);
                textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(scriptAssetPath);
            }

            TextAsset newAsset = (TextAsset)EditorGUI.ObjectField(position, "Lua Script", textAsset, typeof(TextAsset), false);
            if (newAsset != textAsset)
            {
                textAsset = newAsset;
                if (textAsset == null)
                {
                    scriptNameProperty.stringValue = "";
                    scriptPathProperty.stringValue = "";
                }
                else
                {
                    string assetPath = AssetDatabase.GetAssetPath(textAsset);
                    if (assetPath.StartsWith(LuaConfig.DEFAULT_ASSET_DIR) && assetPath.EndsWith(LuaConfig.DEFAULT_SCRIPT_EXTENSION))
                    {
                        assetPath = assetPath.Replace(LuaConfig.DEFAULT_ASSET_DIR+"/", "");
                        assetPath = assetPath.Substring(0, assetPath.LastIndexOf(LuaConfig.DEFAULT_SCRIPT_EXTENSION));
                        scriptNameProperty.stringValue = Path.GetFileNameWithoutExtension(assetPath);
                        scriptPathProperty.stringValue = assetPath;
                    }
                    else
                    {
                        textAsset = null;
                        scriptNameProperty.stringValue = "";
                        scriptPathProperty.stringValue = "";
                    }
                }
            }
        }
    }
}
