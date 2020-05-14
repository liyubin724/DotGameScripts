using Dot.NativeDrawer.Decorator;
using Dot.NativeDrawer.Layout;
using Dot.NativeDrawer.Property;
using Dot.NativeDrawer.Verification;
using Dot.NativeDrawer.Visible;
using DotEditor.Core.Utilities;
using DotEditor.NativeDrawer.Decorator;
using DotEditor.NativeDrawer.Layout;
using DotEditor.NativeDrawer.Property;
using DotEditor.NativeDrawer.Verification;
using DotEditor.NativeDrawer.Visible;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DotEditor.NativeDrawer
{
    public class NativeDrawerProperty
    {
        public object Target { get; private set; }
        public FieldInfo Field { get; private set; }
        public bool IsArrayOrList
        {
            get
            {
                return TypeUtility.IsArrayOrList(ValueType);
            }
        }
        public int IndexOfArrayOrList { get; private set; } = -1;
        public bool IsStructOrClass
        {
            get
            {
                return TypeUtility.IsStructOrClass(ValueType);
            }
        }

        public Type ValueType
        {
            get
            {
                Type valueType = Field.FieldType;
                if(TypeUtility.IsArrayOrList(valueType) && IndexOfArrayOrList>0)
                {
                    return TypeUtility.GetArrayOrListElementType(valueType);
                }
                return valueType;
            }
        }

        public object Value
        {
            get
            {
                var value = Field.GetValue(Target);
                if(IsArrayOrList && IndexOfArrayOrList >0)
                {
                    if(value == null)
                    {
                        return null;
                    }else
                    {
                        IList list = (IList)value;
                        return list[IndexOfArrayOrList];
                    }
                }
                return value;
            }
            set
            {
                if(IsArrayOrList && IndexOfArrayOrList>0)
                {
                    IList list = (IList)value;
                    list[IndexOfArrayOrList] = value;
                }
                else
                {
                    if(value == null)
                    {
                        value = Activator.CreateInstance(ValueType);
                    }
                    Field.SetValue(Target, value);
                }
            }
        }

        private List<NativeDrawerProperty> childProperties = new List<NativeDrawerProperty>();

        private List<DecoratorDrawer> decoratorDrawers = new List<DecoratorDrawer>();
        private List<LayoutDrawer> layoutDrawers = new List<LayoutDrawer>();
        private List<VerificationDrawer> verificationDrawers = new List<VerificationDrawer>();
        private List<VisibleDrawer> visibleDrawers = new List<VisibleDrawer>();
        private List<VisibleCompareDrawer> visibleCompareDrawers = new List<VisibleCompareDrawer>();

        private List<PropertyLabelDrawer> propertyLabelDrawers = new List<PropertyLabelDrawer>();
        private List<PropertyControlDrawer> propertyControlDrawers = new List<PropertyControlDrawer>();
        private List<PropertyDrawer> propertyDrawers = new List<PropertyDrawer>();

        internal NativeDrawerProperty(object propertyObject,FieldInfo field)
        {
            Target = propertyObject;
            Field = field;
        }

        
        private void InitChildProperty()
        {
            
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
                if (propertyDrawers.Count == 0)
                {
                    
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
    }
}
