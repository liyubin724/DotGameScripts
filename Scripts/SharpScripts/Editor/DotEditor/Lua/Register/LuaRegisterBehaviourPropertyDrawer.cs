using Dot.Lua.Register;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(LuaRegisterBehaviour))]
    public class LuaRegisterBehaviourPropertyDrawer : Editor
    {
        SerializedProperty envType = null;
        SerializedProperty luaAsset = null;
        SerializedProperty regLuaObject = null;
        SerializedProperty regLuaObjectArr = null;
        SerializedProperty regLuaBehaviour = null;
        SerializedProperty regLuaBehaviourArr = null;

        private ReorderableList objectRList = null;
        private ReorderableList objectArrRList = null;
        private ReorderableList behaviourRList = null;
        private ReorderableList behaviourArrRList = null;

        private void OnEnable()
        {
            
            envType = serializedObject.FindProperty("envType");
            luaAsset = serializedObject.FindProperty("luaAsset");
            regLuaObject = serializedObject.FindProperty("regLuaObject");
            regLuaObjectArr = serializedObject.FindProperty("regLuaObjectArr");
            regLuaBehaviour = serializedObject.FindProperty("regLuaBehaviour");
            regLuaBehaviourArr = serializedObject.FindProperty("regLuaBehaviourArr");

            objectRList = new ReorderableList(serializedObject, regLuaObject,true,true,true,true);
            objectRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Reg Object List");
            };
            objectRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                EditorGUI.PropertyField(rect, regLuaObject.GetArrayElementAtIndex(index));
            };
            objectRList.elementHeightCallback = (index) =>
            {
                return EditorGUIUtility.singleLineHeight * 4;
            };

            objectArrRList = new ReorderableList(serializedObject, regLuaObjectArr, true, true, true, true);
            objectArrRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Reg Object Arry List");
            };
            objectArrRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                EditorGUI.PropertyField(rect, regLuaObjectArr.GetArrayElementAtIndex(index));
            };
            objectArrRList.elementHeightCallback = (index) =>
            {
                SerializedProperty property = regLuaObjectArr.GetArrayElementAtIndex(index);
                SerializedProperty objects = property.FindPropertyRelative("objects");

                float height = EditorGUIUtility.singleLineHeight;
                if (objects.arraySize == 0)
                {
                    height += 70;
                }
                else
                {
                    height += objects.arraySize * EditorGUIUtility.singleLineHeight * 4 + 50;
                }

                return height;
            };

            behaviourRList = new ReorderableList(serializedObject, regLuaBehaviour, true, true, true, true);
            behaviourRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Reg Behaviour List");
            };
            behaviourRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                EditorGUI.PropertyField(rect, regLuaBehaviour.GetArrayElementAtIndex(index));
            };
            behaviourRList.elementHeightCallback = (index) =>
            {
                return EditorGUIUtility.singleLineHeight * 2;
            };

            behaviourArrRList = new ReorderableList(serializedObject, regLuaBehaviourArr, true, true, true, true);
            behaviourArrRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Reg Behaviour Array List");
            };
            behaviourArrRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                EditorGUI.PropertyField(rect, regLuaBehaviourArr.GetArrayElementAtIndex(index));
            };
            behaviourArrRList.elementHeightCallback = (index) =>
            {
                SerializedProperty property = regLuaBehaviourArr.GetArrayElementAtIndex(index);
                SerializedProperty behaviours = property.FindPropertyRelative("behaviours");

                float height = EditorGUIUtility.singleLineHeight;
                if (behaviours.arraySize == 0)
                {
                    height += 70;
                }
                else
                {
                    height += behaviours.arraySize * EditorGUIUtility.singleLineHeight + 70;
                }

                return height;
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(envType);
            EditorGUILayout.PropertyField(luaAsset);
            objectRList.DoLayoutList();
            objectArrRList.DoLayoutList();
            behaviourRList.DoLayoutList();
            behaviourArrRList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
