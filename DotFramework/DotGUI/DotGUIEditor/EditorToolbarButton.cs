using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEditor.GUI
{
    public abstract class EditorToolbarItem
    {
        public int Order { get; private set; } = 0;
        public Action OnClickHandler { get; private set; } = null;
        public GUIContent LabelContent { get; private set; } = GUIContent.none;

        public EditorToolbarItem(Action onClick,GUIContent label,int order)
        {
            OnClickHandler = onClick;
            LabelContent = label;
            Order = order;
        }

        public EditorToolbarItem(Action onClick,string label,int order):this(onClick,new GUIContent(label),order)
        {
        }

        public EditorToolbarItem(Action onClick,Texture2D icon,int order):this(onClick,new GUIContent(icon),order)
        {
        }


    }

    public class EditorToolbarButton
    {
    }
}
