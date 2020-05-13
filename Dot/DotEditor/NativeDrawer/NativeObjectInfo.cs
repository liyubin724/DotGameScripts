using DotEditor.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

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
        private bool isFoldout = true;

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

        protected override void OnDrawerProperty()
        {
            if(!IsRoot)
            {
                isFoldout = EditorGUILayout.Foldout(isFoldout, Field.Name, true);
                if(isFoldout)
                {
                    EditorGUI.indentLevel++;
                }
            }

            if(isFoldout)
            {
                foreach(var d in allTypeFields)
                {
                    foreach(var field in d.fields)
                    {
                        field.OnLayoutGUI();
                    }
                }
            }

            if(!IsRoot && isFoldout)
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
                            data.fields.Add(new NativeInfo(Value, field));
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
