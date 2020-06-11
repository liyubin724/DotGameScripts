using Game.Core.BMFont;
using Game.Core.Util;
using GameEditor.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GameEditor.Core.BMFont
{
    public class BMFontCreaterWin : EditorWindow
    {   
        private static string BMFont_Texture_Path = "Assets/Resources/Fonts/{0}_bmf_tex.png";
        private static string BMFont_Data_Path = "Assets/Resources/Fonts/{0}_bmf_data.asset";
        private static string BMFont_Path = "Assets/Resources/Fonts/{0}_bmf_font.fontsettings";
        private static string BMFont_Setting_Path = "Assets/Tools/BmFont/{0}_bmf_setting.asset";
        private static string BMFont_Setting_Dir = "Assets/Tools/BmFont";

        private static string BMFont_Material_Shader = "Unlit/Transparent";

        private int charIDStart = 19968;//19968-40869/33-127

        [MenuItem("Game/BM Font/Window")]
        private static void ShowWindow()
        {
            BMFontCreaterWin win = EditorWindow.GetWindow<BMFontCreaterWin>();
            win.titleContent = new GUIContent("BMFont Window");
            win.Show();
        }
        
        private Vector2 scrollPos = Vector2.zero;

        GUIStyle boldTextStyle = null;
        
        private bool isCreating = false;
        private string bmFontName = "";
        string bmFontPath = "";
        string bmTexPath = "";
        string bmDataPath = "";
        string bmSettingPath = "";

        private BMFontSetting bmFontSetting = null;
        private int gatherDeleteIndex = -1;


        void OnGUI()
        {
            if(boldTextStyle == null)
            {
                boldTextStyle = new GUIStyle(EditorStyles.boldLabel);
                boldTextStyle.alignment = TextAnchor.MiddleCenter;
                boldTextStyle.fontSize = 26;
            }

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                {
                    if(GUILayout.Button("Create",EditorStyles.toolbarButton))
                    {
                        isCreating = true;
                    }
                    if(GUILayout.Button("Load",EditorStyles.toolbarDropDown))
                    {
                        string settingDir = PathUtil.GetDiskPath(BMFont_Setting_Dir);
                        string[] files = Directory.GetFiles(settingDir, "*.asset", SearchOption.TopDirectoryOnly);
                        GenericMenu gMenu = new GenericMenu();
                        foreach(string f in files)
                        {
                            string assetPath = PathUtil.GetAssetPath(f);
                            BMFontSetting setting = AssetDatabase.LoadAssetAtPath<BMFontSetting>(assetPath);
                            if ( setting != null)
                            {
                                gMenu.AddItem(new GUIContent(Path.GetFileNameWithoutExtension(f)), false, (userData) =>
                                {
                                    isCreating = false;
                                    bmFontSetting = (BMFontSetting)userData;
                                },setting);
                            }
                        }
                        gMenu.ShowAsContext();
                    }
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField("BM Font Tool", boldTextStyle,GUILayout.Height(30));
                if(isCreating)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        bmFontName = EditorGUILayout.TextField("BM Font Name:", bmFontName);
                        
                        if(!string.IsNullOrEmpty(bmFontName))
                        {
                            bmFontPath = string.Format(BMFont_Path, bmFontName);
                            bmTexPath = string.Format(BMFont_Texture_Path, bmFontName);
                            bmDataPath = string.Format(BMFont_Data_Path, bmFontName);
                            bmSettingPath = string.Format(BMFont_Setting_Path, bmFontName);
                        }
                        else
                        {
                            EditorGUILayout.HelpBox("Plz input the name of BMFont", MessageType.Info);
                            bmFontPath = "";
                            bmTexPath = "";
                            bmDataPath = "";
                            bmSettingPath = "";
                        }

                        EditorGUILayout.LabelField("Font Path:", bmFontPath, EditorStyles.textField);
                        EditorGUILayout.LabelField("Texture Path:", bmTexPath, EditorStyles.textField);
                        EditorGUILayout.LabelField("Data Path:", bmDataPath, EditorStyles.textField);
                        EditorGUILayout.LabelField("Setting Path:", bmSettingPath, EditorStyles.textField);

                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.FlexibleSpace();
                            if(GUILayout.Button("Create",GUILayout.Width(80)))
                            {
                                isCreating = false;
                                bmFontSetting = AssetDatabase.LoadAssetAtPath<BMFontSetting>(bmSettingPath);
                                if(bmFontSetting == null)
                                {
                                    bmFontSetting = BMFontSetting.CreateInstance<BMFontSetting>();
                                    AssetDatabase.CreateAsset(bmFontSetting, bmSettingPath);
                                }
                                bmFontSetting.dataPath = bmDataPath;
                                bmFontSetting.texturePath = bmTexPath;
                                bmFontSetting.fontPath = bmFontPath;

                                EditorUtility.SetDirty(bmFontSetting);
                            }
                            EditorGUILayout.Space();
                            if(GUILayout.Button("Cancel",GUILayout.Width(80)))
                            {
                                isCreating = false;
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                }else if(bmFontSetting !=null)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUILayout.LabelField("Setting:", EditorStyles.boldLabel);
                        EditorGUILayout.LabelField("Data Path:",bmFontSetting.dataPath, EditorStyles.boldLabel);
                        EditorGUILayout.LabelField("Font Path:",bmFontSetting.fontPath, EditorStyles.boldLabel);
                        EditorGUILayout.LabelField("Texture Path:",bmFontSetting.texturePath, EditorStyles.boldLabel);
                    }
                    EditorGUILayout.EndVertical();

                    if (gatherDeleteIndex >= 0)
                    {
                        bmFontSetting.gatherList.RemoveAt(gatherDeleteIndex);
                        gatherDeleteIndex = -1;
                    }

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Font:", EditorStyles.boldLabel);
                            if(GUILayout.Button("Add New Font"))
                            {
                                if(bmFontSetting.gatherList.Count>0 && bmFontSetting.gatherList[0].name == "New Font")
                                {
                                    EditorUtility.DisplayDialog("Warning", "New Font has Been added ","OK");
                                }else
                                {
                                    BMFontGather newGather = new BMFontGather();
                                    newGather.name = "New Font";
                                    bmFontSetting.gatherList.Insert(0, newGather);
                                }
                                scrollPos = Vector2.zero;
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                        {
                            
                            for(int i=0,c = bmFontSetting.gatherList.Count;i<c;i++)
                            {
                                if(DrawBMFontGather(bmFontSetting.gatherList[i]))
                                {
                                    gatherDeleteIndex = i;
                                }
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndVertical();
                }
                if(bmFontSetting!=null)
                {
                    if(GUILayout.Button("Create BM Font"))
                    {
                        if(IsGatherNameRepeat())
                        {
                            EditorUtility.DisplayDialog("Error", "The name of font is Repeated","OK");
                        }else if(IsCharRepeatInGather())
                        {
                            EditorUtility.DisplayDialog("Error", "The char which in gather is Repeated", "OK");
                        }else
                        {
                            ExportBMFont();
                        }
                    }
                }
            }
            EditorGUILayout.EndVertical();
            
            if(GUI.changed)
            {
                EditorUtility.SetDirty(bmFontSetting);
            }
        }

        private void ExportBMFont()
        {
            Texture2D[] fontCharTextures = GetPackedTextures(out string[] fontCharTexPaths);
            Texture2D fontAtlas = PackFontCharTexture(fontCharTextures, out Rect[] rects);

            Dictionary<string, Dictionary<string, int>> mapDataDic = new Dictionary<string, Dictionary<string, int>>();
            Font bmFont = CreateBMFont(fontAtlas, fontCharTexPaths, rects, ref mapDataDic);

            BMFontData fontData = AssetDatabase.LoadAssetAtPath<BMFontData>(bmFontSetting.dataPath);
            if (fontData == null)
            {
                fontData = BMFontData.CreateInstance<BMFontData>();
                AssetDatabase.CreateAsset(fontData, bmFontSetting.dataPath);
                AssetDatabase.ImportAsset(bmFontSetting.dataPath);

                fontData = AssetDatabase.LoadAssetAtPath<BMFontData>(bmFontSetting.dataPath);
            }
            fontData.bmFont = bmFont;
            fontData.fontNames = mapDataDic.Keys.ToArray();
            fontData.mapDatas = new BMFontMapData[fontData.fontNames.Length];
            for(int i =0;i<fontData.fontNames.Length;i++)
            {
                string fNames = fontData.fontNames[i];
                Dictionary<string, int> md = mapDataDic[fNames];
                BMFontMapData data = new BMFontMapData();
                data.orgChars = md.Keys.ToArray() ;
                data.mapChars = new string[data.orgChars.Length];
                for(int j =0;j<data.orgChars.Length;j++)
                {
                    data.mapChars[j] = UnicodeUtil.UnicodeToString(UnicodeUtil.IntToUnicode(md[data.orgChars[j]]));
                }
                fontData.mapDatas[i] = data;
            }
            EditorUtility.SetDirty(fontData);
            AssetDatabase.SaveAssets();

            Selection.activeObject = fontData;
        }

        private Dictionary<string, ReorderableList> gatherDrawerDic = new Dictionary<string, ReorderableList>();
        private bool DrawBMFontGather(BMFontGather gather)
        {
            bool isDeleted = false;
            ReorderableList list = null;
            if(!gatherDrawerDic.TryGetValue(gather.name,out list))
            {
                list = new ReorderableList(gather.charList, typeof(BMFontChar));
                list.displayAdd = true;
                list.displayRemove = true;
                list.draggable = true;
                list.elementHeight = 55;

                list.onRemoveCallback = (rl)=>
                {
                    gather.charList.RemoveAt(rl.index);
                };
                list.onAddCallback = (rl) =>
                {
                    BMFontChar fontChar = new BMFontChar();
                    rl.list.Add(fontChar);
                };
                list.drawElementCallback = (rect,index,isActive,isFocused)=> {
                    BMFontChar fontChar = gather.charList[index];
                    Rect drawRect = rect;
                    drawRect.width = 200;
                    drawRect.height = EditorGUIUtility.singleLineHeight;
                    fontChar.text = EditorGUI.TextField(drawRect,"Font Char:", fontChar.text);
                    if(!string.IsNullOrEmpty(fontChar.text))
                    {
                        fontChar.text = fontChar.text.Substring(0, 1);
                    }
                    drawRect.x += drawRect.width;
                    drawRect.height = 50;
                    fontChar.texture = (Texture2D)EditorGUI.ObjectField(drawRect, "Texture:", fontChar.texture, typeof(Texture2D), false);
                    
                };
                list.drawHeaderCallback = (rect)=> {

                };

                gatherDrawerDic.Add(gather.name, list);
            }
            gather.name = EditorGUILayout.TextField(gather.name);
            gather.characterSpace = EditorGUILayout.IntField("Character Space:", gather.characterSpace);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    list.DoLayoutList();
                }
                EditorGUILayout.EndVertical();
                if(GUILayout.Button("Delete",GUILayout.Width(50),GUILayout.Height(40)))
                {
                    isDeleted = true;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            return isDeleted;
        }

        private bool IsCharRepeatInGather()
        {
            bool isRepeat = false;
            bmFontSetting.gatherList.ForEach((gather) => 
            {
                if(isRepeat)
                {
                    return;
                }
                List<string> names = new List<string>();
                gather.charList.ForEach((fChar) =>
                {
                    if (names.IndexOf(fChar.text) >= 0)
                    {
                        isRepeat = true;
                    }else
                    {
                        names.Add(fChar.text);
                    }
                });
            });

            return isRepeat;
        }

        private bool IsGatherNameRepeat()
        {
            List<string> names = new List<string>();
            bool isRepeat = false;
            bmFontSetting.gatherList.ForEach((gather) =>
            {
                if(isRepeat)
                {
                    return;
                }
                if(names.IndexOf(gather.name)>=0)
                {
                    isRepeat = true;
                }else
                {
                    names.Add(gather.name);
                }
            });

            return false;
        }

        private Font CreateBMFont(Texture2D fontAtlas,string[] fontCharTexPaths,Rect[] rects,ref Dictionary<string, Dictionary<string, int>> mapDataDic)
        {
            Font font = AssetDatabase.LoadMainAssetAtPath(bmFontSetting.fontPath) as Font;
            if (font == null)
            {
                font = new Font();
                AssetDatabase.CreateAsset(font, bmFontSetting.fontPath);
                AssetDatabase.WriteImportSettingsIfDirty(bmFontSetting.fontPath);
                AssetDatabase.ImportAsset(bmFontSetting.fontPath);
            }
            Material material = AssetDatabase.LoadAssetAtPath(bmFontSetting.fontPath, typeof(Material)) as Material;
            Shader fontShader = Shader.Find(BMFont_Material_Shader);
            if (material == null)
            {
                material = new Material(fontShader);
                AssetDatabase.AddObjectToAsset(material, bmFontSetting.fontPath);
                AssetDatabase.ImportAsset(bmFontSetting.fontPath);
            }else
            {
                material.shader = fontShader;
            }
            material.name = "Font Material";
            material.enableInstancing = true;
            material.renderQueue = 4500;

            font.material = material;
            material.mainTexture = fontAtlas;

            List<CharacterInfo> charInfos = new List<CharacterInfo>();
            int charID = charIDStart;
            foreach(var gather in bmFontSetting.gatherList)
            {
                Dictionary<string, int> textToInt = new Dictionary<string, int>();
                mapDataDic.Add(gather.name, textToInt);

                gather.charList.ForEach((fontChar) =>
                {
                    if (fontChar.texture != null)
                    {
                        string texPath = AssetDatabase.GetAssetPath(fontChar.texture);
                        int texIndex = Array.IndexOf(fontCharTexPaths, texPath);
                        if (texIndex >= 0)
                        {
                            textToInt.Add(fontChar.text, charID);

                            CharacterInfo cInfo = new CharacterInfo();
                            cInfo.index = charID;
                            Rect rect = rects[texIndex];
                            cInfo.uvBottomLeft = new Vector2(rect.x, rect.y);
                            cInfo.uvTopRight = new Vector2(rect.x + rect.width, rect.y + rect.height);
                            cInfo.uvBottomRight = new Vector2(rect.x + rect.width, rect.y);
                            cInfo.uvTopLeft = new Vector2(rect.x, rect.y + rect.height);

                            Rect vert = new Rect(0, -rect.height * fontAtlas.height, rect.width * fontAtlas.width, rect.height * fontAtlas.height);
                            cInfo.minX = (int)vert.xMin;
                            cInfo.maxX = (int)vert.xMax;
                            cInfo.minY = (int)vert.yMin;
                            cInfo.maxY = -(int)vert.yMax;

                            cInfo.glyphWidth = Mathf.RoundToInt(fontAtlas.width * rect.width);
                            cInfo.glyphHeight = Mathf.RoundToInt(fontAtlas.height * rect.height);

                            cInfo.advance = cInfo.glyphWidth + gather.characterSpace;
                            cInfo.bearing = 0;

                            cInfo.style = FontStyle.Normal;

                            charInfos.Add(cInfo);
                            ++charID;
                        }
                        else
                        {
                            Debug.LogError("");
                        }
                    }
                });
            }
            font.characterInfo = charInfos.ToArray();

            EditorUtility.SetDirty(font);

            AssetDatabase.SaveAssets();

            return font;
        }

        private Texture2D[] GetPackedTextures(out string[] texurePaths)
        {
            List<string> fontCharTexPath = new List<string>();
            bmFontSetting.gatherList.ForEach((gather) =>
            {
                gather.charList.ForEach((fChar) =>
                {
                    if(fChar.texture!=null)
                    {
                        fontCharTexPath.Add(AssetDatabase.GetAssetPath(fChar.texture));
                    }
                });
            });
            fontCharTexPath = fontCharTexPath.Distinct().ToList();
            texurePaths = fontCharTexPath.ToArray();

            List<Texture2D> fontCharTextures = new List<Texture2D>();
            fontCharTexPath.ForEach((tp) =>
            {
                TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(tp);
                TextureImporterSettings tis = new TextureImporterSettings();
                ti.ReadTextureSettings(tis);
                tis.textureType = TextureImporterType.Default;
                tis.textureShape = TextureImporterShape.Texture2D;
                tis.sRGBTexture = true;
                tis.alphaSource = TextureImporterAlphaSource.FromInput;
                tis.alphaIsTransparency = true;
                tis.mipmapEnabled = false;
                tis.readable = true;
                tis.npotScale = TextureImporterNPOTScale.None;
                ti.SetTextureSettings(tis);

                TextureImporterPlatformSettings ps = ti.GetPlatformTextureSettings("Standalone");
                ps.overridden = true;
                ps.format = TextureImporterFormat.RGBA32;
                ti.SetPlatformTextureSettings(ps);

                AssetDatabase.ImportAsset(tp);

                fontCharTextures.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(tp));
            });

            return fontCharTextures.ToArray();
        }

        private Texture2D PackFontCharTexture(Texture2D[] textures,out Rect[] rects)
        {
            Texture2D fontAtlas = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            rects = null;
            if (textures != null && textures.Length > 0)
            {
                rects = fontAtlas.PackTextures(textures.ToArray(), 2, 1024);
            }
            string fontAtlasDiskPath = PathUtil.GetDiskPath(bmFontSetting.texturePath);
            File.WriteAllBytes(fontAtlasDiskPath, fontAtlas.EncodeToPNG());
            AssetDatabase.ImportAsset(bmFontSetting.texturePath);

            TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(bmFontSetting.texturePath);
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

            AssetDatabase.WriteImportSettingsIfDirty(bmFontSetting.texturePath);

            fontAtlas = AssetDatabase.LoadAssetAtPath<Texture2D>(bmFontSetting.texturePath);
            return fontAtlas;
        }
    }
}
