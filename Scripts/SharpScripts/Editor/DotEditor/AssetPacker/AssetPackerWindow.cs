using DotEditor.AssetFilter.AssetAddress;
using DotEditor.Core.EGUI;
using DotEditor.Core.EGUI.TreeGUI;
using DotEditor.Util;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.AssetPacker
{
    internal enum RunMode
    {
        AssetDatabase,
        AssetBundle,
    }

    public class AssetPackerWindow : EditorWindow
    {
        [MenuItem("Game/Asset Packer/Packer Window")]
        public static void ShowWin()
        {
            AssetPackerWindow win = EditorWindow.GetWindow<AssetPackerWindow>();
            win.titleContent = new GUIContent("Bundle Packer");
            win.Show();
        }
        private static readonly string ASSET_BUNDLE_SYMBOL = "ASSET_BUNDLE";

        public string[] SearchParams = new string[]
        {
            "All",
            "Address",
            "Path",
            "Bundle",
            "Labels",
        };
        private int selecteddSearchParamIndex = 0;
        private string searchText = "";
        private bool isExpandAll = false;

        private RunMode runMode = RunMode.AssetDatabase;

        private AssetPackerTreeView assetPackerTreeView;
        private TreeViewState assetPackerTreeViewState;

        private AssetPackerConfig assetPackerConfig = null;
        private Dictionary<string, List<AssetPackerAddressData>> addressDataDic = new Dictionary<string, List<AssetPackerAddressData>>(); 
        private void OnEnable()
        {
            assetPackerConfig = AssetPackerUtil.GetAssetPackerConfig();

            foreach(var groupData in assetPackerConfig.groupDatas)
            {
                foreach(var addressData in groupData.assetFiles)
                {
                    if(!addressDataDic.TryGetValue(addressData.assetAddress,out List<AssetPackerAddressData> list))
                    {
                        list = new List<AssetPackerAddressData>();
                        addressDataDic.Add(addressData.assetAddress, list);
                    }

                    list.Add(addressData);
                }
            }

            if (PlayerSettingsUtil.HasScriptingDefineSymbol(ASSET_BUNDLE_SYMBOL))
            {
                runMode = RunMode.AssetBundle;
            }
        }

        private void OnGUI()
        {
            DrawToolbar();

            GUIStyle lableStyle = new GUIStyle(EditorStyles.boldLabel);
            lableStyle.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.LabelField("Asset Packer Group", lableStyle, GUILayout.ExpandWidth(true));

            EditorGUILayout.LabelField("", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            
            Rect lastRect = GUILayoutUtility.GetLastRect();
            if (assetPackerTreeView == null)
            {
                InitTreeView();
                EditorApplication.delayCall += () =>
                {
                    SetTreeModel();
                };
            }

            assetPackerTreeView?.OnGUI(lastRect);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    DrawBundleConfig();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUILayout.Width(200));
                {
                    DrawBundleOperation();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUILayout.Width(200));
                {
                    DrawBundleAutoOperation();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal("toolbar", GUILayout.ExpandWidth(true));
            {
                if (GUILayout.Button(isExpandAll ? "\u25BC" : "\u25BA", "toolbarbutton", GUILayout.Width(30)))
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
                    EditorGUIUtil.BeginLabelWidth(70);
                    {
                        runMode = (RunMode)EditorGUILayout.EnumPopup("Run Mode:", runMode, EditorStyles.toolbarPopup, GUILayout.Width(170));
                    }
                    EditorGUIUtil.EndLableWidth();
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

                GUILayout.FlexibleSpace();
                //if (GUILayout.Button("Find Auto Group", "toolbarbutton", GUILayout.Width(120)))
                //{
                    
                //}

                //if (GUILayout.Button("Remove Auto Group", "toolbarbutton", GUILayout.Width(120)))
                //{
                    
                //}

                //if (GUILayout.Button("Open Depend Win", "toolbarbutton", GUILayout.Width(160)))
                //{
                    
                //}

                int newSelectedIndex = EditorGUILayout.Popup(selecteddSearchParamIndex, SearchParams, "ToolbarDropDown", GUILayout.Width(60));
                if (newSelectedIndex != selecteddSearchParamIndex)
                {
                    selecteddSearchParamIndex = newSelectedIndex;
                    SetTreeModel();
                }

                Rect searchRect = EditorGUILayout.GetControlRect(GUILayout.Width(120));

                Rect searchFieldRect = searchRect;
                searchFieldRect.width = 100;
                string newSearchText = EditorGUI.TextField(searchFieldRect, "", searchText, "toolbarSeachTextField");
                Rect searchCancelRect = new Rect(searchFieldRect.x + searchFieldRect.width, searchFieldRect.y, 16, 16);
                if (GUI.Button(searchCancelRect, "", "ToolbarSeachCancelButton"))
                {
                    newSearchText = "";
                    GUI.FocusControl("");
                }
                if (newSearchText != searchText)
                {
                    searchText = newSearchText;
                    EditorApplication.delayCall += () =>
                    {
                        SetTreeModel();
                        isExpandAll = true;
                        assetPackerTreeView.ExpandAll();
                    };

                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void InitTreeView()
        {
            assetPackerTreeViewState = new TreeViewState();
            TreeModel<TreeElementWithData<AssetPackerTreeData>> data = new TreeModel<TreeElementWithData<AssetPackerTreeData>>(
               new List<TreeElementWithData<AssetPackerTreeData>>()
               {
                    new TreeElementWithData<AssetPackerTreeData>(AssetPackerTreeData.Root,"",-1,-1),
               });

            assetPackerTreeView = new AssetPackerTreeView(assetPackerTreeViewState, data);
        }

        private bool FilterAddressData(AssetPackerAddressData addressData)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }

            bool isValid = false;
            if (selecteddSearchParamIndex == 0 || selecteddSearchParamIndex == 1)
            {
                if (!string.IsNullOrEmpty(addressData.assetAddress))
                {
                    isValid = addressData.assetAddress.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                }
            }
            if (!isValid)
            {
                if (selecteddSearchParamIndex == 0 || selecteddSearchParamIndex == 2)
                {
                    if (!string.IsNullOrEmpty(addressData.assetPath))
                    {
                        isValid = addressData.assetPath.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }
            }
            if (!isValid)
            {
                if (selecteddSearchParamIndex == 0 || selecteddSearchParamIndex == 3)
                {
                    if (!string.IsNullOrEmpty(addressData.bundlePath))
                    {
                        isValid = addressData.bundlePath.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }
            }
            if (!isValid)
            {
                string label = string.Join(",", addressData.labels);
                if (selecteddSearchParamIndex == 0 || selecteddSearchParamIndex == 4)
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
                        if (addressDataDic[addressData.assetAddress].Count > 1)
                        {
                            elementTreeData.repeatAddressDatas = addressDataDic[addressData.assetAddress].ToArray();
                            elementTreeData.isAddressRepeat = true;
                            groupTreeData.isAddressRepeat = true;
                        }

                        TreeElementWithData<AssetPackerTreeData> elementData = new TreeElementWithData<AssetPackerTreeData>(
                                elementTreeData, "", 1, (i + 1) * 100000 + (j + 1));

                        treeModel.AddElement(elementData, groupElementData, groupElementData.hasChildren ? groupElementData.children.Count : 0);
                    }
                }
            }
        }

        private BundlePackConfig bundlePackConfig = null;
        private void DrawBundleConfig()
        {
            if(bundlePackConfig == null)
            {
                bundlePackConfig = AssetPackerUtil.GetBundlePackConfig();
            }
            EditorGUI.BeginChangeCheck();
            {
                bundlePackConfig.bundleOutputDir = EditorGUILayoutUtil.DrawDiskFolderSelection("Bundle Output Dir", bundlePackConfig.bundleOutputDir);
                bundlePackConfig.cleanupBeforeBuild = EditorGUILayout.Toggle("Cleanup", bundlePackConfig.cleanupBeforeBuild);
                bundlePackConfig.buildTarget = (ValidBuildTarget)EditorGUILayout.EnumPopup("Build Target", bundlePackConfig.buildTarget);
                bundlePackConfig.compression = (CompressOption)EditorGUILayout.EnumPopup("Compression", bundlePackConfig.compression);
            }
            if(EditorGUI.EndChangeCheck())
            {
                AssetPackerUtil.SaveBundlePackConfig(bundlePackConfig);
            }
        }

        private void DrawBundleOperation()
        {
            if(GUILayout.Button("Update Address"))
            {
                AssetAddressUtil.UpdateAddressConfig();
            }
            if(GUILayout.Button("Set Bundle Names"))
            {
                AssetPackerUtil.SetAssetBundleNames(assetPackerConfig, true);
            }
            if(GUILayout.Button("Clear Bundle Names"))
            {
                AssetPackerUtil.ClearBundleNames();
            }
            if(GUILayout.Button("Pack Bundle"))
            {
                AssetPackerUtil.PackAssetBundle(assetPackerConfig, bundlePackConfig, true);
            }
        }

        private void DrawBundleAutoOperation()
        {
            EditorGUIUtil.BeginGUIBackgroundColor(Color.red);
            {
                if(GUILayout.Button("Auto Pack Bundle",GUILayout.Height(40)))
                {
                    AssetAddressUtil.UpdateAddressConfig();
                    AssetPackerUtil.ClearBundleNames();
                    AssetPackerUtil.SetAssetBundleNames(assetPackerConfig, true);
                    AssetPackerUtil.PackAssetBundle(assetPackerConfig, bundlePackConfig, true);
                }
            }
            EditorGUIUtil.EndGUIBackgroundColor();
        }
    }
}
