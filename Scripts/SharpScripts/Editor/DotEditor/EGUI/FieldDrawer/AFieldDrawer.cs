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
        private FieldReadonly readonlyAttr = null;

        protected AFieldDrawer(FieldInfo fieldInfo)
        {
            this.fieldInfo = fieldInfo;

            descAttr = this.fieldInfo.GetCustomAttribute<FieldDesc>();
            readonlyAttr = this.fieldInfo.GetCustomAttribute<FieldReadonly>();
        }

        public void DrawField(SystemObject data,bool isShowDesc)
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

            EditorGUI.BeginDisabledGroup(readonlyAttr != null);
            {
                OnDraw(data,isShowDesc);
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

        protected abstract void OnDraw(SystemObject data, bool isShowDesc);
    }
}
