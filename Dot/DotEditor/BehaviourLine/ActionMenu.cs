using DotEngine.BehaviourLine.Action;
using DotEngine.Utilities;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class ActionMenu
    {
        private GenericMenu menu = null;
        private Action<ActionData> createdCallback = null;
        
        public ActionMenu()
        {
        }

        public void ShowMenu(Action<ActionData> callback)
        {
            createdCallback = callback;

            if(menu == null)
            {
                menu = new GenericMenu();
                CreateMenuItem();
            }

            menu.ShowAsContext();
        }

        private void CreateMenuItem()
        {
            Type[] dataTypes = AssemblyUtility.GetDerivedTypes(typeof(ActionData));
            if(dataTypes == null || dataTypes.Length == 0)
            {
                return;
            }

            foreach(var dataType in dataTypes)
            {
                ActionMenuAttribute attr = dataType.GetCustomAttribute<ActionMenuAttribute>();
                if(attr == null)
                {
                    continue;
                }
                menu.AddItem(new GUIContent($"{attr.Prefix}/{attr.Name}"),false,()=>
                {
                    createdCallback?.Invoke((ActionData)Activator.CreateInstance(dataType));
                });
            }
        }

    }
}
