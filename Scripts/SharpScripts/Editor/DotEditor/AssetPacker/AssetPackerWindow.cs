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
        private void OnEnable()
        {
            assetPackerConfig = AssetPackerUtil.GetPackerConfig();

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

            Rect lastRect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true),GUILayout.ExpandWidth(true));
            Rect treeViewRect = lastRect;
            treeViewRect.height -= 160;

            if (assetPackerTreeView == null)
            {
                InitTreeView();
                EditorApplication.delayCall += () =>
                {
                    SetTreeModel();
                };
            }

            assetPackerTreeView?.OnGUI(treeViewRect);
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal("toolbar", GUILayout.ExpandWidth(true));
            {
                if (GUILayout.Button(isExpandAll ? "\u25BC" : "\u25BA", "toolbarbutton", GUILayout.Width(60)))
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
                EditorGUILayout.Space();
                EditorGUI.BeginChangeCheck();
                {
                    EditorGUIUtil.BeginLabelWidth(65);
                    {
                        runMode = (RunMode)EditorGUILayout.EnumPopup("Run Mode:", runMode, EditorStyles.toolbarPopup, GUILayout.Width(180));
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
                if (GUILayout.Button("Find Auto Group", "toolbarbutton", GUILayout.Width(120)))
                {
                    
                }

                if (GUILayout.Button("Remove Auto Group", "toolbarbutton", GUILayout.Width(120)))
                {
                    
                }

                if (GUILayout.Button("Open Depend Win", "toolbarbutton", GUILayout.Width(160)))
                {
                    
                }

                int newSelectedIndex = EditorGUILayout.Popup(selecteddSearchParamIndex, SearchParams, "ToolbarDropDown", GUILayout.Width(80));
                if (newSelectedIndex != selecteddSearchParamIndex)
                {
                    selecteddSearchParamIndex = newSelectedIndex;
                    SetTreeModel();
                }

                EditorGUILayout.LabelField("", GUILayout.Width(200));
                Rect lastRect = GUILayoutUtility.GetLastRect();
                Rect searchFieldRect = new Rect(lastRect.x, lastRect.y, 180, 16);
                string newSearchText = EditorGUI.TextField(searchFieldRect, "", searchText, "toolbarSeachTextField"); ;
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
                TreeElementWithData<AssetPackerTreeData> groupElementData = new TreeElementWithData<AssetPackerTreeData>(
                    new AssetPackerTreeData()
                    {
                        groupData = groupData,
                    }, "", 0, (i + 1) * 100000);

                treeModel.AddElement(groupElementData, treeModelRoot, treeModelRoot.hasChildren ? treeModelRoot.children.Count : 0);

                for(int j=0;j<groupData.assetFiles.Count;++j)
                {
                    AssetPackerAddressData addressData = groupData.assetFiles[j];
                    if(FilterAddressData(addressData))
                    {
                        TreeElementWithData<AssetPackerTreeData> elementData = new TreeElementWithData<AssetPackerTreeData>(
                                new AssetPackerTreeData()
                                {
                                    dataIndex = j,
                                    groupData = groupData,
                                }, "", 1, (i + 1) * 100000 + (j + 1));

                        treeModel.AddElement(elementData, groupElementData, groupElementData.hasChildren ? groupElementData.children.Count : 0);
                    }
                }
            }
        }
    }
}
