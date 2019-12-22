using Dot.Lua.Register;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(ChildBindBehaviour),true)]
    public class ChildBindBehaviourEditor : LuaScriptBindBehaviourEditor
    {
        private RegisterBehaviourDataDrawer dataDrawer = null;
        protected override void OnEnable()
        {
            base.OnEnable();
            RegisterBehaviourData behaviourData = (target as ChildBindBehaviour).registerBehaviourData;

            dataDrawer = new RegisterBehaviourDataDrawer(behaviourData);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptInfo();

            serializedObject.ApplyModifiedProperties();

            dataDrawer.OnInspectorGUI();

            if(GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
