using DotEditor.Core.EGUI;
using DotEditor.Core.EGUI.TreeGUI;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.AssetPacker
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
                return EditorGUIUtility.singleLineHeight + 2;
            }
            else
            {
                return rowHeight = EditorGUIUtility.singleLineHeight * 3 + 4;
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

            GUILayout.BeginArea(contentRect);
            {
                if (groupTreeData.IsGroup)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
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
                        EditorGUIUtil.BeginLabelWidth(60);
                        {
                            AssetPackerAddressData assetData = groupData.assetFiles[groupTreeData.dataIndex];
                            //EditorGUILayout.LabelField(new GUIContent("" + args.row), GUILayout.Width(20));
                            EditorGUILayout.TextField("address:", assetData.assetAddress);
                            GUILayout.BeginVertical();
                            {
                                EditorGUILayout.TextField("path:", assetData.assetPath);
                                EditorGUILayout.TextField("bundle:", assetData.bundlePath);
                                EditorGUILayout.TextField("labels:", string.Join(",", assetData.labels));
                            }
                            GUILayout.EndVertical();
                        }
                        EditorGUIUtil.EndLableWidth();
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndArea();
        }
    }
}
