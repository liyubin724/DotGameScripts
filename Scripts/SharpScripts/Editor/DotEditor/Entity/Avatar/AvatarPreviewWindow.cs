using Dot.Avatar;
using Dot.Core.Entity;
using Dot.Entity.Node;
using UnityEditor;

namespace DotEditor.Core.Avatar
{
    public class AvatarPreviewWindow : EditorWindow
    {
        [MenuItem("Game/Avatar/Preview",false,101)]
        public static void ShowWindow()
        {
            GetWindow<AvatarPreviewWindow>().Show();
        }

        public NodeBehaviour skeletonNode = null;
        public AvatarPart[] aParts = new AvatarPart[0];

        private void OnGUI()
        {
            
        }
    }
}
