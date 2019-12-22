using Dot.Lua.Register;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(ObjectBindBehaviour), true)]
    public class ObjectBindBehaviourEditor : LuaScriptBindBehaviourEditor
    {
        private RegisterObjectDataDrawer dataDrawer = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            RegisterObjectData objectData = (target as ObjectBindBehaviour).registerObjectData;
            dataDrawer = new RegisterObjectDataDrawer(objectData);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptInfo();

            serializedObject.ApplyModifiedProperties();

            dataDrawer.OnInspectorGUI();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
