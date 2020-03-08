using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI
{
    public static class DotEditorResorces
    {
        private static Color32 gridLineColor = new Color(0.45f, 0.45f, 0.45f);
        private static Color32 gridBgColor = new Color(0.18f, 0.18f, 0.18f);

        private static Texture2D gridTexture = null;
        public static Texture2D GridTexture
        {
            get
            {
                if(gridTexture == null)
                {
                    gridTexture = GenerateGridTexture(gridLineColor, gridBgColor);
                }
                return gridTexture;
            }
        }

        private static Texture2D crossTexture = null;
        public static Texture2D CrossTexture
        {
            get
            {
                if(crossTexture == null)
                {
                    crossTexture = GenerateCrossTexture(gridLineColor);
                }
                return crossTexture;
            }
        }

        public static Texture2D GenerateGridTexture(Color line, Color bg)
        {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = bg;
                    if (y % 16 == 0 || x % 16 == 0) col = Color.Lerp(line, bg, 0.65f);
                    if (y == 63 || x == 63) col = Color.Lerp(line, bg, 0.35f);
                    cols[(y * 64) + x] = col;
                }
            }
            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Repeat;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Grid";
            tex.Apply();
            return tex;
        }

        public static Texture2D GenerateCrossTexture(Color line)
        {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = line;
                    if (y != 31 && x != 31) col.a = 0;
                    cols[(y * 64) + x] = col;
                }
            }
            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Grid";
            tex.Apply();
            return tex;
        }

        private static Texture2D scriptIconTexture = null;
        public static Texture2D ScriptIconTexture
        {
            get
            {
                if(scriptIconTexture == null)
                {
                    scriptIconTexture = (EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D);
                }
                return scriptIconTexture;
            }
        }
    }
}
