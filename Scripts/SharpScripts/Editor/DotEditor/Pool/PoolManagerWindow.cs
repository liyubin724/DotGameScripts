using Dot;
using Dot.Pool;
using Dot.Util;
using DotEditor.Core.EGUI;
using ReflectionMagic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Pool
{
    public class PoolManagerWindow : EditorWindow
    {
        [MenuItem("Game/Manager/Pool Manager")]
        public static void ShowWin()
        {
            PoolManagerWindow win = GetWindow<PoolManagerWindow>();
            win.titleContent = new GUIContent("Pool Manager");
            win.autoRepaintOnSceneChange = true;
            win.Show();
        }

        private bool isInitSuccess = false;
        private GUIContent initErrorContent = new GUIContent();
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            InitData();
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange mode)
        {
            InitData();
            Repaint();
        }

        private void InitData()
        {
            if (!EditorApplication.isPlaying)
            {
                isInitSuccess = false;
                initErrorContent = Contents.InitFailedForPlayMode;
            }else
            {
                if(DotProxy.proxy == null || !DotProxy.proxy.IsStartup)
                {
                    isInitSuccess = false;
                    initErrorContent = Contents.InitFailedForProxyStartup;
                }else
                {
                    isInitSuccess = true;
                }
            }
        }

        private Dictionary<string, SpawnPoolFoldoutData> spawnPoolFoldoutDic = new Dictionary<string, SpawnPoolFoldoutData>();
        private Vector2 scrollPos = Vector2.zero;

        //private EGUIToolbarSearchField searchField = null;
        //private string searchText = string.Empty;
        //private int searchCategoryIndex = 0;
        private void OnGUI()
        {
            if(!isInitSuccess)
            {
                EditorGUILayout.LabelField(initErrorContent, Styles.bigBoldCenterLableStyle,GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            }else
            {
                DrawToolbar();
                DrawMgr();
            }
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal("toolbar", GUILayout.ExpandWidth(true));
            {
                if(GUILayout.Button(Contents.ExpandAllContent, EditorStyles.toolbarButton,GUILayout.Width(60)))
                {
                    //foreach(var kvp in spawnPoolFoldoutDic)
                    //{
                    //    kvp.Value.isFoldout = true;
                    //    foreach (var kvp2 in kvp.Value.objectPoolFoldout)
                    //    {
                    //        kvp.Value.objectPoolFoldout[kvp2.Key] = false;
                    //    }
                    //}

                    Repaint();
                }

                if (GUILayout.Button(Contents.CollapseAllContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    //foreach (var kvp in spawnPoolFoldoutDic)
                    //{
                    //    kvp.Value.isFoldout = false;

                    //    foreach (var kvp2 in kvp.Value.objectPoolFoldout)
                    //    {
                    //        kvp.Value.objectPoolFoldout[kvp2.Key] = false;
                    //    }
                    //}

                    Repaint();
                }

                GUILayout.FlexibleSpace();

                //if (searchField == null)
                //{
                //    searchField = new EGUIToolbarSearchField((text) =>
                //    {
                //        searchText = text;
                //    }, null);
                //    searchField.Text = searchText;
                //    searchField.Categories = Contents.SearchCategories;
                //    searchField.CategoryIndex = 0;
                //}
                //searchField.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawMgr()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos,EditorStyles.helpBox);
            {
                EditorGUILayout.BeginVertical();
                {
                    dynamic dynamicPoolMgr = PoolManager.GetInstance().AsDynamic();
                    Transform mgrRootTran = dynamicPoolMgr.mgrTransform;
                    Dictionary<string, SpawnPool> spawnDic = dynamicPoolMgr.spawnDic;

                    EditorGUILayout.ObjectField(Contents.TransformContent, mgrRootTran, typeof(Transform), false);
                    EditorGUILayout.LabelField(Contents.CullIntervalContent, new GUIContent(dynamicPoolMgr.cullTimeInterval.ToString()));
                    EditorGUILayout.LabelField(Contents.CountContent, new GUIContent("" + spawnDic.Count));
                    EditorGUIUtil.BeginIndent();
                    {
                        List<string> spawnNames = spawnDic.Keys.ToList();
                        spawnNames.Sort();
                        foreach (var name in spawnNames)
                        {
                            SpawnPool spawn = spawnDic[name];
                            DrawSpawnPool(spawn);
                        }
                    }
                    EditorGUIUtil.EndIndent();

                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawSpawnPool(SpawnPool spawnPool)
        {
            var dynamicSpawnPool = spawnPool.AsDynamic();
            string poolName = dynamicSpawnPool.PoolName;
            if (!spawnPoolFoldoutDic.TryGetValue(poolName, out SpawnPoolFoldoutData foldoutData))
            {
                foldoutData = new SpawnPoolFoldoutData();
                foldoutData.isFoldout = true;
                spawnPoolFoldoutDic.Add(poolName, foldoutData);
            }
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                foldoutData.isFoldout = EditorGUILayout.Foldout(foldoutData.isFoldout, poolName);
                if (foldoutData.isFoldout)
                {
                    EditorGUIUtil.BeginIndent();
                    {
                        Transform spawnTransform = dynamicSpawnPool.SpawnTransform;
                        EditorGUILayout.ObjectField(Contents.TransformContent, spawnTransform, typeof(Transform), false);

                        Dictionary<string, GameObjectPool> gameObjectPools = dynamicSpawnPool.gameObjectPools;
                        EditorGUILayout.LabelField(Contents.CountContent, new GUIContent("" + gameObjectPools.Count));

                        EditorGUIUtil.BeginIndent();
                        {
                            List<string> poolNames = gameObjectPools.Keys.ToList();
                            poolNames.Sort();
                            foreach (var name in poolNames)
                            {
                                if (!foldoutData.objectPoolFoldout.TryGetValue(name, out bool isFoldout))
                                {
                                    isFoldout = true;
                                    foldoutData.objectPoolFoldout.Add(name, isFoldout);
                                }
                                foldoutData.objectPoolFoldout[name] = EditorGUILayout.Foldout(foldoutData.objectPoolFoldout[name], name);
                                if (foldoutData.objectPoolFoldout[name])
                                {
                                    EditorGUIUtil.BeginIndent();
                                    {
                                        DrawGameObjectPool(gameObjectPools[name]);
                                    }
                                    EditorGUIUtil.EndIndent();
                                }
                            }
                        }
                        EditorGUIUtil.EndIndent();
                    }
                    EditorGUIUtil.EndIndent();   
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawGameObjectPool(GameObjectPool gameObjectPool)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                var dynamicGameObjectPool = gameObjectPool.AsDynamic();

                string uniqueName = dynamicGameObjectPool.uniqueName;
                EditorGUILayout.LabelField(Contents.UniqueNameContent, new GUIContent(uniqueName));

                PoolTemplateType templateType = dynamicGameObjectPool.templateType;
                EditorGUILayout.EnumPopup(Contents.TemplateTypeContent, templateType);

                GameObject templateGO = dynamicGameObjectPool.instanceOrPrefabTemplate;
                EditorGUILayout.ObjectField(Contents.TemplateContent, templateGO, typeof(GameObject), false);

                Queue<GameObject> unusedItemQueue = dynamicGameObjectPool.unusedItemQueue;
                EditorGUILayout.LabelField(Contents.UnusedItemsContent);
                if (unusedItemQueue.Count > 0)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUIUtil.BeginIndent();
                        {
                            EditorGUIUtil.BeginLabelWidth(100);
                            {
                                int index = 0;
                                foreach (var gObj in unusedItemQueue)
                                {
                                    EditorGUILayout.ObjectField("" + index, gObj, typeof(GameObject), false);
                                    index++;
                                }
                            }
                            EditorGUIUtil.EndLableWidth();
                        }
                        EditorGUIUtil.EndIndent();
                    }
                    EditorGUILayout.EndVertical();
                }

                List<WeakReference<GameObject>> usedItemList = dynamicGameObjectPool.usedItemList;
                EditorGUILayout.LabelField(Contents.UsedItemsContent);
                if (usedItemList.Count > 0)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUIUtil.BeginIndent();
                        {
                            EditorGUIUtil.BeginLabelWidth(100);
                            {
                                int index = 0;
                                foreach (var weakRef in usedItemList)
                                {
                                    if (weakRef.TryGetTarget(out GameObject gObj))
                                    {
                                        if (!gObj.IsNull())
                                        {
                                            EditorGUILayout.ObjectField("" + index, gObj, typeof(GameObject), false);
                                        }else
                                        {
                                            EditorGUILayout.TextField("" + index, "it has been destroy");
                                        }
                                    }

                                    index++;
                                }
                            }
                            EditorGUIUtil.EndLableWidth();
                        }
                        EditorGUIUtil.EndIndent();
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();
        }

        class SpawnPoolFoldoutData
        {
            public bool isFoldout = false;
            public Dictionary<string, bool> objectPoolFoldout = new Dictionary<string, bool>();
        }

        static class Styles
        {
            internal static GUIStyle bigBoldCenterLableStyle;

            static Styles()
            {
                bigBoldCenterLableStyle = new GUIStyle(EditorStyles.boldLabel);
                bigBoldCenterLableStyle.alignment = TextAnchor.MiddleCenter;
                bigBoldCenterLableStyle.fontSize = 20;
            }
        }

        static class Contents
        {
            internal static GUIContent InitFailedForPlayMode = new GUIContent("This tool can only be used in Play Mode");
            internal static GUIContent InitFailedForProxyStartup = new GUIContent("DotProxy has been startup!");
            //internal static string[] SearchCategories = new string[]
            //{
            //    "All",
            //    "Spawn",
            //    "Pool",
            //};

            internal static GUIContent TransformContent = new GUIContent("Transform");
            internal static GUIContent CountContent = new GUIContent("Count");
            internal static GUIContent CullIntervalContent = new GUIContent("CullInterval");

            internal static GUIContent UniqueNameContent = new GUIContent("UniqueName");
            internal static GUIContent TemplateTypeContent = new GUIContent("TemplateType");
            internal static GUIContent TemplateContent = new GUIContent("Template");
            internal static GUIContent UnusedItemsContent = new GUIContent("UnusedItems");
            internal static GUIContent UsedItemsContent = new GUIContent("UsedItems");

            internal static GUIContent ExpandAllContent = new GUIContent("Expand", "Expand All");
            internal static GUIContent CollapseAllContent = new GUIContent("Collapse", "Collapse All");
        }
    }
}
