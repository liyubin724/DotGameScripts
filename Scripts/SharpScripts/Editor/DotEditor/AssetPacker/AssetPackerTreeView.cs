using DotEditor.Core.EGUI;
using DotEditor.Core.EGUI.TreeGUI;
using DotEditor.Util;
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

        public bool isAddressRepeat = false;
        public AssetPackerAddressData[] repeatAddressDatas = null; 

        public static AssetPackerTreeData Root
        {
            get { return new AssetPackerTreeData(); }
        }
    }

    public class AssetPackerTreeView : TreeViewWithTreeModel<TreeElementWithData<AssetPackerTreeData>>
    {
        private const float SINGLE_ROW_HEIGHT = 17;

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
                return rowHeight = SINGLE_ROW_HEIGHT * 5;
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
                        if (groupTreeData.isAddressRepeat)
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
                        if (groupTreeData.isAddressRepeat)
                        {
                            if (GUILayout.Button(addressRepeatContent, GUILayout.Width(24)))
                            {
                                Vector2 pos = GUIUtility.GUIToScreenPoint(Input.mousePosition);
                                AssetAddressRepeatPopupWindow.ShowWin(groupTreeData.repeatAddressDatas, pos);
                            }
                        }
                        DotEditorGUI.BeginLabelWidth(80);
                        {
                            AssetPackerAddressData assetData = groupData.assetFiles[groupTreeData.dataIndex];
                            EditorGUILayout.TextField("address:", assetData.assetAddress);
                            GUILayout.BeginVertical();
                            {
                                EditorGUILayout.TextField("path:", assetData.assetPath);
                                EditorGUILayout.TextField("bundle:", assetData.bundlePath);
                                EditorGUILayout.TextField("labels:", string.Join(",", assetData.labels));
                                EditorGUILayout.LabelField("compression:  ", assetData.compressionType.ToString());
                            }
                            GUILayout.EndVertical();

                            if(GUILayout.Button("Select",GUILayout.Width(60),GUILayout.ExpandHeight(true)))
                            {
                                SelectionUtil.ActiveObject(assetData.assetPath);
                            }
                        }
                        DotEditorGUI.EndLableWidth();
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndArea();
        }
    }
}
