using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.BMFont
{
    public class BMFontMakerData : ScriptableObject
    {
        public string fontOutputDir = string.Empty;
        public int padding = 2;
        public List<BMFontGatherData> gatherDatas = new List<BMFontGatherData>();

        public string GetFontPath()
        {
            return $"{fontOutputDir}/{name}_bmf_font.asset";
        }

        public string GetFontDataPath()
        {
            return $"{fontOutputDir}/{name}_bmf_data.asset";
        }

        public string GetFontTexturePath()
        {
            return $"{fontOutputDir}/{name}_bmf_tex.png";
        }

        public override string ToString()
        {
            return name;
        }

        [Serializable]
        public class BMFontGatherData
        {
            public string fontName = string.Empty;
            public List<BMFontCharData> charDatas = new List<BMFontCharData>();
        }

        [Serializable]
        public class BMFontCharData
        {
            public char charText;
            public Texture2D texture2D;
        }
    }
}
