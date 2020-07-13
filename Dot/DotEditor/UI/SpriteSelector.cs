using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace DotEditor.UI
{
    public class SpriteSelector : ScriptableWizard
    {
        static public SpriteSelector instance;

        void OnEnable() { instance = this; }
        void OnDisable() { instance = null; }

        public delegate void Callback(string sprite);
        public delegate void CallbackPar(string sprite, params System.Object[] pars);

        SerializedObject mObject;
        SerializedProperty mProperty;

        Sprite mSprite;
        Vector2 mPos = Vector2.zero;
        Callback mCallback;
        CallbackPar mCallbackPar;
        float mClickTime = 0f;
        System.Object[] m_Params = null;

        static Texture2D mBackdropTex;
        /// <summary>
        /// Draw the custom wizard.
        /// </summary>
        static public Texture2D blankTexture
        {
            get
            {
                return EditorGUIUtility.whiteTexture;
            }
        }

        static public Texture2D backdropTexture
        {
            get
            {
                if (mBackdropTex == null) mBackdropTex = CreateCheckerTex(
                    new Color(0.1f, 0.1f, 0.1f, 0.5f),
                    new Color(0.2f, 0.2f, 0.2f, 0.5f));
                return mBackdropTex;
            }
        }


        static Texture2D CreateCheckerTex(Color c0, Color c1)
        {
            Texture2D tex = new Texture2D(16, 16);
            tex.name = "[Generated] Checker Texture";
            tex.hideFlags = HideFlags.DontSave;

            for (int y = 0; y < 8; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c1);
            for (int y = 8; y < 16; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c0);
            for (int y = 0; y < 8; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c0);
            for (int y = 8; y < 16; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c1);

            tex.Apply();
            tex.filterMode = FilterMode.Point;
            return tex;
        }

        static public void DrawOutline(Rect rect, Color color)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Texture2D tex = blankTexture;
                GUI.color = color;
                GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, 1f, rect.height), tex);
                GUI.DrawTexture(new Rect(rect.xMax, rect.yMin, 1f, rect.height), tex);
                GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, 1f), tex);
                GUI.DrawTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1f), tex);
                GUI.color = Color.white;
            }
        }

        void OnGUI()
        {
            EditorGUIUtility.labelWidth = 80f;

            if (UISetting.atlas == null)
            {
                GUILayout.Label("No Atlas selected.");
            }
            else
            {
                SpriteAtlas atlas = UISetting.atlas;
                bool close = false;
                GUILayout.Label(atlas.name + " Sprites", "LODLevelNotifyText");
                GUILayout.Space(12f);

                if (Event.current.type == EventType.Repaint)
                {
                    GUI.color = new Color(0f, 0f, 0f, 0.25f);
                    GUI.DrawTexture(new Rect(0f, GUILayoutUtility.GetLastRect().yMin + 6f, Screen.width, 4f), blankTexture);
                    GUI.DrawTexture(new Rect(0f, GUILayoutUtility.GetLastRect().yMin + 6f, Screen.width, 1f), blankTexture);
                    GUI.DrawTexture(new Rect(0f, GUILayoutUtility.GetLastRect().yMin + 9f, Screen.width, 1f), blankTexture);
                    GUI.color = Color.white;
                }

                GUILayout.BeginHorizontal();
                GUILayout.Space(84f);

                string before = UISetting.partialSprite;
                string after = EditorGUILayout.TextField("", before, "SearchTextField");
                if (before != after) UISetting.partialSprite = after;

                if (GUILayout.Button("", "SearchCancelButton", GUILayout.Width(18f)))
                {
                    UISetting.partialSprite = "";
                    GUIUtility.keyboardControl = 0;
                }
                GUILayout.Space(84f);
                GUILayout.EndHorizontal();

                Sprite[] sprites = new Sprite[atlas.spriteCount];
                atlas.GetSprites(sprites);
                float size = 80f;
                float padded = size + 10f;
                int columns = Mathf.FloorToInt(position.width / padded);
                if (columns < 1) columns = 1;

                int offset = 0;
                Rect rect = new Rect(10f, 0, size, size);

                GUILayout.Space(10f);
                mPos = GUILayout.BeginScrollView(mPos);
                int rows = 1;
                while (offset < sprites.Length)
                {
                    GUILayout.BeginHorizontal();
                    {
                        int col = 0;
                        rect.x = 10f;

                        for (; offset < sprites.Length; ++offset)
                        {
                            Sprite sprite = sprites[offset];
                            if (sprite == null) continue;

                            // Button comes first
                            if (GUI.Button(rect, ""))
                            {
                                if (Event.current.button == 0)
                                {
                                    float delta = Time.realtimeSinceStartup - mClickTime;
                                    mClickTime = Time.realtimeSinceStartup;
                                    string spriteName = sprite.name.Replace("(Clone)", "");
                                    if (UISetting.selectedSprite != spriteName)
                                    {
                                        if (mSprite != null)
                                        {
                                            RegisterUndo("Atlas Selection", mSprite);
                                            //mSprite.MakePixelPerfect();
                                            EditorUtility.SetDirty(mSprite);
                                        }

                                        UISetting.selectedSprite = spriteName;
                                        Repaint();
                                        if (mCallback != null) mCallback(spriteName);

                                        if (mCallbackPar != null)
                                        {
                                            mCallbackPar(spriteName, m_Params);
                                        }
                                    }
                                    else if (delta < 0.5f) close = true;
                                }
                                else
                                {
                                    //NGUIContextMenu.AddItem("Edit", false, EditSprite, sprite);
                                    //NGUIContextMenu.AddItem("Delete", false, DeleteSprite, sprite);
                                    //NGUIContextMenu.Show();
                                }
                            }

                            if (Event.current.type == EventType.Repaint)
                            {
                                // On top of the button we have a checkboard grid
                                DrawTiledTexture(rect, backdropTexture);
                                Rect uv = new Rect(sprite.rect.x, sprite.rect.y, sprite.rect.width, sprite.rect.height);
                                uv = ConvertToTexCoords(uv, (int)sprite.texture.width, (int)sprite.texture.height);

                                // Calculate the texture's scale that's needed to display the sprite in the clipped area
                                float scaleX = rect.width / uv.width;
                                float scaleY = rect.height / uv.height;

                                // Stretch the sprite so that it will appear proper
                                float aspect = (scaleY / scaleX) / ((float)sprite.texture.height / sprite.texture.width);
                                Rect clipRect = rect;

                                if (aspect != 1f)
                                {
                                    if (aspect < 1f)
                                    {
                                        // The sprite is taller than it is wider
                                        float padding = size * (1f - aspect) * 0.5f;
                                        clipRect.xMin += padding;
                                        clipRect.xMax -= padding;
                                    }
                                    else
                                    {
                                        // The sprite is wider than it is taller
                                        float padding = size * (1f - 1f / aspect) * 0.5f;
                                        clipRect.yMin += padding;
                                        clipRect.yMax -= padding;
                                    }
                                }
                                SpriteDrawUtility.DrawSprite(sprite, clipRect, GUI.color);
                                // Draw the selection
                                if (UISetting.selectedSprite == sprite.name)
                                {
                                    DrawOutline(rect, new Color(0.4f, 1f, 0f, 1f));
                                }
                            }

                            GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);
                            GUI.contentColor = new Color(1f, 1f, 1f, 0.7f);
                            GUI.Label(new Rect(rect.x, rect.y + rect.height, rect.width, 32f), sprite.name, "ProgressBarBack");
                            GUI.contentColor = Color.white;
                            GUI.backgroundColor = Color.white;
                            col++;
                            if (col >= columns)
                            {
                                ++offset;
                                break;
                            }
                            rect.x += padded;
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(padded);
                    rect.y += padded + 26;
                    ++rows;
                }
                GUILayout.Space(rows * 26);
                GUILayout.EndScrollView();

                if (close) Close();
            }
        }

        void OnSpriteSelection(string sp)
        {
            if (mObject != null && mProperty != null)
            {
                mObject.Update();
                mProperty.stringValue = sp;
                mObject.ApplyModifiedProperties();
            }
        }

        /// <summary>
        /// Show the sprite selection wizard.
        /// </summary>

        static public void ShowSelected()
        {
            if (UISetting.atlas != null)
            {
                Show(delegate (string sel) { SelectSprite(sel); });
            }
        }

        static public void SelectSprite(string spriteName)
        {
            if (UISetting.atlas != null)
            {
                UISetting.selectedSprite = spriteName;
                //NGUIEditorTools.Select(NGUISettings.atlas.gameObject);
                //RepaintSprites();
                if (SpriteSelector.instance != null)
                    SpriteSelector.instance.Repaint();
            }
        }

        /// <summary>
        /// Show the sprite selection wizard.
        /// </summary>

        static public void Show(SerializedObject ob, SerializedProperty pro, SpriteAtlas atlas)
        {
            if (instance != null)
            {
                instance.Close();
                instance = null;
            }

            if (ob != null && pro != null && atlas != null)
            {
                SpriteSelector comp = ScriptableWizard.DisplayWizard<SpriteSelector>("Select a Sprite");
                UISetting.atlas = atlas;
                UISetting.selectedSprite = pro.hasMultipleDifferentValues ? null : pro.stringValue;
                comp.mSprite = null;
                comp.mObject = ob;
                comp.mProperty = pro;
                comp.mCallback = comp.OnSpriteSelection;
            }
        }

        /// <summary>
        /// Show the selection wizard.
        /// </summary>

        static public void Show(Callback callback, CallbackPar callbackPar = null, params System.Object[] pars)
        {
            if (instance != null)
            {
                instance.Close();
                instance = null;
            }

            SpriteSelector comp = ScriptableWizard.DisplayWizard<SpriteSelector>("Select a Sprite");
            comp.mSprite = null;
            comp.mCallback = callback;
            comp.mCallbackPar = callbackPar;
            comp.m_Params = pars;
        }

        static public void RegisterUndo(string name, params Object[] objects)
        {
            if (objects != null && objects.Length > 0)
            {
                UnityEditor.Undo.RecordObjects(objects, name);

                foreach (Object obj in objects)
                {
                    if (obj == null) continue;
                    EditorUtility.SetDirty(obj);
                }
            }
        }

        static public void DrawTiledTexture(Rect rect, Texture tex)
        {
            GUI.BeginGroup(rect);
            {
                int width = Mathf.RoundToInt(rect.width);
                int height = Mathf.RoundToInt(rect.height);

                for (int y = 0; y < height; y += tex.height)
                {
                    for (int x = 0; x < width; x += tex.width)
                    {
                        GUI.DrawTexture(new Rect(x, y, tex.width, tex.height), tex);
                    }
                }
            }
            GUI.EndGroup();
        }

        static public Rect ConvertToTexCoords(Rect rect, int width, int height)
        {
            Rect final = rect;

            if (width != 0f && height != 0f)
            {
                final.xMin = rect.xMin / width;
                final.xMax = rect.xMax / width;
                final.yMin = 1f - rect.yMax / height;
                final.yMax = 1f - rect.yMin / height;
            }
            return final;
        }
    }
}
