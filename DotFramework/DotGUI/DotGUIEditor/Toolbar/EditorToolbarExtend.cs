using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityObject = UnityEngine.Object;

namespace DotEditor.EGUI.Toolbar
{
    public enum EditorToolbarItemOrientation
    {
        Left,
        Right,
    }

    public class EditorToolbarItemGroup
    {
        internal EditorToolbarItem[] Items { get; private set; }

        public EditorToolbarItemGroup(EditorToolbarItem[] items)
        {
            Items = items;
        }
    }

    [InitializeOnLoad]
    public static class EditorToolbarExtend
    {
        private static readonly Type containterType = typeof(IMGUIContainer);
        private static readonly Type toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");
        private static readonly Type guiViewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GUIView");

        private static readonly FieldInfo onGuiHandler = containterType.GetField("m_OnGUIHandler",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly PropertyInfo visualTree = guiViewType.GetProperty("visualTree",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private static UnityObject toolbar;
        static EditorToolbarExtend()
        {
            EditorApplication.update -= DoUpdate;
            EditorApplication.update += DoUpdate;
        }

        private static void DoUpdate()
        {
            if (toolbar != null) return;
            var toolbars = Resources.FindObjectsOfTypeAll(toolbarType);
            if (toolbars == null || toolbars.Length == 0) return;

            toolbar = toolbars[0];

            var container = (visualTree.GetValue(toolbar, null) as VisualElement)[0];

            var handler = onGuiHandler.GetValue(container) as Action;
            handler -= OnGUIHandler;
            handler += OnGUIHandler;
            onGuiHandler.SetValue(container, handler);
        }

        private static void OnGUIHandler()
        {
            var screenWidth = EditorGUIUtility.currentViewWidth;
            var screenHeight = Screen.height;

            var leftRect = new Rect(2, 0, screenWidth, Styles.TOOLBAR_HEIGHT);
            leftRect.y += Styles.PADDING;
            leftRect.xMax = (screenWidth - Styles.LEFT_FROM_STRIP_OFFSET_X) / 2;
            leftRect.xMin += Styles.LEFT_FROM_TOOLS_OFFSET_X;
            leftRect.xMin += Styles.SPACEING;
            leftRect.xMax -= Styles.SPACEING;

            if (leftRect.width > 0)
            {
                DrawLeftToolbarItems(leftRect);
            }

            var rightRect = new Rect(0, 0, screenWidth, Styles.TOOLBAR_HEIGHT);
            rightRect.y += Styles.PADDING;
            rightRect.xMin = (screenWidth + Styles.RIGHT_FROM_STRIP_OFFSET_X) / 2;
            rightRect.xMin += Styles.SPACEING;
            rightRect.xMax = screenWidth - Styles.RIGHT_FROM_TOOLS_OFFSET_X;
            rightRect.xMax -= Styles.SPACEING;

            if (rightRect.width > 0)
            {
                DrawRightToolbarItems(rightRect);
            }
        }

        private static List<EditorToolbarItemGroup> leftItemGroups = new List<EditorToolbarItemGroup>();
        private static List<EditorToolbarItemGroup> rightItemGroups = new List<EditorToolbarItemGroup>();

        private static void DrawLeftToolbarItems(Rect rect)
        {
            DrawerToolbar(rect, EditorToolbarItemOrientation.Left, leftItemGroups);
        }

        private static void DrawRightToolbarItems(Rect rect)
        {
            DrawerToolbar(rect, EditorToolbarItemOrientation.Right, rightItemGroups);
        }

        private static void DrawerToolbar(Rect rect, EditorToolbarItemOrientation orientation, List<EditorToolbarItemGroup> items)
        {
            if (items.Count == 0) return;

            Rect remainingRect = rect;
            foreach (var itemGroup in items)
            {
                float groupWidth = (from item in itemGroup.Items select item.GetItemWidth()).ToArray().Sum();
                if (rect.width < groupWidth)
                {
                    if (GUI.Button(remainingRect, "...", Styles.commandStyle))
                    {

                    }
                    break;
                }

                Rect groupRect = remainingRect;
                groupRect.width = groupWidth;
                if (orientation == EditorToolbarItemOrientation.Left)
                {
                    groupRect.x = remainingRect.x + remainingRect.width - groupWidth;
                }
                else if (orientation == EditorToolbarItemOrientation.Right)
                {
                    groupRect.x = remainingRect.x;
                }

                float startX = 0.0f;
                if (orientation == EditorToolbarItemOrientation.Left)
                {
                    startX = groupRect.x + groupRect.width;
                }
                else if (orientation == EditorToolbarItemOrientation.Right)
                {
                    startX = groupRect.x;
                }
                Rect itemRect = new Rect(startX, groupRect.y, 0, groupRect.height);
                for (int i = 0; i < itemGroup.Items.Length; ++i)
                {
                    var item = itemGroup.Items[i];
                    itemRect.width = item.GetItemWidth();
                    itemRect.x = startX;
                    if (orientation == EditorToolbarItemOrientation.Left)
                    {
                        startX -= itemRect.width;
                        itemRect.x = startX;
                    }
                    else if (orientation == EditorToolbarItemOrientation.Right)
                    {
                        startX += itemRect.width;
                    }

                    GUIStyle style = null;
                    if (item.GetType() == typeof(EditorToolbarButton))
                    {
                        if (itemGroup.Items.Length == 1)
                        {
                            style = Styles.commandStyle;
                        }
                        else
                        {
                            if (i == 0)
                            {
                                if (orientation == EditorToolbarItemOrientation.Left)
                                {
                                    style = Styles.commandRightStyle;
                                }
                                else if (orientation == EditorToolbarItemOrientation.Right)
                                {
                                    style = Styles.commandLeftStyle;
                                }
                            }
                            else if (i == itemGroup.Items.Length - 1)
                            {
                                if (orientation == EditorToolbarItemOrientation.Left)
                                {
                                    style = Styles.commandLeftStyle;
                                }
                                else if (orientation == EditorToolbarItemOrientation.Right)
                                {
                                    style = Styles.commandRightStyle;
                                }
                            }
                            else
                            {
                                style = Styles.commandMidStyle;
                            }
                        }
                    }

                    item.OnItemGUI(itemRect, style);
                }

                remainingRect.width -= groupRect.width;
                if (orientation == EditorToolbarItemOrientation.Right)
                {
                    remainingRect.width -= Styles.ITEM_GROUP_SPACE;
                }
                else if (orientation == EditorToolbarItemOrientation.Right)
                {
                    remainingRect.x = remainingRect.x + groupRect.width + Styles.ITEM_GROUP_SPACE;
                }
            }
        }

        public static void AddItemGroup(EditorToolbarItemGroup itemGroup, EditorToolbarItemOrientation orientation)
        {
            if (orientation == EditorToolbarItemOrientation.Left)
            {
                leftItemGroups.Add(itemGroup);
            }
            else if (orientation == EditorToolbarItemOrientation.Right)
            {
                rightItemGroups.Add(itemGroup);
            }
        }

        public static void AddItem(EditorToolbarItem item, EditorToolbarItemOrientation orientation)
        {
            if (orientation == EditorToolbarItemOrientation.Left)
            {
                leftItemGroups.Add(new EditorToolbarItemGroup(new EditorToolbarItem[] { item }));
            }
            else if (orientation == EditorToolbarItemOrientation.Right)
            {
                rightItemGroups.Add(new EditorToolbarItemGroup(new EditorToolbarItem[] { item }));
            }
        }

        public static void RemoveItem(EditorToolbarItem item, EditorToolbarItemOrientation orientation)
        {

        }

        private class Styles
        {
            internal static readonly float SPACEING = 10;
            internal static readonly float PADDING = 5;
            internal static readonly float TOOLBAR_HEIGHT = 24;

            internal static readonly float LEFT_FROM_TOOLS_OFFSET_X = 400.0f;
            internal static readonly float LEFT_FROM_STRIP_OFFSET_X = 140.0f;
            internal static readonly float RIGHT_FROM_TOOLS_OFFSET_X = 400.0f;
            internal static readonly float RIGHT_FROM_STRIP_OFFSET_X = 36.0f;

            internal static readonly float ITEM_GROUP_SPACE = 10.0f;

            internal static readonly GUIStyle commandStyle = new GUIStyle("Command")
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove
            };
            internal static readonly GUIStyle commandMidStyle = new GUIStyle("CommandMid")
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove
            };
            internal static readonly GUIStyle commandLeftStyle = new GUIStyle("CommandLeft")
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove
            };
            internal static readonly GUIStyle commandRightStyle = new GUIStyle("CommandRight")
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove
            };
        }
    }
}
