using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DotEngine.BMFont
{
    public class BMFontData : ScriptableObject
    {
        public Font bmFont = null;

        public string[] fontNames = new string[0];
        public CharMap[] chars = new CharMap[0];

        private Dictionary<string, Dictionary<char, char>> fontCharMapDic = null;
        private StringBuilder mappedSB = new StringBuilder();
        public string GetText(string fontName,string text)
        {
            if(string.IsNullOrEmpty(fontName) || string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if(fontCharMapDic == null)
            {
                fontCharMapDic = new Dictionary<string, Dictionary<char, char>>();
                for(int i =0;i<fontNames.Length;++i)
                {
                    Dictionary<char, char> dic = new Dictionary<char, char>();
                    fontCharMapDic.Add(fontNames[i], dic);

                    CharMap cmData = chars[i];
                    for(int j = 0;j<cmData.orgChar.Length;++j)
                    {
                        dic.Add(cmData.orgChar[j], cmData.mapChar[j]);
                    }
                }
            }

            if(!fontCharMapDic.TryGetValue(fontName,out Dictionary<char, char> charMapDic))
            {
                return string.Empty;
            }
            mappedSB.Clear();
            for(int i = 0;i<text.Length;++i)
            {
                if(charMapDic.TryGetValue(text[i],out char value))
                {
                    mappedSB.Append(value);
                }else
                {
                    mappedSB.Append(' ');
                }
            }
            return mappedSB.ToString();
        }

        [Serializable]
        public class CharMap
        {
            public char[] orgChar = new char[0];
            public char[] mapChar = new char[0];
        }
    }
}
