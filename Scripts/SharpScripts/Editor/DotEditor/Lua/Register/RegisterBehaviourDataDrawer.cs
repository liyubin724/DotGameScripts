using Dot.Lua.Register;
using Rotorz.Games.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    public class RegisterBehaviourDataDrawer
    {
        private RegisterBehaviourData data = null;
        public RegisterBehaviourDataDrawer(RegisterBehaviourData data)
        {
            this.data = data;
        }

        public void OnInspectorGUI()
        {
            DrawBehaviourDataList();
            DrawBehaviourArrayDataList();
        }

        private void DrawBehaviourDataList()
        {
            ReorderableListGUI.Title("Bind Behaviour List");
            List<BindBehaviourData> dataList = new List<BindBehaviourData>(data.behaviourDatas);
            ReorderableListGUI.ListField<BindBehaviourData>(dataList, (position, item) =>
            {
                if (item == null)
                {
                    item = new BindBehaviourData();
                    GUI.changed = true;
                }

                Rect curRect = position;
                curRect.height = EditorGUIUtility.singleLineHeight;
                item.name = EditorGUI.TextField(curRect, "Name", item.name);
                curRect.y += curRect.height;
                item.behaviour = (LuaScriptBindBehaviour)EditorGUI.ObjectField(curRect, "Behaviour", item.behaviour, typeof(LuaScriptBindBehaviour), false);

                return item;
            }, EditorGUIUtility.singleLineHeight * 2);

            data.behaviourDatas = dataList.ToArray();
        }

        private void DrawBehaviourArrayDataList()
        {
            ReorderableListGUI.Title("Bind Behaviour Array");
            List<BindBehaviourArrayData> dataList = new List<BindBehaviourArrayData>(data.behaviourArrayDatas);
            ReorderableListGUI.ListField<BindBehaviourArrayData>(dataList, (position, item) =>
            {
                if (item == null)
                {
                    item = new BindBehaviourArrayData();
                    GUI.changed = true;
                }

                Rect curRect = position;
                curRect.height = EditorGUIUtility.singleLineHeight;
                item.name = EditorGUI.TextField(curRect, "Name", item.name);

                curRect.y += curRect.height;
                ReorderableListGUI.Title(curRect, "Behaviours");

                curRect.y += curRect.height;
                curRect.height = position.height - EditorGUIUtility.singleLineHeight * 2;

                List<LuaScriptBindBehaviour> behList = new List<LuaScriptBindBehaviour>(item.behaviours);
                ReorderableListGUI.ListFieldAbsolute<LuaScriptBindBehaviour>(curRect, behList, (behPosition, behItem) =>
                {
                    return (LuaScriptBindBehaviour)EditorGUI.ObjectField(behPosition, "Behaviour", behItem, typeof(LuaScriptBindBehaviour), true);
                }, EditorGUIUtility.singleLineHeight);
                item.behaviours = behList.ToArray();

                return item;
            }, (index) =>
            {
                BindBehaviourArrayData arrayData = dataList[index];
                float height = 20;
                if (arrayData.behaviours.Length == 0)
                {
                    return height += 60;
                }
                else
                {
                    return height += 40 + arrayData.behaviours.Length * 20;
                }
            });
            data.behaviourArrayDatas = dataList.ToArray();
        }

    }
}
