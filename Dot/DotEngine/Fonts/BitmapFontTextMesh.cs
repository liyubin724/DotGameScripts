using UnityEngine;

namespace DotEngine.Fonts
{
    [RequireComponent(typeof(TextMesh))]
    [ExecuteInEditMode]
    public class BitmapFontTextMesh : BitmapFontText
    {
        public TextMesh textMesh = null;
        public MeshRenderer meshRenderer = null;

        protected override void OnTextChanged(string mappedText)
        {
            if(textMesh == null)
            {
                textMesh = GetComponent<TextMesh>();
            }
            if(meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
            if(textMesh!=null)
            {
                if (textMesh.font != FontData.bmFont)
                {
                    textMesh.font = FontData.bmFont;
                    if(meshRenderer!=null)
                    {
                        meshRenderer.material = FontData.bmFont.material;
                    }
                }

                textMesh.text = mappedText;
            }
        }
    }
}
