using Dot.Lua.Event;
using Rotorz.Games.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Lua.Event
{
    [CustomPropertyDrawer(typeof(LuaEventParam))]
    public class LuaEventParamPropertyDrawer : PropertyDrawer
    {
        private const string NULL_NAME = "Null";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ReorderableListGUI.DefaultItemHeight * 4;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect curRect = position;
            curRect.height = ReorderableListGUI.DefaultItemHeight;

            EditorGUI.LabelField(curRect, label, EditorStyles.boldLabel);

            curRect.y += curRect.height;
            curRect.x += 12;

            SerializedProperty paramType = property.FindPropertyRelative("paramType");
            EditorGUI.PropertyField(curRect,paramType);

            if(paramType.intValue == (int)LuaEventParamType.SystemValue)
            {
                curRect.y += curRect.height;
                SerializedProperty systemParamType = property.FindPropertyRelative("systemParamType");
                EditorGUI.PropertyField(curRect,systemParamType);

                curRect.y += curRect.height;
                if(systemParamType.intValue == (int)LuaEventSystemParamType.Int)
                {
                    SerializedProperty valueProperty = property.FindPropertyRelative("intValue");
                    EditorGUI.PropertyField(curRect, valueProperty);
                }
                else if (systemParamType.intValue == (int)LuaEventSystemParamType.Float)
                {
                    SerializedProperty valueProperty = property.FindPropertyRelative("floatValue");
                    EditorGUI.PropertyField(curRect, valueProperty);
                }
                else if(systemParamType.intValue == (int)LuaEventSystemParamType.Bool)
                {
                    SerializedProperty valueProperty = property.FindPropertyRelative("boolValue");
                    EditorGUI.PropertyField(curRect, valueProperty);
                }
                else if (systemParamType.intValue == (int)LuaEventSystemParamType.String)
                {
                    SerializedProperty valueProperty = property.FindPropertyRelative("stringValue");
                    EditorGUI.PropertyField(curRect, valueProperty);
                }
            }
            else if(paramType.intValue == (int)LuaEventParamType.UnityValue)
            {
                curRect.y += curRect.height;
                SerializedProperty unityObject = property.FindPropertyRelative("unityObject");
                EditorGUI.PropertyField(curRect, unityObject);

                curRect.y += curRect.height;
                SerializedProperty unityTypeName = property.FindPropertyRelative("unityTypeName");
                
                List<string> unityObjTypes = new List<string>();
                List<UnityObject> unityObjs = new List<UnityObject>();

                unityObjs.Add(null);
                unityObjTypes.Add(NULL_NAME);
                if(unityObject.objectReferenceValue == null)
                {
                    unityTypeName.stringValue = NULL_NAME;
                }
                else
                {
                    if(unityTypeName.stringValue == NULL_NAME)
                    {
                        unityTypeName.stringValue = unityObject.GetType().Name;
                    }

                    UnityObject uObj = unityObject.objectReferenceValue;
                    GameObject gObj = null;
                    if(uObj.GetType() == typeof(GameObject))
                    {
                        gObj = uObj as GameObject;
                    }else if(uObj.GetType().IsSubclassOf(typeof(Component)))
                    {
                        gObj = (uObj as Component).gameObject;
                    }
                    if(gObj == null)
                    {
                        unityObjs.Add(uObj);
                        unityObjTypes.Add(uObj.GetType().Name);
                    }
                    else
                    {
                        unityObjs.Add(gObj);
                        unityObjTypes.Add(typeof(GameObject).Name);

                        var components = gObj.GetComponents<Component>();
                        foreach (var component in components)
                        {
                            string componentTypeName = component.GetType().Name;
                            if (unityObjTypes.IndexOf(componentTypeName) < 0)
                            {
                                unityObjs.Add(component);
                                unityObjTypes.Add(componentTypeName);
                            }
                        }
                    }
                }

                string typeName = unityTypeName.stringValue;
                int index = -1;
                if (string.IsNullOrEmpty(typeName))
                {
                    index = 0;
                }
                else
                {
                    index = unityObjTypes.IndexOf(typeName);
                    if (index < 0)
                    {
                        index = 0;
                    }
                }

                int newIndex = EditorGUI.Popup(curRect, "Type Name", index, unityObjTypes.ToArray());
                if (newIndex != index || string.IsNullOrEmpty(typeName))
                {
                    unityTypeName.stringValue = unityObjTypes[newIndex];
                    unityObject.objectReferenceValue = unityObjs[newIndex];
                }
            }


        }
    }
}
