using Dot.FieldDrawer.Attributes;
using DotEditor.EGUI.FieldDrawer.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityObject = UnityEngine.Object;
using SystemObject = System.Object;

namespace DotEditor.EGUI.FieldDrawer
{
    public class EGUIObjectDrawer
    {
        private object m_Data;
        private bool m_IsShowDesc = false;
        private List<AEGUIFieldDrawer> fieldDrawers = new List<AEGUIFieldDrawer>();

        public bool IsShowDesc
        {
            set
            {
                m_IsShowDesc = value;
                foreach(var fieldDrawer in fieldDrawers)
                {
                    fieldDrawer.IsShowDesc = value;
                }
            }
        }

        private bool IsChanged { get; set; }

        public EGUIObjectDrawer(object obj)
        {
            m_Data = true;

            FindFields();
        }

        public EGUIObjectDrawer(object obj,bool isShowDesc)
        {
            m_Data = obj;
            m_IsShowDesc = isShowDesc;

            FindFields();
        }

        private void FindFields()
        {
            fieldDrawers.Clear();

            var fieldDrawerTypes = from type in typeof(AEGUIFieldDrawer).Assembly.GetTypes() where type.IsSubclassOf(typeof(AEGUIFieldDrawer)) select type;
            Dictionary<Type, Type> fieldDrawerDic = new Dictionary<Type, Type>();
            foreach(var fdType in fieldDrawerTypes)
            {
                var attr = fdType.GetCustomAttribute<FieldDrawerType>();
                if(attr!=null)
                {
                    fieldDrawerDic.Add(attr.DrawerType, fdType);
                }
            }

            var fields = m_Data.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                Type fieldType = field.FieldType;
                if(fieldType.IsEnum)
                {
                    fieldType = typeof(Enum);
                }else if(fieldType.IsValueType && !fieldType.IsPrimitive)
                {
                    fieldType = typeof(SystemObject);
                }else if(fieldType.IsArray)
                {
                    fieldType = typeof(IList);
                }
                else if(fieldType.IsClass && fieldType != typeof(string))
                {
                    if(fieldType == typeof(UnityObject) || fieldType.IsSubclassOf(typeof(UnityObject)))
                    {
                        fieldType = typeof(UnityObject);
                    }else
                    {
                        fieldType = typeof(SystemObject);
                    }
                }
                if(fieldDrawerDic.TryGetValue(fieldType,out Type drawerType))
                {
                    var drawer = Activator.CreateInstance(drawerType, m_Data, field, m_IsShowDesc);
                    fieldDrawers.Add((AEGUIFieldDrawer)drawer);
                }
            }
            fieldDrawers.Sort((item1, item2) =>
            {
                FieldOrder fOrder1 = item1.FieldInfo.GetCustomAttribute<FieldOrder>();
                FieldOrder fOrder2 = item2.FieldInfo.GetCustomAttribute<FieldOrder>();

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
                if (mDesc != null && m_IsShowDesc)
                {
                    EditorGUILayout.HelpBox(mDesc.Desc, MessageType.Info);
                }

                EditorGUI.BeginChangeCheck();
                {
                    foreach (var drawer in fieldDrawers)
                    {
                        FieldHide fHideAttr = drawer.FieldInfo.GetCustomAttribute<FieldHide>();
                        FieldShow fShowAttr = drawer.FieldInfo.GetCustomAttribute<FieldShow>();

                        if (drawer.FieldInfo.IsPublic)
                        {
                            if (fHideAttr == null)
                            {
                                drawer.OnGUILayout();
                            }
                        }
                        else
                        {
                            if (fShowAttr != null)
                            {
                                drawer.OnGUILayout();
                            }
                        }
                    }
                }
                if (EditorGUI.EndChangeCheck())
                {
                    IsChanged = true;
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
