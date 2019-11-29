﻿using Dot.Core.TimeLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Core.TimeLine
{
    public class TrackLineEditor
    {
        public TrackGroupEditor Group { get; set; }
        
        private bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                if(isSelected)
                {
                    Group.OnTrackSelected(this);
                }else
                {
                    if(selectedItem!=null)
                    {
                        selectedItem.IsSelected = false;
                        selectedItem = null;
                    }
                }
                setting.isChanged = true;
            }
        }
        private ItemEditor selectedItem = null;
        public ItemEditor SelectedItem
        {
            get
            {
                return selectedItem;
            }

            private set
            {
                if (selectedItem != null && selectedItem != value)
                {
                    selectedItem.IsSelected = false;
                }
                selectedItem = value;
                if(selectedItem!=null && !selectedItem.IsSelected)
                {
                    selectedItem.IsSelected = true;
                }
            }
        }

        public TrackLine Track { get; private set; }
        private EditorSetting setting = null;
        private List<ItemEditor> items = new List<ItemEditor>();
        public TrackLineEditor(TrackLine tlTrack,EditorSetting setting)
        {
            Track = tlTrack;
            this.setting = setting;
            foreach(var item in tlTrack.items)
            {
                ItemEditor tleItem = new ItemEditor(item,setting);
                tleItem.Track = this;
                items.Add(tleItem);
            }
            tlTrack.items.Clear();
        }
        
        public void DrawElement(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x +1, rect.y + 1, rect.width - 2, rect.height - 2);
            GUI.Label(rectLabel, Track.Name, IsSelected ? "flow node 2" : "flow node 3");
            if (Event.current.type == EventType.MouseDown)
            {
                if (rectLabel.Contains(Event.current.mousePosition))
                {
                    IsSelected = true;
                    Event.current.Use();
                }
            }
        }

        public void DrawProperty()
        {
            GUILayout.Label("Track:");
            using (new UnityEditor.EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (var sope = new EditorGUI.ChangeCheckScope())
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        Track.Name = UnityEditor.EditorGUILayout.TextField("Name:", Track.Name);
                    }
                    if (sope.changed)
                        setting.isChanged = true;
                }
            }
            
            if (SelectedItem != null)
            {
                UnityEditor.EditorGUILayout.Space();
                SelectedItem.DrawProperty();
            }
        }

        public void DrawItem(Rect rect)
        {
            //GUI.Label(rect, "", IsSelected ? "flow node 5 on" : "flow node 5");
            for(var i =0;i<items.Count;i++)
            {
                items[i].DrawElement(rect);
            }

            if(Event.current.button == 1 && Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                GenericMenu menu = new GenericMenu();

                var fireTime = (Event.current.mousePosition.x + setting.scrollPos.x) / setting.pixelForSecond;
                foreach(var type in setting.ItemTypes)
                {
                    TimeLineMarkAttribute attr = type.GetCustomAttribute<TimeLineMarkAttribute>();
                    if (attr != null)
                        menu.AddItem(new GUIContent(attr.Category + "/" + attr.Label), false, ()=>
                        {
                            AItem item = (AItem)type.Assembly.CreateInstance(type.FullName);
                            item.FireTime = fireTime;
                            ItemEditor eItem = new ItemEditor(item, setting);
                            eItem.Track = this;
                            items.Add(eItem);

                            eItem.IsSelected = true;
                        });
                }

                menu.ShowAsContext();

                Event.current.Use();
            }
        }
        
        public void OnItemSelected(ItemEditor item)
        {
            GUI.FocusControl("");

            SelectedItem = item;
            IsSelected = true;
        }

        public void OnItemDelete(ItemEditor item)
        {
            items.Remove(item);
            if(SelectedItem == item)
            {
                SelectedItem = null;
            }
            setting.isChanged = true;
        }

        public void FillTrack()
        {
            Track.items.Clear();
            foreach(var item in items)
            {
                Track.items.Add(item.Item);
            }
            Track.items.Sort();
        }

        internal int[] GetDependOnItem(Type type) => (from item in items where item.Item.GetType() == type select item.Item.Index).ToArray();
    }
}
