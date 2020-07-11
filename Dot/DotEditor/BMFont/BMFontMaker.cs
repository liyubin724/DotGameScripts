using DotEditor.GUIExtension;
using DotEditor.GUIExtension.ListView;
using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BMFont
{
    public class BMFontMaker : EditorWindow
    {
        private static string MATERIAL_SHADER = "Unlit/Transparent";

        private static float TOOLBAR_HEIGHT = 20;

        private static float LISTVIEW_DRAG_WIDTH = 5;
        private static float MIN_LISTVIEW_WIDTH = 100;
        private static float MAX_LISTVIEW_WIDTH = 300;

        [MenuItem("Game/BM Font/Font Maker")]
        public static void ShowWin()
        {
            var win = GetWindow<BMFontMaker>();
            win.titleContent = Contents.titleContent;
            win.Show();
        }

        private List<BMFontMakerData> makerDatas = new List<BMFontMakerData>();
        private SimpleListView<BMFontMakerData> makerDataListView = null;

        private float listViewWidth = 120;
        
        void Awake()
        {
            makerDatas.AddRange(AssetDatabaseUtility.FindInstances<BMFontMakerData>());
        }

        void OnGUI()
        {
            Rect toolbarRect = new Rect(0, 0, position.width, TOOLBAR_HEIGHT);
            DrawToolbar(toolbarRect);


            Rect listViewRect = new Rect(0, TOOLBAR_HEIGHT, listViewWidth, position.height - TOOLBAR_HEIGHT);
            DrawListView(listViewRect);

            EGUI.DrawAreaLine(listViewRect, Color.grey);

            Rect listViewDragRect = new Rect(listViewRect.x + listViewRect.width, listViewRect.y, LISTVIEW_DRAG_WIDTH, listViewRect.height);
            DrawListViewDrag(listViewDragRect);

            EGUI.DrawAreaLine(listViewDragRect, Color.grey);

            Rect contentRect = new Rect(listViewDragRect.x + listViewDragRect.width, listViewRect.y, position.width - listViewDragRect.x - listViewDragRect.width, listViewRect.height);
            DrawContent(contentRect);

            EGUI.DrawAreaLine(contentRect, Color.grey);
        }

        private void DrawToolbar(Rect rect)
        {
            EditorGUI.LabelField(rect, GUIContent.none,EditorStyles.toolbar);
            
            Rect createRect = rect;
            createRect.width = Styles.toolbarWidth;
            if(GUI.Button(createRect,Contents.creatContent, EditorStyles.toolbarButton))
            {

            }

            Rect deleteRect = createRect;
            deleteRect.x += createRect.width;
            if(GUI.Button(deleteRect,Contents.deleteContent, EditorStyles.toolbarButton))
            {

            }

            Rect exportRect = deleteRect;
            exportRect.x += deleteRect.width;
            if(GUI.Button(exportRect,Contents.exportContent, EditorStyles.toolbarButton))
            {

            }

            Rect exportAllRect = rect;
            exportAllRect.x += rect.width - Styles.toolbarWidth;
            exportAllRect.width = Styles.toolbarWidth;
            if(GUI.Button(exportAllRect,Contents.exportAllContent, EditorStyles.toolbarButton))
            {

            }
        }

        private void DrawListView(Rect rect)
        {
            if(makerDataListView == null)
            {
                makerDataListView = new SimpleListView<BMFontMakerData>();
                makerDataListView.AddItems(makerDatas.ToArray());
            }

            makerDataListView.OnGUI(rect);
        }

        private bool isListViewDragging = false;
        private void DrawListViewDrag(Rect dragRect)
        {
            EGUI.DrawVerticalLine(dragRect, Color.grey, 2.0f);

            EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.ResizeHorizontal);
            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && dragRect.Contains(Event.current.mousePosition))
                {
                    isListViewDragging = true;

                    Event.current.Use();
                    Repaint();
                }
                else if (isListViewDragging && Event.current.type == EventType.MouseDrag)
                {
                    listViewWidth += Event.current.delta.x;
                    if (listViewWidth < MIN_LISTVIEW_WIDTH)
                    {
                        listViewWidth = MIN_LISTVIEW_WIDTH;
                    }
                    else if (listViewWidth > MAX_LISTVIEW_WIDTH)
                    {
                        listViewWidth = MAX_LISTVIEW_WIDTH;
                    }
                    Repaint();
                }
                else if (isListViewDragging && Event.current.type == EventType.MouseUp)
                {
                    isListViewDragging = false;
                    Event.current.Use();
                    Repaint();
                }
            }
        }

        private void DrawContent(Rect rect)
        {

        }

        private Texture2D PackFontCharTexture(string texturePath,Texture2D[] textures, out Rect[] rects)
        {
            Texture2D fontAtlas = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            rects = null;
            if (textures != null && textures.Length > 0)
            {
                rects = fontAtlas.PackTextures(textures.ToArray(), 2, 1024);
            }
            string fontAtlasDiskPath = PathUtility.GetDiskPath(texturePath);
            File.WriteAllBytes(fontAtlasDiskPath, fontAtlas.EncodeToPNG());
            AssetDatabase.ImportAsset(texturePath);

            TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(texturePath);
            TextureImporterSettings tis = new TextureImporterSettings();
            ti.ReadTextureSettings(tis);
            tis.textureType = TextureImporterType.Default;
            tis.textureShape = TextureImporterShape.Texture2D;
            tis.sRGBTexture = true;
            tis.alphaSource = TextureImporterAlphaSource.FromInput;
            tis.alphaIsTransparency = true;
            tis.mipmapEnabled = false;
            tis.readable = false;
            tis.npotScale = TextureImporterNPOTScale.None;
            ti.SetTextureSettings(tis);

            TextureImporterPlatformSettings ps = ti.GetPlatformTextureSettings("Standalone");
            ps.overridden = true;
            ps.format = TextureImporterFormat.RGBA32;
            ti.SetPlatformTextureSettings(ps);

            ps = ti.GetPlatformTextureSettings("Android");
            ps.overridden = true;
            ps.format = TextureImporterFormat.ETC2_RGBA8;
            ti.SetPlatformTextureSettings(ps);

            ps = ti.GetPlatformTextureSettings("iPhone");
            ps.overridden = true;
            ps.format = TextureImporterFormat.ASTC_RGBA_4x4;
            ti.SetPlatformTextureSettings(ps);

            AssetDatabase.WriteImportSettingsIfDirty(texturePath);

            fontAtlas = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            return fontAtlas;
        }

        class Contents
        {
            public static GUIContent titleContent = new GUIContent("Font Maker");
            public static GUIContent creatContent = new GUIContent("Create", "create a new data");
            public static GUIContent deleteContent = new GUIContent("Delete", "delete the data which be selected");
            public static GUIContent exportContent = new GUIContent("Export", "export the data");
            public static GUIContent exportAllContent = new GUIContent("Export All", "export all");
        }

        class Styles
        {
            public static float toolbarWidth = 60;
        }
    }
}
