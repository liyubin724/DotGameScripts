using Dot.Asset;
using DotEditor.EGUI;
using ReflectionMagic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

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

        private BundleLoader bundleLoader = null;

        private EGUIToolbarSearchField searchField = null;
        private string searchText = string.Empty;
        private void OnEnable()
        {
            
        }

        private void OnGUI()
        {
            InitData();
            if(isInitSuccess)
            {
                DrawToolbar();
            }else
            {
                EditorGUILayout.LabelField(initErrorContent, Styles.bigBoldCenterLableStyle,GUILayout.ExpandHeight(true));
            }
        }

        private void InitData()
        {
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
                    bundleLoader = assetMgr.assetLoader as BundleLoader;
                    if (bundleLoader == null)
                    {
                        isInitSuccess = false;
                        initErrorContent = Contents.InitFailedForLoader;
                    }
                    else
                    {
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
        }
    }
}
