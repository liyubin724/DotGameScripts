using DotEditor.GUIExtension;
using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Core.Window
{
    public class PopupWindow : EditorWindow
    {
        public static T ShowPopupWin<T>(Rect position,bool isAutoClose) where T : PopupWindow
        {
            T win = EditorWindow.GetWindow<T>();
            win.minSize = position.size;
            win.position = position;

            win.AutoClose = isAutoClose;

            win.ShowPopup();

            win.Focus();

            return win;
        }

        public static PopupWindow ShowPopupWin(Rect position, Action<PopupWindow> drawElementCallback, Action closeCallback,bool isAutoClose)
        {
            var win = GetWindow<PopupWindow>();
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
        private Action<PopupWindow> drawElementCallback = null;
        private Action closeCallback = null;

        protected virtual void DrawBackground()
        {
            Rect winRect = new Rect(Vector2.zero, position.size);
            EditorGUI.DrawRect(winRect, EGUIResources.BorderColor);

            Rect backgroundRect = new Rect(Vector2.one, position.size - new Vector2(2f, 2f));
            EditorGUI.DrawRect(backgroundRect, EGUIResources.BackgroundColor);
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
