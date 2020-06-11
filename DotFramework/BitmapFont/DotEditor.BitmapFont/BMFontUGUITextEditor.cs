using Game.Core.BMFont;
using GameEditor.Core.BMFont;
using System;
using UnityEditor;

namespace GameEditor.GameEditor.Core.BMFont
{
    [CustomEditor(typeof(BMFontUGUIText))]
    [CanEditMultipleObjects]
    public class BMFontUGUITextEditor : BMFontBaseTextEditor
    {
        protected override void DoTextChanged()
        {
            Array.ForEach(targets, (t) =>
            {
                ((BMFontUGUIText)t).OnTextChanged();
            });
        }
    }
}
