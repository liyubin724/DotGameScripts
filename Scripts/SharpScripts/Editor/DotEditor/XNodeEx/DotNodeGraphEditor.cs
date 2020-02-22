using System;
using XNodeEditor;

namespace DotEditor.XNodeEx
{
    public class DotNodeGraphEditor : NodeGraphEditor
    {
        public override string GetNodeMenuName(Type type)
        {
            return XNodeEditorUtil.GetNodeMenuName(type);
        }
    }
}
