using XNodeEditor;
using static XNode.Node;

namespace DotEditor.XNodeEx
{
    public static class XNodeEditorUtil
    {
        public static string GetNodeMenuName(System.Type type)
        {
            if(type.Namespace == "Dot.XNodeEx.Nodes")
            {
                if (NodeEditorUtilities.GetAttrib(type, out CreateNodeMenuAttribute attrib))
                {
                    return attrib.menuName;
                }
            }
            return null;
        }
    }
}
