using Dot.NativeDrawer.Decorator;
using Dot.NativeDrawer.Layout;
using Dot.NativeDrawer.Property;
using Dot.NativeDrawer.Verification;
using Dot.NativeDrawer.Visible;
using DotEditor.NativeDrawer.Decorator;
using DotEditor.NativeDrawer.DefaultTypeDrawer;
using DotEditor.NativeDrawer.Layout;
using DotEditor.NativeDrawer.Property;
using DotEditor.NativeDrawer.Verification;
using DotEditor.NativeDrawer.Visible;
using System.Collections.Generic;
using System.Reflection;

namespace DotEditor.NativeDrawer
{
    public class NativeValueInfo : NativeInfo
    {
        private List<DecoratorDrawer> decoratorDrawers = new List<DecoratorDrawer>();
        private List<LayoutDrawer> layoutDrawers = new List<LayoutDrawer>();
        private List<VerificationDrawer> verificationDrawers = new List<VerificationDrawer>();
        private List<VisibleDrawer> visibleDrawers = new List<VisibleDrawer>();
        private List<VisibleCompareDrawer> visibleCompareDrawers = new List<VisibleCompareDrawer>();

        private List<PropertyLabelDrawer> propertyLabelDrawers = new List<PropertyLabelDrawer>();
        private List<PropertyControlDrawer> propertyControlDrawers = new List<PropertyControlDrawer>();
        private List<PropertyDrawer> propertyDrawers = new List<PropertyDrawer>();

        private NativeTypeDrawer defaultTypeDrawer = null;

        public NativeValueInfo(object target,FieldInfo field) : base(target,field)
        {
            InitDrawers();
        }

        public override void OnLayoutGUI()
        {
            bool isVisible = IsVisible();

            foreach(var drawer in layoutDrawers)
            {
                drawer.OnLayoutGUI();
            }

            if(isVisible)
            {
                foreach(var drawer in decoratorDrawers)
                {
                    drawer.OnLayoutGUI();
                }

                foreach(var drawer in verificationDrawers)
                {
                    drawer.OnLayoutGUI();
                }

                foreach(var drawer in propertyControlDrawers)
                {
                    drawer.OnLayoutGUIStart();
                }

                string label = Field.Name;
                foreach(var drawer in propertyLabelDrawers)
                {
                    label = drawer.GetLabel();
                }

                if(propertyDrawers.Count == 0)
                {
                    if(defaultTypeDrawer == null)
                    {
                        defaultTypeDrawer = NativeDrawerUtility.CreateDefaultTypeDrawer(Target, Field);
                    }
                    if(defaultTypeDrawer!=null)
                    {
                        defaultTypeDrawer.OnLayoutGUI(label);
                    }
                }else
                {
                    propertyDrawers[0].OnLayoutGUI(label);
                }

                foreach (var drawer in propertyControlDrawers)
                {
                    drawer.OnLayoutGUIEnd();
                }

            }
        }

        private void InitDrawers()
        {
            var decoratorAttrEnumerable = Field.GetCustomAttributes<DecoratorAttribute>();
            foreach(var attr in decoratorAttrEnumerable)
            {
                decoratorDrawers.Add(NativeDrawerUtility.CreateDecoratorDrawer(attr));
            }

            var layoutAttrEnumerable = Field.GetCustomAttributes<LayoutAttribute>();
            foreach(var attr in layoutAttrEnumerable)
            {
                layoutDrawers.Add(NativeDrawerUtility.CreateLayoutDrawer(attr));
            }

            var verificationAttrEnumerable = Field.GetCustomAttributes<VerificationCompareAttribute>();
            foreach(var attr in verificationAttrEnumerable)
            {
                verificationDrawers.Add(NativeDrawerUtility.CreateVerificationDrawer(Target, attr));
            }

            var visibleAttrEnumerable = Field.GetCustomAttributes<VisibleAtrribute>();
            foreach(var attr in visibleAttrEnumerable)
            {
                visibleDrawers.Add(NativeDrawerUtility.CreateVisibleDrawer(attr));
            }

            var visibleCompareAttrEnumerable = Field.GetCustomAttributes<VisibleCompareAttribute>();
            foreach(var attr in visibleCompareAttrEnumerable)
            {
                visibleCompareDrawers.Add(NativeDrawerUtility.CreateVisibleCompareDrawer(Target, attr));
            }

            var propertyLabelAttrEnumerable = Field.GetCustomAttributes<PropertyLabelAttribute>();
            foreach(var attr in propertyLabelAttrEnumerable)
            {
                propertyLabelDrawers.Add(NativeDrawerUtility.CreatePropertyLabelDrawer(attr));
            }

            var propertyControlAttrEnumerable = Field.GetCustomAttributes<PropertyControlAttribute>();
            foreach(var attr in propertyControlAttrEnumerable)
            {
                propertyControlDrawers.Add(NativeDrawerUtility.CreatePropertyControlDrawer(attr));
            }

            var propertyAttrEnumerable = Field.GetCustomAttributes<PropertyDrawerAttribute>();
            foreach(var attr in propertyAttrEnumerable)
            {
                propertyDrawers.Add(NativeDrawerUtility.CreatePropertyDrawer(Target, Field, attr));
            }    
        }

        private bool IsVisible()
        {
            bool visible = Field.IsPublic;
            if(visibleDrawers.Count>0)
            {
                visible = visibleDrawers[0].IsVisible();
            }else if(visibleCompareDrawers.Count>0)
            {
                visible = visibleCompareDrawers[0].IsVisible();
            }
            return visible;
        }
    }
}
