using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DotEditor.BMFont
{
    public class BMFontConfig : ScriptableObject
    {
        public string fontOutputDir = string.Empty;
        public int padding = 2;
        public int maxSize = 1024;
        public List<BMFontChar> fonts = new List<BMFontChar>();

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

        public bool IsValid()
        {
            if(string.IsNullOrEmpty(fontOutputDir) || !fontOutputDir.StartsWith("Assets"))
            {
                return false;
            }

            if(fonts == null ||fonts.Count ==0)
            {
                return false;
            }

            foreach(var fontChar in fonts)
            {
                if(!fontChar.IsValid())
                {
                    return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            return name;
        }

        [Serializable]
        public class BMFontChar
        {
            public string fontName = string.Empty;
            public int charSpace = 0;
            
            public List<char> chars = new List<char>();
            public List<Texture2D> textures = new List<Texture2D>();

            [NonSerialized]
            public int[] charIndexes = null;
            [NonSerialized]
            public Rect[] charRects = null;

            public bool IsValid()
            {
                if(string.IsNullOrEmpty(fontName))
                {
                    return false;
                }
                if(chars == null || chars.Count == 0)
                {
                    return false;
                }
                if(textures == null || textures.Count == 0)
                {
                    return false;
                }
                if(chars.Count!=textures.Count)
                {
                    return false;
                }

                bool isRepeat = chars.GroupBy((c) => c).Where((g)=>g.Count()>1).Count()>0 ;
                if(isRepeat)
                {
                    return false;
                }
                bool isNull = textures.Any((texture) => texture == null);
                if(isNull)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
