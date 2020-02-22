using Dot.XNodeEx.Nodes;
using UnityEditor;

namespace DotEditor.XNodeEx
{
    [CustomNodeEditor(typeof(DebugLogNode))]
    public class DebugLogNodeEditor : DotNodeEditor
    {
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            DebugLogNode debugLog = target as DebugLogNode;
            object value = debugLog.GetValue();
            if(value!=null)
            {
                //EditorGUILayout.LabelField(value.ToString());
            }
        }
    }
}
