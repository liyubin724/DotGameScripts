using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.BMFont
{
    [RequireComponent(typeof(Text))]
    [ExecuteInEditMode]
    public class BMFontUGUIText : BMFontBaseText
    {
       
        private Text uiText = null;
        protected override void Awake()
        {
            uiText = GetComponent<Text>();
            base.Awake();
        }


        public override void OnTextChanged()
        {
            if (uiText == null)
            {
                return;
            }
            if (FontData == null || FontData.bmFont == null)
            {
                uiText.text = "";
                return;
            }
            if (uiText.font != FontData.bmFont)
            {
                uiText.font = FontData.bmFont;
            }
            uiText.text = GetText();
        }
    }
}
