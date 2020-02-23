﻿using DotEditor.Core.EGUI;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Core.Window
{
    public abstract class DotPopupWindow : EditorWindow
    {
        private bool isAutoClose = false;
        protected bool AutoClose { set => isAutoClose = value; }

        public static T GetPopupWindow<T>() where T : DotPopupWindow
        {
            var array = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];
            var t = (array.Length <= 0) ? null : array[0];

            return t ?? CreateInstance<T>();
        }

        public void Show<T>(Rect position, bool focus = true, bool autoClose = false) where T : DotPopupWindow
        {
            minSize = position.size;
            this.position = position;
            isAutoClose = autoClose;

            if (focus) Focus();

            ShowPopup();
        }

        protected virtual void DrawBackground()
        {
            Rect winRect = new Rect(Vector2.zero, position.size);
            EditorGUI.DrawRect(winRect, DotEditorGUI.BorderColor);

            Rect backgroundRect = new Rect(Vector2.one, position.size - new Vector2(2f, 2f));
            EditorGUI.DrawRect(backgroundRect, DotEditorGUI.BackgroundColor);
        }


        protected virtual void OnGUI()
        {
            DrawBackground();
        }

        private void OnLostFocus()
        {
            if (isAutoClose)
            {
                Close();
            }
        }
    }
}
