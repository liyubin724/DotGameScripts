using Dot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal;

namespace DotEditor.EGUI
{
    public class EGUIObjectField
    {
        private object m_Data = null;
        private FieldInfo m_Field = null;
        private bool m_ShowDesc = false;

        private bool m_IsFoldout = true;
        private EGUIObjectDrawer objDrawer = null;
        private ReorderableList listDrawer = null;

        public EGUIObjectField(object data,FieldInfo field,bool showDesc)
        {
            m_Data = data;
            m_Field = field;
            m_ShowDesc = showDesc;
        }

        public Type FieldType { get => m_Field.FieldType; }



        public void OnGUILayout()
        {
            var value = m_Field.GetValue(m_Data);

            MemberDesc memberDesc = FieldType.GetCustomAttribute<MemberDesc>();
            if (memberDesc != null && m_ShowDesc)
            {
                EditorGUILayout.HelpBox(memberDesc.Desc, MessageType.Info);
            }

            object newValue = null;
            if(typeof(int) == FieldType)
            {
                newValue = EditorGUILayout.IntField(value == null ? 0 : (int)value);
            }else if(typeof(float) == FieldType)
            {
                newValue = EditorGUILayout.FloatField(value == null?0.0f:(float)value);
            }else if(typeof(string) == FieldType)
            {
                FieldMultilineText fmText = FieldType.GetCustomAttribute<FieldMultilineText>();
                if(fmText == null)
                {
                    newValue = EditorGUILayout.TextField(value == null ? "" : (string)value);
                }
            }else if(FieldType.IsEnum)
            {
                Enum e = (Enum)value;
                newValue = EditorGUILayout.EnumPopup(e);
            }else if(FieldType.IsArray)
            {

            }else if(FieldType.IsClass)
            {

            }
        }
    }


    public class EGUIObjectDrawer
    {
        private object m_Data;
        private List<EGUIObjectField> m_Fields = new List<EGUIObjectField>();

        public bool ShowDesc { get; set; } = false;

        public EGUIObjectDrawer(object obj)
        {
            m_Data = obj;
        }

        private void FindFields()
        {
            m_Fields.Clear();

            var fields = m_Data.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                FieldHide fHideAttr = field.GetCustomAttribute<FieldHide>();
                FieldShow fShowAttr = field.GetCustomAttribute<FieldShow>();
                if ((field.IsPublic && fHideAttr == null) || (!field.IsPublic&&fShowAttr!=null))
                {
                    m_Fields.Add(new EGUIObjectField(m_Data, field, ShowDesc));
                }
            }
            m_Fields.Sort((item1, item2) =>
            {
                FieldOrder fOrder1 = item1.FieldType.GetCustomAttribute<FieldOrder>();
                FieldOrder fOrder2 = item2.FieldType.GetCustomAttribute<FieldOrder>();

                int order1 = fOrder1 == null ? 0 : fOrder1.Order;
                int order2 = fOrder2 == null ? 0 : fOrder2.Order;
                return order1.CompareTo(order2);
            });
        }

        public void OnGUILayout()
        {
            Type dataType = m_Data.GetType();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                MemberDesc mDesc = dataType.GetCustomAttribute<MemberDesc>();
                if(mDesc!=null && ShowDesc)
                {
                    EditorGUILayout.HelpBox(mDesc.Desc,MessageType.Info);
                }

                foreach(var field in m_Fields)
                {
                    field.OnGUILayout();
                }
            }
            EditorGUILayout.EndVertical();

        }

    }
}
