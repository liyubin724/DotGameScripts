using DotEditor.Core.Utilities;
using DotEditor.GUIExtension;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer
{
    public class NativeTypeData
    {
        public Type type;
        public List<NativeInfo> fields = new List<NativeInfo>();
    }

    public class NativeObjectInfo : NativeInfo
    {
        private List<NativeTypeData> allTypeFields = new List<NativeTypeData>();
        private bool isFoldout = false;

        public bool IsRoot 
        { 
            get
            {
                return Field == null;
            } 
        }

        public NativeObjectInfo(object target):this(target,null)
        {
        }

        public NativeObjectInfo(object target,FieldInfo field) : base(target,field)
        {
            InitField();
        }

        public override void OnLayoutGUI()
        {
            if(!IsRoot)
            {
                isFoldout = EditorGUILayout.Foldout(isFoldout, Field.Name, true);
                if(!isFoldout)
                {
                    EditorGUI.indentLevel++;
                }
            }

            foreach(var d in allTypeFields)
            {
                EGUILayout.DrawBoxHeader(d.type.Name,GUILayout.ExpandWidth(true));
                foreach(var field in d.fields)
                {
                    field.OnLayoutGUI();
                }
                EGUILayout.DrawHorizontalLine();
            }

            if(!IsRoot && !isFoldout)
            {
                EditorGUI.indentLevel--;
            }
        }

        private void InitField()
        {
            Type[] allTypes = TypeUtility.GetAllBasedTypes(ValueType,typeof(object));
            if(allTypes!=null)
            {
                foreach(var type in allTypes)
                {
                    NativeTypeData data = new NativeTypeData() { type = type };
                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    foreach(var field in fields)
                    {
                        if(NativeDrawerUtility.IsValueType(field.FieldType))
                        {
                            data.fields.Add(new NativeValueInfo(Value, field));
                        }else
                        {
                            if(Value == null)
                            {
                                Value = Activator.CreateInstance(ValueType);
                            }
                            data.fields.Add(new NativeObjectInfo(Value, field));
                        }
                    }
                    allTypeFields.Add(data);
                }
            }
        }
    }
}
