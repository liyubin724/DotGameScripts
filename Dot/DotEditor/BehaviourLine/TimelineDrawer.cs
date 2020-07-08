using DotEditor.GUIExtension;
using DotEngine.BehaviourLine.Line;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class TimelineDrawer
    {
        private const int TOOLBAR_HEIGHT = 20;
        private const int TRACK_TITLE_HEIGHT = 20;
        private const int PROPERTY_TITLE_HEIGHT = 20;
        private const int LINE_RULER_HEIGHT = 20;

        private const float MIN_TRACK_WIDTH = 100;
        private const float MAX_TRACK_WIDTH = 300;

        private const float MIN_PROPERTY_WIDTH = 200;
        private const float MAX_PROPERTY_WIDTH = 400;

        private const float DRAG_WIDTH = 5;

        public TimelineData Data { get; private set; }

        private EditorWindow window;

        private LineSetting setting;
        private string titleName = string.Empty;
        private List<TracklineDrawer> trackDrawers = new List<TracklineDrawer>();

        private float trackWidth = MIN_TRACK_WIDTH;
        private float propertyWidth = MIN_PROPERTY_WIDTH;

        private bool isTrackDragging = false;
        private bool isPropertyDragging = false;

        private int m_SelectTrackIndex = -1;
        public int SelectTrackIndex
        {
            get { return m_SelectTrackIndex; }
            set
            {
                if (m_SelectTrackIndex != value)
                {
                    if (m_SelectTrackIndex >= 0 && m_SelectTrackIndex < trackDrawers.Count)
                    {
                        //trackDrawers[m_SelectTrackIndex].IsSelected = false;
                    }
                    m_SelectTrackIndex = value;
                    if (m_SelectTrackIndex < 0 || m_SelectTrackIndex >= trackDrawers.Count)
                    {
                        m_SelectTrackIndex = -1;
                    }
                    else
                    {
                        //trackDrawers[m_SelectTrackIndex].IsSelected = true;
                    }
                }
            }
        }

        public TimelineDrawer(EditorWindow win,string titleName = null)
        {
            window = win;
            window.wantsMouseMove = true;

            this.titleName = titleName??"Timeline";
            setting = new LineSetting();
            LineSetting.Setting = setting;
        }

        public void SetData(TimelineData data)
        {
            Data = data;

            int maxActionIndex = 0;
            foreach(var track in data.Tracks)
            {
                foreach(var action in track.Actions)
                {
                    if(action.Index>maxActionIndex)
                    {
                        maxActionIndex = action.Index;
                    }
                }
            }
            setting.MaxActionIndex = maxActionIndex;

            for(int i =0;i<data.Tracks.Count;++i)
            {
                TracklineDrawer tracklineDrawer = new TracklineDrawer(this);
                tracklineDrawer.SetData(i, data.Tracks[i]);

                trackDrawers.Add(tracklineDrawer);
            }
        }

        public void OnGUI(Rect rect)
        {
            Rect toolbarRect = new Rect(rect.x, rect.y, rect.width, TOOLBAR_HEIGHT);
            DrawToolbar(toolbarRect);

            Rect trackRect = new Rect(rect.x, rect.y + TOOLBAR_HEIGHT, trackWidth, rect.height - TOOLBAR_HEIGHT);
            DrawTrack(trackRect);

            Rect trackDragRect = new Rect(trackRect.x + trackRect.width, trackRect.y, DRAG_WIDTH, trackRect.height);
            DrawTrackDrag(trackDragRect);

            Rect propertyRect = new Rect(rect.x + rect.width - propertyWidth, trackRect.y, propertyWidth, trackRect.height);
            DrawProperty(propertyRect);

            Rect propertyDragRect = new Rect(propertyRect.x - DRAG_WIDTH, trackRect.y, DRAG_WIDTH, trackRect.height);
            DrawPropertyDrag(propertyDragRect);

            Rect lineRect = new Rect(trackDragRect.x + trackDragRect.width, trackRect.y, rect.width - trackRect.width - trackDragRect.width - propertyRect.width - propertyDragRect.width, trackRect.height);
            if(lineRect.width>0)
            {
                DrawLine(lineRect);
            }
        }

        private void DrawToolbar(Rect toolbarRect)
        {
            EditorGUI.LabelField(toolbarRect, GUIContent.none, EditorStyles.toolbar);
            EditorGUI.LabelField(toolbarRect, titleName, Styles.titleStyle);

            Rect helpBtnRect = new Rect(toolbarRect.x + toolbarRect.width - 30, toolbarRect.y, 30, toolbarRect.height);
            if(GUI.Button(helpBtnRect,Contents.helpContent,EditorStyles.toolbarButton))
            {

            }

            Rect settingBtnRect = helpBtnRect;
            settingBtnRect.x -= 30;
            if(GUI.Button(settingBtnRect,Contents.settingContent,EditorStyles.toolbarButton))
            {
                LineSettingWindow.ShowWin();
            }

            Rect zoomOutBtnRect = settingBtnRect;
            zoomOutBtnRect.x -= 33;
            if (GUI.Button(zoomOutBtnRect, Contents.zoomOutContent, EditorStyles.toolbarButton))
            {

            }

            Rect zoomInBtnRect = zoomOutBtnRect;
            zoomInBtnRect.x -= 30;
            if (GUI.Button(zoomInBtnRect, Contents.zoomInContent, EditorStyles.toolbarButton))
            {

            }
        }

        private void DrawTrack(Rect trackRect)
        {
            EGUI.DrawAreaLine(trackRect, Color.gray);

            LineSetting setting = LineSetting.Setting;

            Rect titleRect = new Rect(trackRect.x, trackRect.y, trackRect.width, TRACK_TITLE_HEIGHT);
            EditorGUI.LabelField(titleRect, "Tracks",EditorStyles.toolbar);

            Rect rect = new Rect(trackRect.x, trackRect.y + TRACK_TITLE_HEIGHT, trackRect.width, trackRect.height - TRACK_TITLE_HEIGHT);
            using (new GUI.ClipScope(new Rect(rect.x, rect.y, rect.width, rect.height)))
            {
                int start = Mathf.FloorToInt(setting.ScrollPosY / setting.TracklineHeight);
                int end = Mathf.CeilToInt((setting.ScrollPosY + rect.height) / setting.TracklineHeight);

                for (int i = start; i < end; ++i)
                {
                    float y = setting.TracklineHeight * i - setting.ScrollPos.y;

                    if (i >= Data.Tracks.Count)
                    {
                        break;
                    }

                    Rect indexRect = new Rect(0, y, trackWidth, setting.TracklineHeight);
                    GUI.Label(indexRect, $"{(Data.Tracks[i].Name ?? "")} ({i.ToString()})", SelectTrackIndex == i ? "flow node 1" : "flow node 0");
                    if (indexRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp && Event.current.button == 0)
                    {
                        SelectTrackIndex = i;
                        Event.current.Use();
                    }
                }
            }
        }

        private void DrawTrackDrag(Rect dragRect)
        {
            EGUI.DrawVerticalLine(dragRect, Color.grey, 2.0f);

            EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.ResizeHorizontal);

            if (Event.current!=null)
            {
                if(Event.current.type == EventType.MouseDown && Event.current.button == 0 && dragRect.Contains(Event.current.mousePosition))
                {
                    isTrackDragging = true;

                    Event.current.Use();
                    window.Repaint();
                }else if(isTrackDragging && Event.current.type == EventType.MouseDrag)
                {
                    trackWidth += Event.current.delta.x;
                    if(trackWidth < MIN_TRACK_WIDTH)
                    {
                        trackWidth = MIN_TRACK_WIDTH;
                    }else if(trackWidth>MAX_TRACK_WIDTH)
                    {
                        trackWidth = MAX_TRACK_WIDTH;
                    }
                    window.Repaint();
                }
                else if(isTrackDragging && Event.current.type == EventType.MouseUp)
                {
                    isTrackDragging = false;
                    Event.current.Use();
                    window.Repaint();
                }
            }
        }

        private void DrawLine(Rect lineRect)
        {
            EGUI.DrawAreaLine(lineRect, Color.gray);

            Rect rulerRect = new Rect(lineRect.x, lineRect.y, lineRect.width, LINE_RULER_HEIGHT);
            EditorGUI.LabelField(rulerRect, GUIContent.none, EditorStyles.toolbar);
            DrawLineRuler(rulerRect);

            Rect gridRect = new Rect(lineRect.x, lineRect.y + LINE_RULER_HEIGHT, lineRect.width, lineRect.height - LINE_RULER_HEIGHT);
            DrawLineGrid(gridRect);

            DrawTrackline(gridRect);

            LineSetting setting = LineSetting.Setting;
            using (new GUILayout.AreaScope(gridRect))
            {
                using (var scop = new UnityEditor.EditorGUILayout.ScrollViewScope(setting.ScrollPos))
                {
                    float scrollWith = Mathf.Max(Data.TimeLength * setting.WidthForSecond, gridRect.width);
                    float scrollHeight = Mathf.Max(Data.Tracks.Count * setting.TracklineHeight, gridRect.height);

                    GUILayout.Label("", GUILayout.Width(scrollWith), GUILayout.Height(scrollHeight - 20));

                    setting.ScrollPos = scop.scrollPosition;
                }
            }
        }

        private void DrawLineRuler(Rect rulerRect)
        {
            LineSetting setting = LineSetting.Setting;
            using (new GUI.ClipScope(rulerRect))
            {
                int start = Mathf.FloorToInt(setting.ScrollPosX / setting.WidthForSecond);
                int end = Mathf.CeilToInt((setting.ScrollPosX + rulerRect.width) / setting.WidthForSecond);

                int startCount = Mathf.FloorToInt(start / setting.TimeStep);
                int endCount = Mathf.FloorToInt(end / setting.TimeStep);
                for (int i = startCount; i <= endCount; i++)
                {
                    var x = i * setting.TimeStepWidth - setting.ScrollPosX;

                    if (i % 10 == 0)
                    {
                        Handles.color = new Color(0, 0, 0, 0.8f);
                        Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, rulerRect.height * 0.8f, 0));
                        GUI.Label(new Rect(x, 5, 40, 40), (i * setting.TimeStep).ToString("F1"));
                    }
                    else if (i % 5 == 0)
                    {
                        Handles.color = new Color(0, 0, 0, 0.5f);
                        Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, rulerRect.height * 0.5f, 0));
                    }
                    else
                    {
                        Handles.color = new Color(0, 0, 0, 0.5f);
                        Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, rulerRect.height * 0.3f, 0));
                    }
                }
            }
        }

        private void DrawLineGrid(Rect gridRect)
        {
            LineSetting setting = LineSetting.Setting;
            using (new GUI.ClipScope(new Rect(gridRect.x, gridRect.y, gridRect.width, gridRect.height)))
            {
                int startX = Mathf.FloorToInt(setting.ScrollPosX / setting.WidthForSecond);
                int endX = Mathf.CeilToInt((setting.ScrollPosX + gridRect.width) / setting.WidthForSecond);

                int startXCount = Mathf.FloorToInt(startX / setting.TimeStep);
                int endXCount = Mathf.FloorToInt(endX / setting.TimeStep);
                for (int i = startXCount; i <= endXCount; i++)
                {
                    var x = i * setting.TimeStepWidth - setting.ScrollPosX;

                    Color handlesColor = new Color(0, 0, 0, 0.3f);
                    if (i % 10 == 0)
                    {
                        handlesColor = new Color(0, 0, 0, 1.0f);
                    }
                    else if (i % 5 == 0)
                    {
                        handlesColor = new Color(0, 0, 0, 0.8f);
                    }
                    Handles.color = handlesColor;
                    Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, gridRect.height, 0));
                }

                float stopLineX = Data.TimeLength * setting.WidthForSecond - setting.ScrollPosX;
                Handles.color = Color.red;
                Handles.DrawLine(new Vector3(stopLineX, 0, 0), new Vector3(stopLineX, gridRect.height, 0));

                int startY = Mathf.FloorToInt(setting.ScrollPosY / setting.TracklineHeight);
                int endY = Mathf.CeilToInt((setting.ScrollPosY + gridRect.height) / setting.TracklineHeight);
                for (int i = startY; i <= endY; i++)
                {
                    float y = setting.TracklineHeight * i - setting.ScrollPosY;
                    Handles.color = new Color(0, 0, 0, 0.9f);
                    Handles.DrawLine(new Vector3(0, y, 0), new Vector3(gridRect.width, y, 0));
                }
            }
        }

        private void DrawTrackline(Rect lineRect)
        {
            LineSetting setting = LineSetting.Setting;
            using (new GUI.ClipScope(lineRect))
            {
                int startY = Mathf.FloorToInt(setting.ScrollPosY / setting.TracklineHeight);
                int endY = Mathf.CeilToInt((setting.ScrollPosY + lineRect.height) / setting.TracklineHeight);

                float maxWidth = Data.TimeLength * setting.WidthForSecond;

                for (int i = startY; i < endY; ++i)
                {
                    float y = setting.TracklineHeight * i - setting.ScrollPosY;

                    if (i >= trackDrawers.Count)
                    {
                        break;
                    }

                    Rect tRect = new Rect(0, y, lineRect.width, setting.TracklineHeight);
                    tRect.width = Mathf.Min(lineRect.width, maxWidth - setting.ScrollPosX);
                    //if (SelectTrackIndex == i)
                    //{
                    //    EGUI.DrawAreaLine(trackRect, Color.green);
                    //}

                    trackDrawers[i].OnDrawGUI(tRect);
                }
            }
        }

        private void DrawPropertyDrag(Rect dragRect)
        {
            EGUI.DrawVerticalLine(dragRect,Color.grey,2.0f);

            EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.ResizeHorizontal);

            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && dragRect.Contains(Event.current.mousePosition))
                {
                    isPropertyDragging = true;

                    Event.current.Use();
                    window.Repaint();
                }
                else if (isPropertyDragging && Event.current.type == EventType.MouseDrag)
                {
                    propertyWidth -= Event.current.delta.x;
                    if (propertyWidth < MIN_PROPERTY_WIDTH)
                    {
                        propertyWidth = MIN_PROPERTY_WIDTH;
                    }
                    else if (propertyWidth > MAX_PROPERTY_WIDTH)
                    {
                        propertyWidth = MAX_PROPERTY_WIDTH;
                    }
                    window.Repaint();
                }
                else if (isPropertyDragging && Event.current.type == EventType.MouseUp)
                {
                    isPropertyDragging = false;
                    Event.current.Use();
                    window.Repaint();
                }
            }
        }

        private void DrawProperty(Rect propertyRect)
        {
            EGUI.DrawAreaLine(propertyRect, Color.gray);

            LineSetting setting = LineSetting.Setting;

            Rect titleRect = new Rect(propertyRect.x, propertyRect.y, propertyRect.width, TRACK_TITLE_HEIGHT);
            EditorGUI.LabelField(titleRect, "Property", EditorStyles.toolbar);

            Rect clipRect = new Rect(propertyRect.x, propertyRect.y + TRACK_TITLE_HEIGHT, propertyRect.width, propertyRect.height - TRACK_TITLE_HEIGHT);
            using (new GUI.ClipScope(clipRect))
            {

            }
        }

        class Contents
        {
            public static GUIContent helpContent = new GUIContent("?","Show help");
            public static GUIContent settingContent = new GUIContent("S", "Open Setting Window");
            public static GUIContent zoomInContent = new GUIContent("+", "Zoom in");
            public static GUIContent zoomOutContent = new GUIContent("-", "Zoom out");

        }

        class Styles
        {
            public static GUIStyle titleStyle = null;
            public static GUIStyle propertyStyle = null;

            static Styles()
            {
                titleStyle = new GUIStyle(EditorStyles.label);
                titleStyle.alignment = TextAnchor.MiddleCenter;
            }
        }
    }
}
