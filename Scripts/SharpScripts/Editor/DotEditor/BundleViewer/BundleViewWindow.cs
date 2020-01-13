using Dot.Asset;
using DotEditor.Core.EGUI;
using DotEditor.EGUI;
using ReflectionMagic;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEditor.BundleViewer
{
    public class BundleViewWindow : EditorWindow
    {
        [MenuItem("Game/Asset/Bundle Viewer")]
        public static void ShowWin()
        {
            BundleViewWindow win = EditorWindow.GetWindow<BundleViewWindow>();
            win.titleContent = new GUIContent("Bundle Viewer");
            win.autoRepaintOnSceneChange = true;
            win.Show();
        }

        private bool isInitSuccess = false;
        private GUIContent initErrorContent = new GUIContent();

        private int toolbarSelectIndex = 0;

        private dynamic dynamicBundleLoader = null;

        private EGUIToolbarSearchField searchField = null;
        private string searchText = string.Empty;
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

        private Vector2 scrollPos = Vector2.zero;
        private void OnGUI()
        {
            if(!isInitSuccess)
            {
                EditorGUILayout.LabelField(initErrorContent, Styles.bigBoldCenterLableStyle,GUILayout.ExpandHeight(true));
                return;
            }
            DrawToolbar();
            int selectedIndex = GUILayout.Toolbar(toolbarSelectIndex, Contents.ToolbarTitle,GUILayout.ExpandWidth(true),GUILayout.Height(40));
            if (selectedIndex != toolbarSelectIndex)
            {
                searchText = "";
                toolbarSelectIndex = selectedIndex;
                scrollPos = Vector2.zero;
            }
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                if(toolbarSelectIndex == 0)
                {
                    DrawAssetNodes();
                }else if(toolbarSelectIndex == 1)
                {
                    DrawBundleNodes();
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void InitData()
        {
            dynamicBundleLoader = null;

            if (!EditorApplication.isPlaying)
            {
                isInitSuccess = false;
                initErrorContent = Contents.InitFailedForPlayMode;
            }
            else
            {
                dynamic assetMgr = AssetManager.GetInstance().AsDynamic();
                AssetLoaderMode loaderMode = assetMgr.loaderMode;
                if (loaderMode != AssetLoaderMode.AssetBundle)
                {
                    isInitSuccess = false;
                    initErrorContent = Contents.InitFailedForLoaderMode;
                }
                else
                {
                    AAssetLoader assetLoader = assetMgr.assetLoader;
                    BundleLoader bundleLoader = (BundleLoader)assetLoader;
                    if (bundleLoader == null)
                    {
                        isInitSuccess = false;
                        initErrorContent = Contents.InitFailedForLoader;
                    }
                    else
                    {
                        dynamicBundleLoader = bundleLoader.AsDynamic();
                        isInitSuccess = true;
                    }
                }
            }
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal("toolbar", GUILayout.ExpandWidth(true));
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("GC", EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    AssetManager.GetInstance().UnloadUnusedAsset();
                }

                GUILayout.Space(5);

                if (searchField == null)
                {
                    searchField = new EGUIToolbarSearchField((text) =>
                    {
                        searchText = text;
                    }, null);
                    searchField.Text = searchText;
                }
                searchField.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawAssetNodes()
        {
            Dictionary<string, AAssetNode> assetNodeDic = dynamicBundleLoader.assetNodeDic;
            foreach(var kvp in assetNodeDic)
            {
                AAssetNode assetNode = kvp.Value;
                BundleAssetNode node = (BundleAssetNode)assetNode;
                DrawAssetNode(node);
            }
        }

        private void DrawAssetNode(BundleAssetNode node)
        {
            dynamic dynamicNode = node.AsDynamic();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField(Contents.AssetPathContent, new GUIContent(dynamicNode.AssetPath));
                EditorGUILayout.Toggle(Contents.IsNeverDestroyContent, dynamicNode.IsNeverDestroy);
                EditorGUILayout.LabelField(Contents.RefCountContent, new GUIContent(""+dynamicNode.refCount));
                EditorGUILayout.LabelField(Contents.BundleNodeContent);
                BundleNode bNode = dynamicNode.bundleNode;
                EditorGUIUtil.BeginIndent();
                {
                    if(bNode == null)
                    {
                        EditorGUILayout.LabelField(Contents.NULLContent);
                    }else
                    {
                        DrawBundleNode(bNode);
                    }
                }
                EditorGUIUtil.EndIndent();
                EditorGUILayout.LabelField(Contents.WeakAssetContent);
                EditorGUIUtil.BeginIndent();
                {
                    List<WeakReference> weakAssets = dynamicNode.weakAssets;
                    int index = 0;
                    foreach(var asset in weakAssets)
                    {
                        if(!IsNull(asset.Target))
                        {
                            EditorGUILayout.ObjectField("" + index.ToString(), ((UnityObject)asset.Target), typeof(UnityObject), false);
                        }
                    }
                }
                EditorGUIUtil.EndIndent();
            }
            EditorGUILayout.EndVertical();
        }

        private bool IsNull(SystemObject sysObj)
        {
            if (sysObj == null || sysObj.Equals(null))
            {
                return true;
            }

            return false;
        }

        private void DrawBundleNodes()
        {
            Dictionary<string, BundleNode> bundleNodeDic = dynamicBundleLoader.bundleNodeDic;
            foreach (var kvp in bundleNodeDic)
            {
                DrawBundleNode(kvp.Value);
            }
        }

        private void DrawBundleNode(BundleNode node)
        {
            dynamic dynamicNode = node.AsDynamic();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField(Contents.BundlePathContent, new GUIContent(dynamicNode.BundlePath));
                EditorGUILayout.LabelField(Contents.RefCountContent, new GUIContent(""+dynamicNode.RefCount));
                EditorGUILayout.Toggle(Contents.IsDoneContent, dynamicNode.isDone);
                AssetBundle ab = dynamicNode.assetBundle;
                EditorGUILayout.LabelField(Contents.AssetBundleContent, ab == null ? Contents.NULLContent : new GUIContent(ab.name));
                EditorGUILayout.Toggle(Contents.IsUsedBySceneContent, dynamicNode.IsUsedByScene);
                EditorGUILayout.LabelField(Contents.DependNodesContent);
                EditorGUIUtil.BeginIndent();
                {
                    List<BundleNode> nodes = dynamicNode.dependNodes;
                    if(nodes.Count == 0)
                    {
                        EditorGUILayout.LabelField(Contents.EmptyContent);
                    }else
                    {
                        for(int i =0;i<nodes.Count;++i)
                        {
                            EditorGUILayout.LabelField(i.ToString(), nodes[i].AsDynamic().BundlePath);
                        }
                    }
                }
                EditorGUIUtil.EndIndent();
            }
            EditorGUILayout.EndVertical();
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
            internal static GUIContent InitFailedForLoaderMode = new GUIContent("This tool can only be used in AssetBundle Mode");
            internal static GUIContent InitFailedForLoader = new GUIContent("BundleLoader is null");
            internal static GUIContent[] ToolbarTitle = new GUIContent[]
            {
                new GUIContent("Asset Node"),
                new GUIContent("Bundle Node"),
            };

            internal static GUIContent NULLContent = new GUIContent("Null");
            internal static GUIContent EmptyContent = new GUIContent("Empty");

            internal static GUIContent BundlePathContent = new GUIContent("BundlePath");
            internal static GUIContent RefCountContent = new GUIContent("RefCount");
            internal static GUIContent IsDoneContent = new GUIContent("IsDone");
            internal static GUIContent AssetBundleContent = new GUIContent("AssetBundle");
            internal static GUIContent DependNodesContent = new GUIContent("Depend Nodes");
            internal static GUIContent IsUsedBySceneContent = new GUIContent("IsUsedByScene");

            internal static GUIContent AssetPathContent = new GUIContent("AssetPath");
            internal static GUIContent IsNeverDestroyContent = new GUIContent("IsNeverDestroy");
            internal static GUIContent BundleNodeContent = new GUIContent("BundleNode");
            internal static GUIContent WeakAssetContent = new GUIContent("WeakAssets");
            internal static GUIContent IsSceneContent = new GUIContent("IsSceneContent");
        }
    }
}
