using UnityEngine;

namespace Game.Core.BMFont
{
    [RequireComponent(typeof(TextMesh))]
    [ExecuteInEditMode]
    public class BMFontTextMesh : BMFontBaseText
    {
        
        private TextMesh textMesh = null;
        private MeshRenderer textMeshRenderer = null;
        protected override void Awake()
        {
            textMesh = GetComponent<TextMesh>();
            textMeshRenderer = GetComponent<MeshRenderer>();

            base.Awake();
        }

        public override void OnTextChanged()
        {
            if(textMesh == null)
            {
                return;
            }
            if(FontData == null || FontData.bmFont == null)
            {
                textMesh.text = "";
                return;
            }
            if(textMesh.font != FontData.bmFont)
            {
                textMesh.font = FontData.bmFont;
                textMeshRenderer.material = textMesh.font.material;
            }
            textMesh.text = GetText();
        }
    }
}
