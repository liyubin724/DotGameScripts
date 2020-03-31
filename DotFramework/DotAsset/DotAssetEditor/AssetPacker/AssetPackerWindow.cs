using DotEditor.Core;
using DotEditor.Core.TreeGUI;
using DotEditor.Core.Util;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.Asset.AssetPacker
{
    public enum RunMode
    {
        AssetDatabase,
        AssetBundle,
    }

    public enum BundlePathFormatType
    {
        Origin = 0,
        MD5,
    }

    public class AssetPackerWindow : EditorWindow
    {
        [MenuItem("Game/Asset/Packer Window",priority =10)]
        public static void ShowWin()
        {
            AssetPackerWindow win = EditorWindow.GetWindow<AssetPackerWindow>();
            win.titleContent = new GUIContent("Bundle Packer");
            win.Show();
        }
        private static readonly string ASSET_BUNDLE_SYMBOL = "ASSET_BUNDLE";

        private RunMode runMode = RunMode.AssetDatabase;
        private BundlePathFormatType bundlePathFormatType = BundlePathFormatType.Origin;
        
        private bool isExpandAll = false;

        private EGUIToolbarSearchField searchField = null;
        public string[] SearchCategories = new string[]
        {
            "All",
            "Address",
            "Path",
            "Bundle",
            "Labels",
        };
        private int searchCategoryIndex = 0;
        private string searchText = "";

        private AssetPackerTreeView assetPackerTreeView;
        private TreeViewState assetPackerTreeViewState;

        private AssetPackerConfig assetPackerConfig = null;
        private Dictionary<string, List<AssetPackerAddressData>> addressRepeatDataDic = new Dictionary<string, List<AssetPackerAddressData>>();
        private void OnEnable()
        {
            assetPackerConfig = AssetPackerUtil.GetAssetPackerConfig();

            foreach(var group in assetPackerConfig.groupDatas)
            {
                foreach(var data in group.assetFiles)
                {
                    if(!addressRepeatDataDic.TryGetValue(data.assetAddress,out List<AssetPackerAddressData> dataList))
                    {
                        dataList = new List<AssetPackerAddressData>();
                        addressRepeatDataDic.Add(data.assetAddress, dataList);
                    }
                    dataList.Add(data);
                }
            }

            if (PlayerSettingsUtil.HasScriptingDefineSymbol(ASSET_BUNDLE_SYMBOL))
            {
                runMode = RunMode.AssetBundle;
            }
        }

        public bool IsAddressRepeated(string address,out List<AssetPackerAddressData> datas)
        {
            if(addressRepeatDataDic.TryGetValue(address,out List<AssetPackerAddressData> list))
            {
                if(list.Count>1)
                {
                    datas = list;
                    return true;
                }
            }

            datas = null;
            return false;
        }

        public bool IsGroupAddressRepeated(AssetPackerGroupData group)
        {
            foreach(var data in group.assetFiles)
            {
                if(IsAddressRepeated(data.assetAddress, out List <AssetPackerAddressData> list))
                {
                    return true;
                }
            }
            return false;
        }

        private void OnGUI()
        {
            DrawToolbar();

            EditorGUILayout.LabelField("Asset Packer Group", EGUIStyles.MiddleCenterLabel, GUILayout.ExpandWidth(true));

            if (assetPackerTreeView == null)
            {
                InitTreeView();
                EditorApplication.delayCall += () =>
                {
                    SetTreeModel();
                };
            }
            Rect lastRect = GUILayoutUtility.GetLastRect();
            GUILayout.Label("", GUILayout.ExpandWidth(true), GUILayout.Height(position.height - lastRect.y-lastRect.height -120));
            lastRect = GUILayoutUtility.GetLastRect();
            Rect treeViewRect = new Rect(lastRect.x, lastRect.y, lastRect.width, lastRect.height);
            assetPackerTreeView?.OnGUI(treeViewRect);

            //GUILayout.BeginHorizontal();
            //{
            //    GUILayout.BeginVertical();
            //    {
            //        DrawBundleConfig();
            //    }
            //    GUILayout.EndVertical();

            //    GUILayout.BeginVertical(GUILayout.Width(200));
            //    {
            //        DrawBundleOperation();
            //    }
            //    GUILayout.EndVertical();

            //    GUILayout.BeginVertical(GUILayout.Width(200));
            //    {
            //        DrawBundleAutoOperation();
            //    }
            //    GUILayout.EndVertical();
            //}
            //GUILayout.EndHorizontal();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal("toolbar", GUILayout.ExpandWidth(true));
            {
                if (GUILayout.Button(isExpandAll ? "\u25BC" : "\u25BA", EditorStyles.toolbarButton, GUILayout.Width(30)))
                {
                    isExpandAll = !isExpandAll;
                    if (isExpandAll)
                    {
                        assetPackerTreeView.ExpandAll();
                    }
                    else
                    {
                        assetPackerTreeView.CollapseAll();
                    }
                }
                EditorGUI.BeginChangeCheck();
                {
                    EGUI.BeginLabelWidth(70);
                    {
                        runMode = (RunMode)EditorGUILayout.EnumPopup(Contents.RunModeContent, runMode, EditorStyles.toolbarPopup, GUILayout.Width(170));
                    }
                    EGUI.EndLableWidth();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    if (runMode == RunMode.AssetBundle)
                    {
                        PlayerSettingsUtil.AddScriptingDefineSymbol(ASSET_BUNDLE_SYMBOL);
                    }
                    else
                    {
                        PlayerSettingsUtil.RemoveScriptingDefineSymbol(ASSET_BUNDLE_SYMBOL);
                    }
                }

                bundlePathFormatType = (BundlePathFormatType)EditorGUILayout.EnumPopup(Contents.BundlePathFormatContent, bundlePathFormatType, EditorStyles.toolbarPopup, GUILayout.Width(170));

                GUILayout.FlexibleSpace();

                if(searchField == null)
                {
                    searchField = new EGUIToolbarSearchField((text) =>
                    {
                        if(searchText!=text)
                        {
                            searchText = text;
                            SetTreeModel();
                        }
                    }, (category) =>
                    {
                        int newIndex = Array.IndexOf(SearchCategories, category);
                        if(searchCategoryIndex!=newIndex)
                        {
                            searchCategoryIndex = newIndex;
                            SetTreeModel();
                        }
                    });

                    searchField.Categories = SearchCategories;
                    searchField.CategoryIndex = 0;
                }
                searchField.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void InitTreeView()
        {
            assetPackerTreeViewState = new TreeViewState();
            TreeModel<TreeElementWithData<AssetPackerTreeData>> model = new TreeModel<TreeElementWithData<AssetPackerTreeData>>(
               new List<TreeElementWithData<AssetPackerTreeData>>()
               {
                    new TreeElementWithData<AssetPackerTreeData>(AssetPackerTreeData.Root,"",-1,-1),
               });
            assetPackerTreeView = new AssetPackerTreeView(assetPackerTreeViewState, model);
            assetPackerTreeView.Window = this;
        }

        private bool FilterAddressData(AssetPackerAddressData addressData)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }

            bool isValid = false;
            if (searchCategoryIndex == 0 || searchCategoryIndex == 1)
            {
                if (!string.IsNullOrEmpty(addressData.assetAddress))
                {
                    isValid = addressData.assetAddress.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                }
            }
            if (!isValid)
            {
                if (searchCategoryIndex == 0 || searchCategoryIndex == 2)
                {
                    if (!string.IsNullOrEmpty(addressData.assetPath))
                    {
                        isValid = addressData.assetPath.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }
            }
            if (!isValid)
            {
                if (searchCategoryIndex == 0 || searchCategoryIndex == 3)
                {
                    if (!string.IsNullOrEmpty(addressData.bundlePath))
                    {
                        string bPath = $"{addressData.bundlePath} {addressData.bundlePathMd5}";
                        isValid = bPath.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }
            }
            if (!isValid)
            {
                string label = string.Join(",", addressData.labels);
                if (searchCategoryIndex == 0 || searchCategoryIndex == 4)
                {
                    if (!string.IsNullOrEmpty(label))
                    {
                        isValid = label.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }
            }
            return isValid;
        }

        private void SetTreeModel()
        {
            TreeModel<TreeElementWithData<AssetPackerTreeData>> treeModel = assetPackerTreeView.treeModel;
            TreeElementWithData<AssetPackerTreeData> treeModelRoot = treeModel.root;
            treeModelRoot.children?.Clear();

            for(int i =0;i<assetPackerConfig.groupDatas.Count;++i)
            {
                AssetPackerGroupData groupData = assetPackerConfig.groupDatas[i];
                AssetPackerTreeData groupTreeData = new AssetPackerTreeData();
                groupTreeData.groupData = groupData;

                TreeElementWithData<AssetPackerTreeData> groupElementData = new TreeElementWithData<AssetPackerTreeData>(
                    groupTreeData, "", 0, (i + 1) * 100000);

                treeModel.AddElement(groupElementData, treeModelRoot, treeModelRoot.hasChildren ? treeModelRoot.children.Count : 0);

                for(int j=0;j<groupData.assetFiles.Count;++j)
                {
                    AssetPackerAddressData addressData = groupData.assetFiles[j];
                    if(FilterAddressData(addressData))
                    {
                        AssetPackerTreeData elementTreeData = new AssetPackerTreeData();
                        elementTreeData.groupData = groupData;
                        elementTreeData.dataIndex = j;

                        TreeElementWithData<AssetPackerTreeData> elementData = new TreeElementWithData<AssetPackerTreeData>(
                                elementTreeData, "", 1, (i + 1) * 100000 + (j + 1));

                        treeModel.AddElement(elementData, groupElementData, groupElementData.hasChildren ? groupElementData.children.Count : 0);
                    }
                }
            }
        }

        //private BundlePackConfig bundlePackConfig = null;
        //private void DrawBundleConfig()
        //{
        //    if(bundlePackConfig == null)
        //    {
        //        bundlePackConfig = AssetPackerUtil.GetBundlePackConfig();
        //    }

        //    EditorGUI.BeginChangeCheck();
        //    {
        //        bundlePackConfig.bundleOutputDir = EGUILayout.DrawDiskFolderSelection("Bundle Output Dir", bundlePackConfig.bundleOutputDir);
        //        bundlePackConfig.cleanupBeforeBuild = EditorGUILayout.Toggle("Cleanup", bundlePackConfig.cleanupBeforeBuild);
        //        bundlePackConfig.buildTarget = (ValidBuildTarget)EditorGUILayout.EnumPopup("Build Target", bundlePackConfig.buildTarget);
        //        bundlePackConfig.compression = (CompressOption)EditorGUILayout.EnumPopup("Compression", bundlePackConfig.compression);
        //    }
        //    if(EditorGUI.EndChangeCheck())
        //    {
        //        AssetPackerUtil.SaveBundlePackConfig(bundlePackConfig);
        //    }
        //}

        //private void DrawBundleOperation()
        //{
        //    if(GUILayout.Button("Update Address"))
        //    {
        //        AssetAddressUtil.UpdateAddressConfig();
        //    }
        //    if(GUILayout.Button("Set Bundle Names"))
        //    {
        //        AssetPackerUtil.SetAssetBundleNames(assetPackerConfig, true);
        //    }
        //    if(GUILayout.Button("Clear Bundle Names"))
        //    {
        //        AssetPackerUtil.ClearBundleNames();
        //    }
        //    if(GUILayout.Button("Pack Bundle"))
        //    {
        //        AssetPackerUtil.PackAssetBundle(assetPackerConfig, bundlePackConfig, true);
        //    }
        //}

        //private void DrawBundleAutoOperation()
        //{
        //    DotEditorGUI.BeginGUIBackgroundColor(Color.red);
        //    {
        //        if(GUILayout.Button("Auto Pack Bundle",GUILayout.Height(40)))
        //        {
        //            EditorApplication.delayCall += () =>
        //            {
        //                AssetAddressUtil.UpdateAddressConfig();

        //                AssetPackerUtil.ClearBundleNames();
        //                AssetPackerUtil.SetAssetBundleNames(assetPackerConfig, true);
        //                AssetPackerUtil.PackAssetBundle(assetPackerConfig, bundlePackConfig, true);
        //            };
        //        }
        //    }
        //    DotEditorGUI.EndGUIBackgroundColor();
        //}

        static class Contents
        {
            internal static GUIContent WinTitleContent = new GUIContent("Asset Packer");
            internal static GUIContent RunModeContent = new GUIContent("Run Mode");
            internal static GUIContent BundlePathFormatContent = new GUIContent("Path Format");
        }
    }
}
