using Dot.FieldDrawer;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.EGUI.FieldDrawer
{
    public abstract class AFieldDrawer
    {
        protected FieldInfo fieldInfo = null;

        private FieldDesc descAttr = null;
        private bool isReadonly = false;

        protected GUIContent nameContent = null;
        protected AFieldDrawer(FieldInfo fieldInfo)
        {
            this.fieldInfo = fieldInfo;

            descAttr = this.fieldInfo.GetCustomAttribute<FieldDesc>();
            isReadonly = this.fieldInfo.GetCustomAttribute<FieldReadonly>() != null;

            if(descAttr!=null)
            {
                nameContent = new GUIContent(fieldInfo.Name, descAttr.BriefDesc);
            }else
            {
                nameContent = new GUIContent(fieldInfo.Name);
            }
        }

        protected SystemObject data;
        public virtual void SetData(SystemObject data)
        {
            this.data = data;
        }

        public void DrawField(bool isShowDesc)
        {
            bool isShowDescAsContent = descAttr != null && isShowDesc;
            bool isShowDescAsTip = descAttr != null && !isShowDescAsContent;

            if(isShowDescAsContent)
            {
                EditorGUILayout.BeginVertical();

                EditorGUILayout.HelpBox(descAttr.DetailDesc, MessageType.Info);
            }

            if(isShowDescAsTip)
            {
                EditorGUILayout.BeginHorizontal();
            }

            EditorGUI.BeginDisabledGroup(isReadonly);
            {
                OnDraw(isShowDesc);
            }
            EditorGUI.EndDisabledGroup();

            
            if(isShowDescAsTip)
            {
                if(descAttr!=null)
                {
                    GUIContent askBtn = new GUIContent("?");
                    Rect rect = GUILayoutUtility.GetRect(askBtn,EditorStyles.miniButton, GUILayout.Width(16), GUILayout.Height(16));
                    if(GUI.Button(rect,askBtn, EditorStyles.miniButton))
                    {
                        Rect position = GUIUtility.GUIToScreenRect(rect);
                        FieldDescPopWindow.ShowWin(position, fieldInfo.Name, descAttr.DetailDesc);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            if(isShowDescAsContent)
            {
                EditorGUILayout.EndVertical();
            }
        }

        protected abstract void OnDraw(bool isShowDesc);
    }
}
