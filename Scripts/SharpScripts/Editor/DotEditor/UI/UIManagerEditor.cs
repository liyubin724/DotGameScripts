using Dot.UI;
using DotEditor.Core.EGUI;
using DotEditor.Lua.Register;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.UI
{
    [CustomEditor(typeof(UIManager))]
    public class UIManagerEditor : LuaScriptBindBehaviourEditor
    {
        SerializedProperty uiLayers = null;
        ReorderableList uiLayerRList = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            uiLayers = serializedObject.FindProperty("uiLayers");
            uiLayerRList = new ReorderableList(serializedObject, uiLayers, true, false, true, true);
            uiLayerRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty element = uiLayers.GetArrayElementAtIndex(index);
                EditorGUIUtil.BeginLabelWidth(40);
                {
                    EditorGUI.PropertyField(rect, element,new GUIContent(""+index));
                }
                EditorGUIUtil.EndLableWidth();
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptInfo();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("UI Layers");
            uiLayerRList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
