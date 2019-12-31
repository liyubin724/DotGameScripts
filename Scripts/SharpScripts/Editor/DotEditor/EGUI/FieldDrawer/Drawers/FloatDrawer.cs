using System.Reflection;
using UnityEditor;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(float))]
    public class FloatDrawer : AFieldDrawer
    {
        public FloatDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }

        protected override void OnDraw(bool isReadonly, bool isShowDesc)
        {
            float value = (float)fieldInfo.GetValue(data);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(isReadonly);
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        value = EditorGUILayout.FloatField(nameContent, value);
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        fieldInfo.SetValue(data, value);
                    }
                }
                EditorGUI.EndDisabledGroup();

                OnDrawAskOperation();
            }
            EditorGUILayout.EndHorizontal();

        }
    }
}
