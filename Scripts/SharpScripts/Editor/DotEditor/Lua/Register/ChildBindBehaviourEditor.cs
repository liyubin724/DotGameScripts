using Dot.Lua.Register;
using Rotorz.Games.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(ChildBindBehaviour))]
    public class ChildBindBehaviourEditor : LuaScriptBindBehaviourEditor
    {
        private SerializedProperty registerBehaviourData = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            registerBehaviourData = serializedObject.FindProperty("registerBehaviourData");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            RegisterBehaviourData data = (target as ChildBindBehaviour).registerBehaviourData;
            List<BindBehaviourData> behDatas = new List<BindBehaviourData>(data.behaviourDatas);

            ReorderableListGUI.Title("Behaviour Data");
            ReorderableListGUI.ListField<BindBehaviourData>(behDatas, (position, item) =>
            {
                if(item == null)
                {
                    item = new BindBehaviourData();
                    GUI.changed = true;
                }

                Rect curRect = position;
                curRect.height = EditorGUIUtility.singleLineHeight;
                item.name = EditorGUI.TextField(curRect,"Name", item.name);
                curRect.y += curRect.height;
                item.behaviour = (LuaScriptBindBehaviour)EditorGUI.ObjectField(curRect,"Behaviour", item.behaviour, typeof(LuaScriptBindBehaviour), false);

                return item;
            },EditorGUIUtility.singleLineHeight * 2);

            data.behaviourDatas = behDatas.ToArray();

            List<BindBehaviourArrayData> behaviourArrayDatas = new List<BindBehaviourArrayData>(data.behaviourArrayDatas);
            ReorderableListGUI.Title("Behaviour Array");
            ReorderableListGUI.ListField<BindBehaviourArrayData>(behaviourArrayDatas, (position, item) =>
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
                ReorderableListGUI.ListFieldAbsolute<LuaScriptBindBehaviour>(curRect,behList, (behPosition, behItem) =>
                {
                    return (LuaScriptBindBehaviour)EditorGUI.ObjectField(behPosition, "Behaviour",behItem,typeof(LuaScriptBindBehaviour),true);
                }, EditorGUIUtility.singleLineHeight);
                item.behaviours = behList.ToArray();

                return item;
            }, (index) =>
            {
                BindBehaviourArrayData arrayData = behaviourArrayDatas[index];
                float height = 20;
                if(arrayData.behaviours.Length == 0)
                {
                    return height += 60;
                }else
                {
                    return height += 40 + arrayData.behaviours.Length * 20;
                }
            });
            data.behaviourArrayDatas = behaviourArrayDatas.ToArray();


            serializedObject.ApplyModifiedProperties();
        }



    }
}
