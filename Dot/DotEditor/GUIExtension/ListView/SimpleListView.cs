using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.GUIExtension.ListView
{
    public interface ISimpleListViewItem
    {
        int GetID();
        string GetDisplayName();
    }

    class SimpleListViewItem<T> : TreeViewItem where T : class,ISimpleListViewItem
    {
        public T ItemData { get; private set; }
        public SimpleListViewItem(T itemData)
        {
            ItemData = itemData;

            if (ItemData != null)
            {
                id = ItemData.GetID();
                displayName = ItemData.GetDisplayName();
                depth = 0;
            }
            else
            {
                depth = -1;
                id = -1;
            }
            children = new List<TreeViewItem>();
        }
    }

    public delegate void OnSimpleListViewItemSelected<T>(T item) where T : class, ISimpleListViewItem;
    public delegate void DrawSimpleListViewItem<T>(Rect rect, T item) where T : class, ISimpleListViewItem;
    public delegate float GetSimpleListViewItemHeight<T>(T item) where T : class, ISimpleListViewItem;

    public class SimpleListView<T> : TreeView where T : class,ISimpleListViewItem
    {
        public OnSimpleListViewItemSelected<T> OnItemSelected{get; set;}
        public DrawSimpleListViewItem<T> OnDrawItem { get; set; }
        public GetSimpleListViewItemHeight<T> GetHeight { get; set; }
        public bool ShowSeparator { get; set; } = true;
        public string Header { get; set; } = null;

        private List<T> itemDatas = null;
        public SimpleListView(List<T> datas) : base(new TreeViewState())
        {
            itemDatas = datas;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            useScrollView = true;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            SimpleListViewItem<T> root = new SimpleListViewItem<T>(null);

            if (itemDatas != null && itemDatas.Count > 0)
            {
                foreach (var item in itemDatas)
                {
                    SimpleListViewItem<T> element = new SimpleListViewItem<T>(item);
                    root.AddChild(element);
                }
            }

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            Rect rect = args.rowRect;
            T itemData = (args.item as SimpleListViewItem<T>).ItemData;

            if(ShowSeparator)
            {
                rect.height -= 6.0f;
            }

            if(OnDrawItem == null)
            {
                EditorGUI.LabelField(rect,itemData.GetDisplayName());
            }else
            {
                OnDrawItem(rect, itemData);
            }

            if(ShowSeparator)
            {
                EGUI.DrawHorizontalLine(new Rect(rect.x, rect.y + rect.height, rect.width, 6.0f));
            }
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            float height = 0.0f;
            if(GetHeight == null)
            {
                height = EditorGUIUtility.singleLineHeight;
            }else
            {
                height = GetHeight((item as SimpleListViewItem<T>).ItemData);
            }
            if(ShowSeparator)
            {
                height += 6.0f;
            }
            return height;
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);

            if (selectedIds.Count > 0)
            {
                int selectedId = selectedIds[0];
                SimpleListViewItem<T> viewItem = FindItem(selectedId, rootItem) as SimpleListViewItem<T>;
                OnItemSelected?.Invoke(viewItem.ItemData);
            }
        }

        public override void OnGUI(Rect rect)
        {
            Rect viewRect = rect;
            if(!string.IsNullOrEmpty(Header))
            {
                EGUI.DrawBoxHeader(new Rect(rect.x, rect.y, rect.width, 30), Header);
                viewRect.y += 30;
                viewRect.height -= 30;
            }
            base.OnGUI(viewRect);
        }
    }
}
