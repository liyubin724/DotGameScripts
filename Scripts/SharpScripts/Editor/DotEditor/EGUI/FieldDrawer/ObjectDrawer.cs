using DotEditor.Core.EGUI;
using UnityEditor;
using SystemObject = System.Object;

namespace DotEditor.EGUI.FieldDrawer
{
    public class ObjectDrawer
    {
        private string title = string.Empty;
        private SystemObject data = null;
        private FieldData[] fieldDatas = null;

        public ObjectDrawer(string title,SystemObject data)
        {
            this.title = title;
            this.data = data;
            if(data!=null)
            {
                fieldDatas = FieldDrawerUtil.GetTypeFieldDrawer(data.GetType());
            }
        }
        
        public void OnGUI(bool isShowDesc = false)
        {
            EditorGUILayout.LabelField(title);

            EditorGUIUtil.BeginIndent();
            {
                if (data == null)
                {
                    EditorGUILayout.HelpBox("Data is null", MessageType.Error);
                }
                else
                {
                    foreach (var fieldData in fieldDatas)
                    {
                        if (fieldData.drawer == null)
                        {
                            EditorGUILayout.LabelField(fieldData.name, "Drawer is null");
                        }
                        else
                        {
                            fieldData.drawer.DrawField(data, isShowDesc);
                        }
                    }
                }
            }
            EditorGUIUtil.EndIndent();
        }
    }
}
