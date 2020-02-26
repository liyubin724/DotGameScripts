using DotEditor.Core.EGUI;
using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.Window
{
    public class DotPopupWindow : EditorWindow
    {
        public static T ShowPopupWin<T>(Rect position,bool isAutoClose) where T : DotPopupWindow
        {
            T win = EditorWindow.GetWindow<T>();
            win.minSize = position.size;
            win.position = position;

            win.AutoClose = isAutoClose;

            win.ShowPopup();

            win.Focus();

            return win;
        }

        public static DotPopupWindow ShowPopupWin(Rect position, Action<DotPopupWindow> drawElementCallback, Action closeCallback,bool isAutoClose)
        {
            var win = GetWindow<DotPopupWindow>();
            win.minSize = position.size;
            win.position = position;

            win.AutoClose = isAutoClose;
            win.drawElementCallback = drawElementCallback;
            win.closeCallback = closeCallback;

            win.ShowPopup();
            win.Focus();
            return win;
        }

        protected bool AutoClose { get; set; } = false;
        private Action<DotPopupWindow> drawElementCallback = null;
        private Action closeCallback = null;

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
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Space();
                DrawElement();
            }
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawElement() 
        {
            drawElementCallback?.Invoke(this);
        }


        private void OnDestroy()
        {
            closeCallback?.Invoke();
        }

        private void OnLostFocus()
        {
            if (AutoClose)
            {
                Close();
            }else
            {
                Focus();
            }
        }
    }
}
