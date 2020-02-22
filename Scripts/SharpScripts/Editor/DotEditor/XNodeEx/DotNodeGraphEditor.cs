using Dot.XNodeEx;
using System;
using XNodeEditor;

namespace DotEditor.XNodeEx
{
    public class DotNodeGraphEditor : NodeGraphEditor
    {
        public T GetGraph<T>() where T : DotNodeGraph
        {
            return (T)target;
        }

        public override string GetNodeMenuName(Type type)
        {
            string menuName = XNodeEditorUtil.GetNodeMenuName(type);
            if(string.IsNullOrEmpty(menuName))
            {
                menuName = base.GetNodeMenuName(type);
            }
            return menuName;
        }
    }
}
