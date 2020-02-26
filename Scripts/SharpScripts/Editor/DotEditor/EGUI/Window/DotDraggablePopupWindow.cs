using UnityEngine;

namespace DotEditor.EGUI.Window
{
    /// <summary>
    /// 通过继承DraggablePopupWindow可以生成一个可拖动的弹框
    /// </summary>
    public abstract class DotDraggablePopupWindow : DotPopupWindow
    {
        private Vector2 offset;

        protected void OnGUIDrag()
        {
            var e = Event.current;
            if (e.button == 0 && e.type == EventType.MouseDown)
            {
                offset = position.position - GUIUtility.GUIToScreenPoint(e.mousePosition);
            }

            if (e.button == 0 && e.type == EventType.MouseDrag)
            {
                var mousePos = GUIUtility.GUIToScreenPoint(e.mousePosition);
                position = new Rect(mousePos + offset, position.size);
            }
        }

        protected override void OnGUI()
        {
            base.OnGUI();

            OnGUIDrag();
        }
    }

}
