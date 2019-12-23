using Dot.UI;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace DotEditor.UI
{
    public static class UIExtensionDefaultControls
    {
        public struct Resources
        {
            public Sprite standard;
            public Sprite background;
            public Sprite inputField;
            public Sprite knob;
            public Sprite checkmark;
            public Sprite dropdown;
            public Sprite mask;
        }

        private const float  kWidth       = 160f;
        private const float  kThickHeight = 30f;
        private const float  kThinHeight  = 20f;
        private static Vector2 s_ThickElementSize       = new Vector2(kWidth, kThickHeight);
        private static Vector2 s_ThinElementSize        = new Vector2(kWidth, kThinHeight);
        private static Vector2 s_ImageElementSize       = new Vector2(100f, 100f);
        private static Color   s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
        private static Color   s_PanelColor             = new Color(1f, 1f, 1f, 0.392f);
        private static Color   s_TextColor              = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        // Helper methods at top

        private static GameObject CreateUIElementRoot(string name, Vector2 size)
        {
            GameObject child = new GameObject(name);
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }

        static GameObject CreateUIObject(string name, GameObject parent)
        {
            GameObject go = new GameObject(name);
            go.AddComponent<RectTransform>();
            SetParentAndAlign(go, parent);
            return go;
        }

        private static void SetDefaultTextValues(Text lbl)
        {
            lbl.color = s_TextColor;

            System.Type textType = lbl.GetType();
            MethodInfo mi = textType.GetMethod("AssignDefaultFont", BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(lbl, new System.Object[] { });
        }

        private static void SetDefaultColorTransitionValues(Selectable slider)
        {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor     = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor    = new Color(0.521f, 0.521f, 0.521f);
        }

        private static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform, false);
            SetLayerRecursively(child, parent.layer);
        }

        private static void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++)
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
        }
        
        public static GameObject CreateAtlasImage(Resources resources)
        {
            GameObject go = CreateUIElementRoot("AtlasImage", s_ImageElementSize);
            go.AddComponent<SpriteAtlasImage>();
            return go;
        }

        public static GameObject CreateDynamicAtlasImage(Resources resources)
        {
            GameObject go = CreateUIElementRoot("DynamicAtlasImage", s_ImageElementSize);
            go.AddComponent<DynamicAtlasImage>();
            return go;
        }

        public static GameObject CreateAtlasImageAnimation(Resources resources)
        {
            GameObject go = CreateUIElementRoot("AtlasImageAnimation", s_ImageElementSize);
            go.AddComponent<SpriteAtlasImageAnimation>();
            return go;
        }

        public static GameObject CreateLuaButton(Resources resources)
        {
            GameObject buttonRoot = CreateUIElementRoot("LuaButton", s_ThickElementSize);

            GameObject childText = new GameObject("Text");
            childText.AddComponent<RectTransform>();
            SetParentAndAlign(childText, buttonRoot);

            Image image = buttonRoot.AddComponent<Image>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;

            LuaUIButton bt = buttonRoot.AddComponent<LuaUIButton>();
            SetDefaultColorTransitionValues(bt);

            Text text = childText.AddComponent<Text>();
            text.text = "Lua Button";
            text.alignment = TextAnchor.MiddleCenter;
            SetDefaultTextValues(text);

            RectTransform textRectTransform = childText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;

            return buttonRoot;
        }

    }
}
