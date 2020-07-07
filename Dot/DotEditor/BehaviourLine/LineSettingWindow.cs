using UnityEditor;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class LineSettingWindow : EditorWindow
    {
        private const float MIN_TIME_LENGTH = 0.0f;
        private const int MIN_TRACKLINE_HEIGHT = 30;
        private const float MIN_TIME_STEP = 0.01f;
        private const float MAX_TIME_STEP = 1.0f;
        private const int MIN_WIDTH_FOR_SECOND = 200;
        private const int MAX_WIDTH_FOR_SECOND = 400;

        public static void ShowWin()
        {
            var win = GetWindow<LineSettingWindow>();
            win.titleContent = new GUIContent("Setting");
            win.Show();
        }

        void OnGUI()
        {
            LineSetting setting = LineSetting.Setting;
            if(setting == null)
            {
                return;
            }

            setting.TimeLength = EditorGUILayout.FloatField("Time Length", setting.TimeLength);
            if(setting.TimeLength < MIN_TIME_LENGTH)
            {
                setting.TimeLength = MIN_TIME_LENGTH;
            }

            setting.TracklineHeight = EditorGUILayout.IntField("Trackline Height", setting.TracklineHeight);
            if(setting.TracklineHeight< MIN_TRACKLINE_HEIGHT)
            {
                setting.TracklineHeight = MIN_TRACKLINE_HEIGHT;
            }

            setting.TimeStep = EditorGUILayout.FloatField("Time Step", setting.TimeStep);
            if(setting.TimeStep< MIN_TIME_STEP)
            {
                setting.TimeStep = MIN_TIME_STEP;
            }else if(setting.TimeStep > MAX_TIME_STEP)
            {
                setting.TimeStep = MAX_TIME_STEP;
            }

            setting.WidthForSecond = EditorGUILayout.IntField("Width For Second", setting.WidthForSecond);
            if(setting.WidthForSecond> MAX_WIDTH_FOR_SECOND)
            {
                setting.WidthForSecond = MAX_WIDTH_FOR_SECOND;
            }else if(setting.WidthForSecond < MIN_WIDTH_FOR_SECOND)
            {
                setting.WidthForSecond = MIN_WIDTH_FOR_SECOND;
            }

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUILayout.FloatField("Time Step Width", setting.TimeStepWidth);
                EditorGUILayout.Vector2Field("Scroll Pos", setting.ScrollPos);
            }
            EditorGUI.EndDisabledGroup();

        }
    }
}
