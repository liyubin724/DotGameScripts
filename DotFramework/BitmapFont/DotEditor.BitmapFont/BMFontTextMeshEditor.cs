using Game.Core.BMFont;
using System;
using UnityEditor;

namespace GameEditor.Core.BMFont
{
    [CustomEditor(typeof(BMFontTextMesh))]
    [CanEditMultipleObjects]
    public class BMFontTextMeshEditor : BMFontBaseTextEditor
    {
        protected override void DoTextChanged()
        {
            Array.ForEach(targets, (t) =>
            {
                ((BMFontTextMesh)t).OnTextChanged();
            });
        }
    }
}
