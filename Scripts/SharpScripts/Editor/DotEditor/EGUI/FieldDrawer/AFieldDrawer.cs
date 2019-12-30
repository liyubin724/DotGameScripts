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

        protected AFieldDrawer(FieldInfo fieldInfo)
        {
            this.fieldInfo = fieldInfo;

            descAttr = this.fieldInfo.GetCustomAttribute<FieldDesc>();
            isReadonly = this.fieldInfo.GetCustomAttribute<FieldReadonly>() != null;
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

                EditorGUILayout.HelpBox(descAttr.Desc, MessageType.Info);
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
                    GUILayout.Label(new GUIContent("?", descAttr.Desc),EditorStyles.miniButton,GUILayout.Width(16),GUILayout.Height(16));
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
