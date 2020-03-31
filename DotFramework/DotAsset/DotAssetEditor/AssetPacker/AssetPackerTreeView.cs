using DotEditor.Core;
using DotEditor.Core.TreeGUI;
using DotEditor.Core.Util;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.Asset.AssetPacker
{
    public class AssetPackerTreeData
    {
        public AssetPackerGroupData groupData;
        public int dataIndex = -1;

        public bool IsGroup { get => dataIndex < 0; }

        public static AssetPackerTreeData Root
        {
            get { return new AssetPackerTreeData(); }
        }
    }

    public class AssetPackerTreeView : TreeViewWithTreeModel<TreeElementWithData<AssetPackerTreeData>>
    {
        private const float SINGLE_ROW_HEIGHT = 17;

        public AssetPackerWindow Window { get; set; } = null;

        private GUIContent addressRepeatContent;

        public AssetPackerTreeView(TreeViewState state, TreeModel<TreeElementWithData<AssetPackerTreeData>> model)
            : base(state, model)
        {
            addressRepeatContent = EditorGUIUtility.IconContent("console.erroricon.sml", "Address Repeat");
            showBorder = true;
            showAlternatingRowBackgrounds = true;
            Reload();
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override bool CanStartDrag(CanStartDragArgs args)
        {
            return false;
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            var viewItem = (TreeViewItem<TreeElementWithData<AssetPackerTreeData>>)item;
            AssetPackerTreeData groupTreeData = viewItem.data.Data;
            if (groupTreeData.IsGroup)
            {
                return SINGLE_ROW_HEIGHT + 8;
            }
            else
            {
                return SINGLE_ROW_HEIGHT * 5;
            }
        }
        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (TreeViewItem<TreeElementWithData<AssetPackerTreeData>>)args.item;
            AssetPackerTreeData groupTreeData = item.data.Data;
            AssetPackerGroupData groupData = groupTreeData.groupData;

            int childCount = item.data.children == null ? 0 : item.data.children.Count;

            Rect contentRect = args.rowRect;
            contentRect.x += GetContentIndent(item);
            contentRect.width -= GetContentIndent(item);
            contentRect.y += 2;
            contentRect.height -= 4;

            GUILayout.BeginArea(contentRect);
            {
                if (groupTreeData.IsGroup)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (Window.IsGroupAddressRepeated(groupData))
                        {
                            if (GUILayout.Button(addressRepeatContent,GUILayout.Width(24)))
                            {
                                SetExpanded(args.item.id, true);
                            }
                        }

                        string groupName = groupData.groupName;
                        if (groupData.isMain)
                        {
                            groupName += "(Main)";
                        }
                        if(groupData.isPreload)
                        {
                            groupName += "(Preload)";
                        }
                        if(groupData.isNeverDestroy)
                        {
                            groupName += "(NeverDestroy)";
                        }

                        groupName += "  " + childCount;

                        EditorGUILayout.LabelField(new GUIContent(groupName),GUILayout.ExpandWidth(true));
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    {
                        AssetPackerAddressData assetData = groupData.assetFiles[groupTreeData.dataIndex];
                        if (Window.IsAddressRepeated(assetData.assetAddress,out List<AssetPackerAddressData> datas))
                        {
                            Rect rect = GUILayoutUtility.GetRect(addressRepeatContent,"button",GUILayout.Width(24));
                            if (GUILayout.Button(addressRepeatContent, GUILayout.Width(24)))
                            {
                                AssetAddressRepeatPopupContent content = new AssetAddressRepeatPopupContent()
                                {
                                    RepeatAddressDatas = datas.ToArray(),
                                };
                                PopupWindow.Show(rect, content);
                            }
                        }
                        EGUI.BeginLabelWidth(80);
                        {
                            EditorGUILayout.TextField("address:", assetData.assetAddress);
                            GUILayout.BeginVertical();
                            {
                                EditorGUILayout.TextField("path:", assetData.assetPath);
                                EditorGUILayout.TextField("bundle:", assetData.bundlePath);
                                EditorGUILayout.TextField("bundle-md5:", assetData.bundlePathMd5);
                                EditorGUILayout.TextField("labels:", string.Join(",", assetData.labels));
                            }
                            GUILayout.EndVertical();
                        }
                        EGUI.EndLableWidth();
                        if (GUILayout.Button("Select", GUILayout.Width(60), GUILayout.ExpandHeight(true)))
                        {
                            SelectionUtil.PingObject(assetData.assetPath);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndArea();
        }
    }
}
