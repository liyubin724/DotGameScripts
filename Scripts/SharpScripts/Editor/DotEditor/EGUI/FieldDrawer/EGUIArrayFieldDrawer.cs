using DotEditor.EGUI.FieldDrawer.Attributes;
using System;
using System.Collections;
using System.Reflection;
using UnityEditorInternal;

namespace DotEditor.EGUI.FieldDrawer
{
    [FieldDrawerType(typeof(IList))]
    public class EGUIArrayFieldDrawer : AEGUIFieldDrawer
    {
        public EGUIArrayFieldDrawer(object data, FieldInfo fieldInfo, bool isShowDesc) : base(data, fieldInfo, isShowDesc)
        {
        }

        protected override object DrawField()
        {
            var value = m_FieldInfo.GetValue(m_Data);
            if(value == null)
            {

                return value;
            }else
            {

            }
            
            return null;    
        }
    }
}
