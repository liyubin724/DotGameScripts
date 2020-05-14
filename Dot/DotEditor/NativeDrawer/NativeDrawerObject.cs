using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotEditor.NativeDrawer
{
    public class NativeDrawerObject
    {
        private object drawerObject;
        private List<NativeDrawerProperty> drawerProperties = new List<NativeDrawerProperty>();

        public NativeDrawerObject(object obj)
        {
            drawerObject = obj;

            InitField();
        }

        private void InitField()
        {
            Type[] allTypes = NativeDrawerUtility.GetAllBaseTypes(drawerObject.GetType());
            if(allTypes!=null)
            {
                foreach (var type in allTypes)
                {
                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (var field in fields)
                    {
                        NativeDrawerProperty drawerProperty = new NativeDrawerProperty(drawerObject, field);
                        drawerProperty.Init();

                        drawerProperties.Add(drawerProperty);
                    }
                }
            }
        }

        public void OnGUILayout()
        {
            foreach(var property in drawerProperties)
            {
                property.OnGUILayout();
            }
        }
    }
}
