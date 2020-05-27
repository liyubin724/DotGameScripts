using DotEditor.GUIExtension;
using DotEditor.GUIExtension.ListView;
using DotEditor.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public class CreatorAssetPathData
    {
        public string assetPath;
        public override string ToString()
        {
            return Path.GetFileName(assetPath);
        }
    }

    public class AvatarCreatorWindow : EditorWindow
    {
        [MenuItem("Game/Entity/Avatar Creator")]
        static void ShowWin()
        {
            var win = EditorWindow.GetWindow<AvatarCreatorWindow>();
            win.titleContent = new GUIContent("Avatar Creator");
            win.Show();
        }

        private static int TOOLBAR_HEIGHT = 18;
        private static int DATA_LIST_WIDTH = 200;
        private static int LINE_THINKNESS = 1;

        private List<CreatorAssetPathData> creatorDatas = new List<CreatorAssetPathData>();
        private SimpleListView<CreatorAssetPathData> dataListView;

        private AvatarCreatorData creatorData = null;
        void OnEnable()
        {
            FindAllData();

            dataListView = new SimpleListView<CreatorAssetPathData>(creatorDatas);
            dataListView.Header = "Data List";
            dataListView.OnItemSelected = OnListViewItemSelected;
            dataListView.OnDrawItem = (rect, data) =>
            {
                EditorGUI.LabelField(rect, data.ToString(), EGUIStyles.BoldLabelStyle);
            };
            dataListView.Reload();
        }

        private void OnListViewItemSelected(CreatorAssetPathData data)
        {
            creatorData = AssetDatabase.LoadAssetAtPath<AvatarCreatorData>(data.assetPath);
            Repaint();
        }

        private void FindAllData()
        {
            creatorDatas.Clear();

            string[] assetPaths = AssetDatabaseUtility.FindAssets<AvatarCreatorData>();
            foreach(var assetPath in assetPaths)
            {
                creatorDatas.Add(new CreatorAssetPathData() { assetPath = assetPath });
            }

            dataListView?.Reload();
        }

        void OnGUI()
        {
            Rect rect = new Rect(0, 0, position.width, position.height);

            Rect toolbarRect = new Rect(rect.x, rect.y, position.width, TOOLBAR_HEIGHT);
            EditorGUI.LabelField(toolbarRect, GUIContent.none, EditorStyles.toolbar);
            DrawToolbar(toolbarRect);

            Rect dataListRect = new Rect(rect.x+LINE_THINKNESS, rect.y + TOOLBAR_HEIGHT+LINE_THINKNESS,
                                                                DATA_LIST_WIDTH-LINE_THINKNESS*2, rect.height - TOOLBAR_HEIGHT - LINE_THINKNESS *2);
            dataListView.OnGUI(dataListRect);

            Rect skeletonRect = new Rect(dataListRect.x + dataListRect.width, dataListRect.y, (rect.width - dataListRect.width) * 0.5f, dataListRect.height);
            EGUI.DrawAreaLine(skeletonRect,Color.black);
            Rect partRect = skeletonRect;
            partRect.x += skeletonRect.width;
            EGUI.DrawAreaLine(partRect, Color.black);

            DrawSkeleton(skeletonRect);
            DrawParts(partRect);
        }

        private void DrawToolbar(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                GUILayout.BeginHorizontal();
                {
                    EGUILayout.ToolbarButton("New");
                    EGUILayout.ToolbarButton("Delete");
                    EGUILayout.ToolbarButton("Export");
                    GUILayout.FlexibleSpace();
                    EGUILayout.ToolbarButton("Export All");
                    EGUILayout.ToolbarButton("Refresh");
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private void DrawSkeleton(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EGUILayout.DrawBoxHeader("Skeleton Data", EGUIStyles.BoxedHeaderCenterStyle, GUILayout.ExpandWidth(true));
                if(creatorData !=null)
                {

                }
            }
            GUILayout.EndArea();
        }

        private void DrawParts(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EGUILayout.DrawBoxHeader("Part Data", EGUIStyles.BoxedHeaderCenterStyle,GUILayout.ExpandWidth(true));
                if (creatorData != null)
                {

                }
            }
            GUILayout.EndArea();
        }










        [MenuItem("Game/Entity/Create data")]
        static void CreateCreatorData()
        {
            string dir = SelectionUtility.GetSelectionDir();
            Debug.Log(dir);
            if(!string.IsNullOrEmpty(dir))
            {
                var data = ScriptableObject.CreateInstance<AvatarCreatorData>();
                AssetDatabase.CreateAsset(data, $"{dir}/t.asset");
            }
        }
    }
}
