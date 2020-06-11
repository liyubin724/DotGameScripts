using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game.Core.BMFont
{
    [Serializable]
    public class BMFontMapData
    {
        public string[] orgChars = new string[0];
        public string[] mapChars = new string[0];
    }

    public class BMFontData : ScriptableObject
    {
        public Font bmFont = null;
        public string[] fontNames = new string[0];
        public BMFontMapData[] mapDatas = new BMFontMapData[0];

        private Dictionary<string, Dictionary<string, string>> orgToMapDic = null;
        private StringBuilder strBuilder =null;
        public string GetBMFontText(string fontName,string text)
        {
            if(orgToMapDic == null)
            {
                orgToMapDic = new Dictionary<string, Dictionary<string, string>>();
            }
            if(strBuilder == null)
            {
                strBuilder = new StringBuilder();
            }
            strBuilder.Length = 0;
            Dictionary<string, string> mapDic = null;
            if(!orgToMapDic.TryGetValue(fontName,out mapDic))
            {
                int index = Array.IndexOf(fontNames, fontName);
                if(index>=0)
                {
                    mapDic = new Dictionary<string, string>();
                    orgToMapDic.Add(fontName, mapDic);

                    BMFontMapData mapData = mapDatas[index];
                    for(int i =0;i<mapData.orgChars.Length;i++)
                    {
                        mapDic.Add(mapData.orgChars[i], mapData.mapChars[i]);
                    }
                }
            }
            if (orgToMapDic == null)
                return string.Empty;

            for(int i = 0;i<text.Length;i++)
            {
                string mapStr = "";
                if(mapDic.TryGetValue(text.Substring(i,1),out mapStr))
                {
                    strBuilder.Append(mapStr);
                }
            }
            return strBuilder.ToString();
        }
    }

    public class BMFontSetting : ScriptableObject
    {
        public string texturePath = "";
        public string fontPath = "";
        public string dataPath = "";

        public List<BMFontGather> gatherList = new List<BMFontGather>();
    }
    [Serializable]
    public class BMFontGather
    {
        public string name = "";
        public int characterSpace = 0;
        public List<BMFontChar> charList = new List<BMFontChar>();
    }
    [Serializable]
    public class BMFontChar
    {
        public string text;
        public Texture2D texture;
    }
}
