using System;
using UnityEngine;

namespace DotEditor.EGUI.Toolbar
{
    public abstract class EditorToolbarItem
    {
        public GUIContent LabelContent { get; set; } = GUIContent.none;
        public Action OnClickAction { get; set; } = null;

        protected EditorToolbarItem(Action onClick,GUIContent label)
        {
            OnClickAction = onClick;
            LabelContent = label;
        }

        protected EditorToolbarItem(Action onClick,string label,string tooltip):this(onClick,new GUIContent(label,tooltip))
        {
        }

        protected EditorToolbarItem(Action onClick,Texture icon,string tooltip):this(onClick,new GUIContent(icon,tooltip))
        {

        }

        public virtual float GetItemWidth()
        {
            return 24f;
        }

        protected internal abstract void OnItemGUI(Rect rect, GUIStyle style);
    }

    public class EditorToolbarButton : EditorToolbarItem
    {
        public EditorToolbarButton(Action onClick, GUIContent label) : base(onClick, label)
        {
        }

        public EditorToolbarButton(Action onClick, string label, string tooltip) : base(onClick, label, tooltip)
        {
        }

        public EditorToolbarButton(Action onClick, Texture icon, string tooltip) : base(onClick, icon, tooltip)
        {
        }

        protected internal override void OnItemGUI(Rect rect,GUIStyle style)
        {
            if(GUI.Button(rect,LabelContent,style))
            {
                OnClickAction?.Invoke();
            }
        }
    }

    //public class EditorToolbarDropdownMenu : EditorToolbarItem
    //{
    //    public EditorToolbarDropdownMenu(Action onClick, GUIContent label, int order) : base(onClick, label, order)
    //    {
    //    }

    //    public EditorToolbarDropdownMenu(Action onClick, string label, string tooltip, int order) : base(onClick, label, tooltip, order)
    //    {
    //    }

    //    public EditorToolbarDropdownMenu(Action onClick, Texture icon, string tooltip, int order) : base(onClick, icon, tooltip, order)
    //    {
    //    }

    //    protected internal override void OnItemGUI(Rect rect, GUIStyle style)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
