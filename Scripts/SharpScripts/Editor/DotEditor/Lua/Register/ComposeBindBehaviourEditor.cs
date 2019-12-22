using Dot.Lua.Register;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(ComposeBindBehaviour), true)]
    public class ComposeBindBehaviourEditor : LuaScriptBindBehaviourEditor
    {
        private RegisterObjectDataDrawer objectDataDrawer = null;
        private RegisterBehaviourDataDrawer behaviourDataDrawer = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            RegisterBehaviourData behaviourData = (target as ComposeBindBehaviour).registerBehaviourData;
            behaviourDataDrawer = new RegisterBehaviourDataDrawer(behaviourData);

            RegisterObjectData objectData = (target as ComposeBindBehaviour).registerObjectData;
            objectDataDrawer = new RegisterObjectDataDrawer(objectData);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptInfo();

            serializedObject.ApplyModifiedProperties();

            behaviourDataDrawer.OnInspectorGUI();
            objectDataDrawer.OnInspectorGUI();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
