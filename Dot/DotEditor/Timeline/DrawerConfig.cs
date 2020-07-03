using DotEngine.Timeline.Data;
using DotEngine.Utilities;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Timeline
{
    public class DrawerConfig
    {
        public float TimeLength { get; set; } = 5.0f;

        public float TimeStep { get; set; } = 0.05f;
        public int TrackLineHeight { get; set; } = 60;
        public int WidthForSecond { get; set; } = 200;

        public Vector2 GroupScrollPos { get; set; } = Vector2.zero;

        public float TimeStepWidth { get => TimeStep * WidthForSecond; }
        public float ScrollPosX { get => GroupScrollPos.x; }
        public float ScrollPosY { get => GroupScrollPos.y; }

        public void ShowMenu(Action<ActionData> callback)
        {
            Type[] actionDataTypes = AssemblyUtility.GetDerivedTypes(typeof(ActionData));

            GenericMenu menu = new GenericMenu();
            foreach (var type in actionDataTypes)
            {
                ActionMenuAttribute menuAttr = type.GetCustomAttribute<ActionMenuAttribute>();
                if(menuAttr == null)
                {
                    continue;
                }

                menu.AddItem(new GUIContent($"{menuAttr.Prefix}/{menuAttr.Name}"), false, () =>
                {
                    ActionData data = (ActionData)Activator.CreateInstance(type);
                    callback(data);
                });
            }
            menu.ShowAsContext();
        }
    }
}
