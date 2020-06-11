using UnityEngine;

namespace Game.Core.BMFont
{
    [ExecuteInEditMode]
    public class BMFontBaseText : MonoBehaviour
    {
        [SerializeField]
        private BMFontData m_FontData;
        public BMFontData FontData
        {
            get
            {
                return m_FontData;
            }
            set
            {
                if (m_FontData != value)
                {
                    m_FontData = value;
                    OnTextChanged();
                }
            }
        }
        [SerializeField]
        private string m_FontName;
        public string FontName
        {
            get
            {
                return m_FontName;
            }
            set
            {
                if (m_FontName != value)
                {
                    m_FontName = value;
                    OnTextChanged();
                }
            }
        }
        [SerializeField]
        private string m_Text;
        public string Text
        {
            get
            {
                return m_Text;
            }
            set
            {
                if (m_Text != value)
                {
                    m_Text = value;
                    OnTextChanged();
                }
            }
        }

        protected virtual void Awake()
        {
            OnTextChanged();
        }

        public virtual void OnTextChanged()
        {
            
        }

        protected string GetText()
        {
            return FontData.GetBMFontText(FontName, Text);
        }
    }
}
