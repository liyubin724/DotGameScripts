﻿using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI
{
    public class EGUIToolbarSearchField
    {
        private static readonly int FIELD_WIDTH = 150;
        private static readonly int FIELD_HEIGHT = 16;

        public string Text { get; set; } = string.Empty;
        public string[] Categories { get; set; } = new string[0];
        public int CategoryIndex { get; set; } = -1;

        private Action<string> onTextChanged = null;
        private Action<string> onCategoryChagned = null;

        public EGUIToolbarSearchField(Action<string> textChangedCallback,Action<string> categoryChangedCallback)
        {
            onTextChanged = textChangedCallback;
            onCategoryChagned = categoryChangedCallback;
        }

        public void OnGUI(Rect rect)
        {
            int categoryIndex = CategoryIndex;
            if(Categories == null || Categories.Length == 0)
            {
                if(CategoryIndex>=0)
                {
                    categoryIndex = -1;
                }
            }else
            {
                if(CategoryIndex<0 || CategoryIndex>=Categories.Length)
                {
                    categoryIndex = 0;
                }
            }
            if(categoryIndex!=CategoryIndex)
            {
                CategoryIndex = categoryIndex;
                if(CategoryIndex>=0)
                {
                    onCategoryChagned?.Invoke(Categories[CategoryIndex]);
                }
            }

            Rect textFieldRect;
            if (Categories == null || Categories.Length == 0)
            {
                textFieldRect = new Rect(rect.x, rect.y + 2, rect.width - 16, 14);
            }else
            {
                textFieldRect = new Rect(rect.x + 32, rect.y + 2, rect.width - 48, 14);
            }
            string searchText = GUI.TextField(textFieldRect, Text, "toolbarSeachTextField");
            if (searchText != Text)
            {
                Text = searchText;
                onTextChanged?.Invoke(Text);
            }

            if(Categories!=null && Categories.Length>0)
            {
                Rect popRect = new Rect(rect.x, rect.y + 2, 48, 16);
                int index = EditorGUI.Popup(popRect, "", CategoryIndex, Categories, "ToolbarSeachTextFieldPopup");
                if(index != CategoryIndex)
                {
                    CategoryIndex = index;

                    onCategoryChagned?.Invoke(Categories[CategoryIndex]);
                }
            }

            Rect cancelRect = new Rect(rect.x + rect.width - 16, rect.y + 2, 16, 14);
            if (GUI.Button(cancelRect, "", "ToolbarSeachCancelButton"))
            {
                if(Text!= "")
                {
                    Text = "";
                    onTextChanged?.Invoke(Text);
                }
            }
        }

        public void OnGUILayout()
        {
            Rect rect = GUILayoutUtility.GetRect(FIELD_WIDTH, FIELD_HEIGHT,"toolbar");
            OnGUI(rect);
        }

    }
}
