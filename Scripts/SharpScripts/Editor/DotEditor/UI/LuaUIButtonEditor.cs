using Dot.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace DotEditor.UI
{
    [CustomEditor(typeof(LuaUIButton), true)]
    public class LuaUIButtonEditor : SelectableEditor
    {
        SerializedProperty eventData = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            eventData = serializedObject.FindProperty("eventData");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(eventData);

            serializedObject.ApplyModifiedProperties();

            if(GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
