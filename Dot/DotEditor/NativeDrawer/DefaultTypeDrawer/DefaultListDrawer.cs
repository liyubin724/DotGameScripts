using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(IList))]
    public class DefaultListDrawer : NativeTypeDrawer
    {
        private IList list = null;
        private ReorderableList rList = null;
        public DefaultListDrawer(object target, FieldInfo field) : base(target, field)
        {
            
        }

        protected override bool IsValid()
        {
            return typeof(IList).IsAssignableFrom(ValueType);
        }

        protected override void OnDraw(string label)
        {
            if(rList == null)
            {
                list = GetValue<IList>();
                if(list == null)
                {
                    Value = Activator.CreateInstance(ValueType);
                    list = GetValue<IList>();
                }

                rList = new ReorderableList(list, ValueType.GenericTypeArguments[0], true, true, true, true);
                rList.drawHeaderCallback = (rect) =>
                {
                    EditorGUI.LabelField(rect,Field.Name);
                };
                rList.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    
                };
            }
        }
    }
}
