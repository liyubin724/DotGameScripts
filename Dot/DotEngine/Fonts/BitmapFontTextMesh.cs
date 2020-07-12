using DotEngine.Fonts;
using UnityEngine;

namespace DotEngine.Fonts
{
    [RequireComponent(typeof(TextMesh))]
    public class BitmapFontTextMesh : BitmapFontText
    {
        public TextMesh textMesh = null;
        protected override void Awake()
        {
            if(textMesh == null)
            {
                textMesh = GetComponent<TextMesh>();
            }
            if(textMesh == null)
            {
                enabled = false;
            }
            else
            {
                textMesh.font = FontData.bmFont;
            }
        }

        protected override void OnTextChanged(string mappedText)
        {
            textMesh.text = mappedText;
        }
    }
}
