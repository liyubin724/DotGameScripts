using Dot.NativeDrawer.Decorator;
using Dot.NativeDrawer.Layout;
using Dot.NativeDrawer.Property;
using Dot.NativeDrawer.Verification;
using Dot.NativeDrawer.Visible;
using Dot.Utilities;
using DotEditor.GUIExtension;
using DotEditor.NativeDrawer.Decorator;
using DotEditor.NativeDrawer.DefaultTypeDrawer;
using DotEditor.NativeDrawer.Layout;
using DotEditor.NativeDrawer.Property;
using DotEditor.NativeDrawer.Verification;
using DotEditor.NativeDrawer.Visible;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DotEditor.NativeDrawer
{
    public class NativeDrawerProperty
    {
        public object Target { get; private set; }
        public FieldInfo Field { get; private set; }
        
        public Type ValueType
        {
            get
            {
                return Field.FieldType;
            }
        }

        public object Value
        {
            get
            {
                object value = Field.GetValue(Target);
                if(value == null)
                {
                    value = NativeDrawerUtility.CreateDefaultInstance(ValueType);
                    Field.SetValue(Target, value);
                }

                return value;
            }
            set
            {
                if(value == null)
                {
                    value = NativeDrawerUtility.CreateDefaultInstance(ValueType); 
                }
                Field.SetValue(Target, value);
            }
        }

        internal T GetValue<T>()
        {
            return (T)Value;
        }

        private List<DecoratorDrawer> decoratorDrawers = new List<DecoratorDrawer>();
        private List<LayoutDrawer> layoutDrawers = new List<LayoutDrawer>();
        private List<VerificationDrawer> verificationDrawers = new List<VerificationDrawer>();
        private List<VisibleDrawer> visibleDrawers = new List<VisibleDrawer>();
        private List<VisibleCompareDrawer> visibleCompareDrawers = new List<VisibleCompareDrawer>();

        private List<PropertyLabelDrawer> propertyLabelDrawers = new List<PropertyLabelDrawer>();
        private List<PropertyControlDrawer> propertyControlDrawers = new List<PropertyControlDrawer>();
        private List<PropertyDrawer> propertyDrawers = new List<PropertyDrawer>();

        private NativeTypeDrawer typeDrawer = null;
        internal NativeDrawerProperty(object propertyObject,FieldInfo field)
        {
            Target = propertyObject;
            Field = field;
        }

        internal void Init()
        {
            InitFieldAttr();

            typeDrawer = NativeDrawerUtility.CreateDefaultTypeDrawer(this);
        }

        private void InitFieldAttr()
        {
            var decoratorAttrEnumerable = Field.GetCustomAttributes<DecoratorAttribute>();
            foreach (var attr in decoratorAttrEnumerable)
            {
                decoratorDrawers.Add(NativeDrawerUtility.CreateDecoratorDrawer(attr));
            }

            var layoutAttrEnumerable = Field.GetCustomAttributes<LayoutAttribute>();
            foreach (var attr in layoutAttrEnumerable)
            {
                layoutDrawers.Add(NativeDrawerUtility.CreateLayoutDrawer(attr));
            }

            var verificationAttrEnumerable = Field.GetCustomAttributes<VerificationCompareAttribute>();
            foreach (var attr in verificationAttrEnumerable)
            {
                verificationDrawers.Add(NativeDrawerUtility.CreateVerificationDrawer(Target, attr));
            }

            var visibleAttrEnumerable = Field.GetCustomAttributes<VisibleAtrribute>();
            foreach (var attr in visibleAttrEnumerable)
            {
                visibleDrawers.Add(NativeDrawerUtility.CreateVisibleDrawer(attr));
            }

            var visibleCompareAttrEnumerable = Field.GetCustomAttributes<VisibleCompareAttribute>();
            foreach (var attr in visibleCompareAttrEnumerable)
            {
                visibleCompareDrawers.Add(NativeDrawerUtility.CreateVisibleCompareDrawer(Target, attr));
            }

            var propertyLabelAttrEnumerable = Field.GetCustomAttributes<PropertyLabelAttribute>();
            foreach (var attr in propertyLabelAttrEnumerable)
            {
                propertyLabelDrawers.Add(NativeDrawerUtility.CreatePropertyLabelDrawer(attr));
            }

            var propertyControlAttrEnumerable = Field.GetCustomAttributes<PropertyControlAttribute>();
            foreach (var attr in propertyControlAttrEnumerable)
            {
                propertyControlDrawers.Add(NativeDrawerUtility.CreatePropertyControlDrawer(attr));
            }

            var propertyAttrEnumerable = Field.GetCustomAttributes<PropertyDrawerAttribute>();
            foreach (var attr in propertyAttrEnumerable)
            {
                propertyDrawers.Add(NativeDrawerUtility.CreatePropertyDrawer(Target, Field, attr));
            }
        }

        internal void OnGUILayout()
        {
            bool isVisible = IsVisible();

            foreach (var drawer in layoutDrawers)
            {
                drawer.OnLayoutGUI();
            }

            if (isVisible)
            {
                foreach (var drawer in decoratorDrawers)
                {
                    drawer.OnLayoutGUI();
                }

                foreach (var drawer in verificationDrawers)
                {
                    drawer.OnLayoutGUI();
                }

                foreach (var drawer in propertyControlDrawers)
                {
                    drawer.OnLayoutGUIStart();
                }

                string label = GetFieldLabel();
                if(!string.IsNullOrEmpty(label))
                {
                    label = UnityEditor.ObjectNames.NicifyVariableName(label);
                }
                if (propertyDrawers.Count == 0)
                {
                    if(typeDrawer !=null)
                    {
                        typeDrawer.OnGUILayout(label);
                    }else
                    {
                        EGUI.BeginGUIColor(Color.red);
                        {
                            UnityEditor.EditorGUILayout.LabelField(string.IsNullOrEmpty(label) ? "" : label, "Unknown Drawer");
                        }
                        EGUI.EndGUIColor();
                    }
                }
                else
                {
                    propertyDrawers[0].OnLayoutGUI(label);
                }

                foreach (var drawer in propertyControlDrawers)
                {
                    drawer.OnLayoutGUIEnd();
                }
            }
        }

        private bool IsVisible()
        {
            if (Field == null)
            {
                return true;
            }

            bool visible = Field.IsPublic;
            if (visibleDrawers.Count > 0)
            {
                visible = visibleDrawers[0].IsVisible();
            }
            else if (visibleCompareDrawers.Count > 0)
            {
                visible = visibleCompareDrawers[0].IsVisible();
            }
            return visible;
        }

        private string GetFieldLabel()
        {
            string label = Field?.Name;
            foreach (var drawer in propertyLabelDrawers)
            {
                label = drawer.GetLabel();
            }
            return label ?? "";
        }

        internal void ClearArrayElement()
        {
            if (TypeUtility.IsArrayOrList(ValueType))
            {
                if (ValueType.IsArray)
                {
                    Value = NativeDrawerUtility.CreateDefaultInstance(ValueType);
                }
                else
                {
                    ((IList)Value).Clear();
                }
            }
        }

        internal void AddArrayElement()
        {
            if (TypeUtility.IsArrayOrList(ValueType))
            {
                object element = NativeDrawerUtility.CreateDefaultInstance(TypeUtility.GetArrayOrListElementType(ValueType));
                if (ValueType.IsArray)
                {
                    Array array = (Array)Value;
                    ArrayUtility.Add(ref array,element);

                    Value = array;
                }
                else
                {
                    ((IList)Value).Add(element);
                }
            }
        }

        internal void RemoveArrayElementAtIndex(int index)
        {
            if(TypeUtility.IsArrayOrList(ValueType))
            {
                if(ValueType.IsArray)
                {
                    Array array = (Array)Value;
                    ArrayUtility.Remove(ref array, index);

                    Value = array;
                }
                else
                {
                    ((IList)Value).RemoveAt(index);
                }
            }
        }
    }
}
