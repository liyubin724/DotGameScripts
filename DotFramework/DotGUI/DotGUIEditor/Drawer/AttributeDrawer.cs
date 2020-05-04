using Dot.GUI.Drawer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.EGUI.Drawer
{
    public abstract class AttributeDrawer 
    {
        public object Data { get; private set; }
        public FieldInfo Field { get; private set; }
        public DrawerAttribute Attr { get; private set; }

        public T GetAttr<T>() where T:DrawerAttribute
        {
            return (T)Attr;
        }    

        protected AttributeDrawer(object data,FieldInfo field,DrawerAttribute attr)
        {
            Data = data;
            Field = field;
            Attr = attr;
        }
    }

    public abstract class DecoratorDrawer : AttributeDrawer
    {
        protected DecoratorDrawer(object data, FieldInfo field, DrawerAttribute attr) : base(data, field, attr)
        {
        }

        public abstract void DoLayoutGUI();
    }

    public abstract class LayoutDrawer : AttributeDrawer
    {
        protected LayoutDrawer(object data, FieldInfo field, DrawerAttribute attr) : base(data, field, attr)
        {
        }

        public abstract void DoBeginLayoutGUI();
        public abstract void DoEndLayoutGUI();
    }

    public abstract class VerificationDrawer : AttributeDrawer
    {
        protected VerificationDrawer(object data, FieldInfo field) : base(data, field)
        {
        }

        public abstract bool IsValid();
    }

    public abstract class VisibleDrawer : AttributeDrawer
    {
        protected VisibleDrawer(object data, FieldInfo field) : base(data, field)
        {
        }

        public abstract bool IsVisible();
    }


}
