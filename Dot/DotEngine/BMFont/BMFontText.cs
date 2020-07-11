﻿using UnityEngine;

namespace DotEngine.BMFont
{
    public class BMFontText : MonoBehaviour
    {
        [SerializeField]
        private BMFontData m_FontData = null;
        public BMFontData FontData
        {
            get
            {
                return m_FontData;
            }
            set
            {
                if(m_FontData != value)
                {
                    m_FontData = value;
                    TextChanged();
                }
            }
        }

        [SerializeField]
        public string m_FontName = "";
        public string FontName
        {
            get
            {
                return m_FontName;
            }
            set
            {
                if(m_FontName!=value)
                {
                    m_FontName = value;
                    TextChanged();
                }
            }
        }

        [SerializeField]
        public string m_Text = "";
        public string Text
        {
            get 
            {
                return m_Text;
            }
            set
            {
                if(m_Text != value)
                {
                    m_Text = value;
                    TextChanged();
                }
            }
        }

        protected virtual void Awake()
        {
            TextChanged();
        }

        private void TextChanged()
        {
            if(FontData!=null && !string.IsNullOrEmpty(FontName) && !string.IsNullOrEmpty(Text))
            {
                OnTextChanged(FontData.GetText(FontName, Text));
            }else
            {
                OnTextChanged(string.Empty);
            }
        }

        protected virtual void OnTextChanged(string mappedText)
        {

        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            TextChanged();
        }
#endif
    }
}
