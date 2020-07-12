using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DotEngine.BMFont
{
    public class BMFontData : ScriptableObject
    {
        public Font bmFont = null;
        public FontCharMap[] charMaps = new FontCharMap[0];

        private Dictionary<string, FontCharMap> charMapDic = null;
        private StringBuilder textSB = new StringBuilder();

        public string GetText(string fontName,string text)
        {
            if(string.IsNullOrEmpty(fontName) || string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if(charMapDic == null)
            {
                charMapDic = new Dictionary<string, FontCharMap>();
                for(int i =0;i<charMaps.Length;++i)
                {
                    charMapDic.Add(charMaps[i].name, charMaps[i]);
                }
            }

            if(!charMapDic.TryGetValue(fontName,out FontCharMap charMap))
            {
                return string.Empty;
            }
            textSB.Clear();
            for(int i =0;i<text.Length;++i)
            {
                textSB.Append(charMap.GetChar(text[i]));
            }
            return textSB.ToString();
        }

        [Serializable]
        public class FontCharMap
        {
            public string name = string.Empty;
            public char[] orgChars = new char[0];
            public char[] mapChars = new char[0];

            private Dictionary<char, char> charDic = null;
            public char GetChar(char c)
            {
                if(charDic == null)
                {
                    charDic = new Dictionary<char, char>();
                    for(int i =0;i<orgChars.Length;++i)
                    {
                        charDic.Add(orgChars[i], mapChars[i]);
                    }
                }
                if (charDic.TryGetValue(c, out char result))
                {
                    return result;
                }
                return ' ';
            }
        }
    }
}
