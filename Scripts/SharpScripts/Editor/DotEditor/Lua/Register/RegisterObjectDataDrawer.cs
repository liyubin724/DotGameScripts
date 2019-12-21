using Dot.Lua.Register;
using Rotorz.Games.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace DotEditor.Lua.Register
{
    public class RegisterObjectDataDrawer
    {
        private RegisterObjectData data = null;

        public RegisterObjectDataDrawer(RegisterObjectData data)
        {
            this.data = data;
        }

        public void OnInspectorGUI()
        {
            DrawObjectDataList();
            DrawObjectArrayDataList();
        }

        private ObjectData DrawObjectData(Rect position,ObjectData item)
        {
            if (item == null)
            {
                item = new ObjectData();
                GUI.changed = true;
            }

            Rect curRect = position;
            curRect.height = EditorGUIUtility.singleLineHeight;
            item.name = EditorGUI.TextField(curRect, "Name", item.name);
            curRect.y += curRect.height;
            item.obj = (GameObject)EditorGUI.ObjectField(curRect, "Obj", item.obj, typeof(GameObject), true);
            if (item.obj == null)
            {
                item.regObj = null;
                item.typeName = string.Empty;
            }
            else if (string.IsNullOrEmpty(item.name))
            {
                item.name = item.obj.name;
            }

            curRect.y += curRect.height;
            if (item.obj == null)
            {
                EditorGUI.LabelField(curRect, "Type Name", "Null");
            }
            else
            {
                List<string> componentTypeNames = new List<string>();
                componentTypeNames.Add(typeof(GameObject).Name);

                GameObject uObj = item.obj as GameObject;
                var components = uObj.GetComponents<Component>();
                foreach (var component in components)
                {
                    string componentTypeName = component.GetType().Name;
                    if (componentTypeNames.IndexOf(componentTypeName) < 0)
                    {
                        componentTypeNames.Add(componentTypeName);
                    }
                }

                int index = -1;
                if (string.IsNullOrEmpty(item.typeName))
                {
                    index = 0;
                }
                else
                {
                    index = componentTypeNames.IndexOf(item.typeName);
                    if (index < 0)
                    {
                        index = 0;
                    }
                }

                int newIndex = EditorGUI.Popup(curRect, "Type Name", index, componentTypeNames.ToArray());
                if (newIndex != index || string.IsNullOrEmpty(item.typeName))
                {
                    item.typeName = componentTypeNames[newIndex];
                    if (newIndex == 0)
                    {
                        item.regObj = uObj;
                    }
                    else
                    {
                        item.regObj = components[newIndex - 1];
                    }
                }
            }

            curRect.y += curRect.height;
            EditorGUI.BeginDisabledGroup(true);
            {
                if (item.regObj != null)
                {
                    EditorGUI.ObjectField(curRect, "Reg Obj", item.regObj, item.regObj.GetType(), true);
                }
                else
                {
                    EditorGUI.ObjectField(curRect, "Reg Obj", item.regObj, typeof(GameObject), true);
                }
            }
            EditorGUI.EndDisabledGroup();

            return item;
        }

        private void DrawObjectDataList()
        {
            ReorderableListGUI.Title("Bind Object List");
            List<ObjectData> dataList = new List<ObjectData>(data.objectDatas);
            ReorderableListGUI.ListField<ObjectData>(dataList, (position, item) =>
            {
                return DrawObjectData(position,item);
            }, EditorGUIUtility.singleLineHeight * 4);

            data.objectDatas = dataList.ToArray();
        }

        private ObjectArrayData DrawObjectArrayData(Rect position,ObjectArrayData item)
        {
            if(item == null)
            {
                item = new ObjectArrayData();
                GUI.changed = true;
            }
            Rect curRect = position;
            curRect.height = EditorGUIUtility.singleLineHeight;
            item.name = EditorGUI.TextField(curRect, "Name", item.name);

            curRect.y += curRect.height;
            ReorderableListGUI.Title(curRect,"Bind Object List");

            curRect.y += curRect.height;
            curRect.height = position.height - curRect.height * 2;

            List<ObjectData> dataList = new List<ObjectData>(item.objects);
            ReorderableListGUI.ListFieldAbsolute<ObjectData>(curRect, dataList,(childPosition, childItem) =>
            {
                return DrawObjectData(childPosition, childItem);
            }, EditorGUIUtility.singleLineHeight * 4);

            item.objects = dataList.ToArray();

            return item;
        }

        private void DrawObjectArrayDataList()
        {
            ReorderableListGUI.Title("Bind Object Array List");
            List<ObjectArrayData> dataList = new List<ObjectArrayData>(data.objectArrayDatas);
            ReorderableListGUI.ListField<ObjectArrayData>(dataList, (position, item) =>
            {
                return DrawObjectArrayData(position, item);
            }, (index) =>
            {
                ObjectArrayData arrayData = dataList[index];
                float height = 20;
                if (arrayData.objects.Length == 0)
                {
                    return height += 60;
                }
                else
                {
                    return height += 40 + arrayData.objects.Length * 20*4;
                }
            });

            data.objectArrayDatas = dataList.ToArray();
        }


    }
}
