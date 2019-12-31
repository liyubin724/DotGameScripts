using DotEditor.Core.Window;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.FieldDrawer
{
    public class FieldDescPopWindow : DotPopupWindow
    {
        public static void ShowWin(Rect position,string fieldName,string fieldDesc)
        {
            var win = GetPopupWindow<FieldDescPopWindow>();
            win.fieldName = fieldName;
            win.fieldDesc = fieldDesc;
            win.Show<FieldDescPopWindow>(new Rect(position.x-100, position.y+position.height, 200, 100), true, true);
        }

        private string fieldName = "";
        private string fieldDesc = "";

        private GUIStyle boldLabelCenterStyle = null;
        private GUIStyle labelWrapStyle = null;
        protected override void OnGUI()
        {
            base.OnGUI();

            if(boldLabelCenterStyle == null)
            {
                boldLabelCenterStyle = new GUIStyle(EditorStyles.boldLabel);
                boldLabelCenterStyle.alignment = TextAnchor.MiddleCenter;
                boldLabelCenterStyle.fontSize = 15;
            }
            if(labelWrapStyle == null)
            {
                labelWrapStyle = new GUIStyle(EditorStyles.label);
                labelWrapStyle.wordWrap = true;
            }

            EditorGUILayout.LabelField(fieldName, boldLabelCenterStyle);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(fieldDesc, labelWrapStyle, GUILayout.ExpandHeight(true));

        }
    }
}
