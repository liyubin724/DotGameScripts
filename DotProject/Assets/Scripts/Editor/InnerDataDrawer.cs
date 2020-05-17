using DotEditor.NativeDrawer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

[CustomTypeDrawer(typeof(InnerData))]
public class InnerDataDrawer : NativeTypeDrawer
{
    public InnerDataDrawer(NativeDrawerProperty property) : base(property)
    {
    }

    protected override bool IsValidProperty()
    {
        return typeof(InnerData) == DrawerProperty.ValueType;
    }

    protected override void OnDrawProperty(string label)
    {
        InnerData data = DrawerProperty.GetValue<InnerData>();

        data.iValue = EditorGUILayout.IntField("FFFF", data.iValue);
        
    }
}