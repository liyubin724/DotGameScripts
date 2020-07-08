using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class LineSetting
    {
        public static LineSetting Setting = null;

        public float TimeLength = 5.0f;
        public int TracklineHeight = 60;

        public float TimeStep { get; set; } = 0.05f;
        public int WidthForSecond { get; set; } = 200;

        public float TimeStepWidth { get => TimeStep * WidthForSecond; }
        public Vector2 ScrollPos = Vector2.zero;
        public float ScrollPosX { get => ScrollPos.x; }
        public float ScrollPosY { get => ScrollPos.y; }

        public int MaxActionIndex = 0;
        public int GetActionIndex()
        {
            return ++MaxActionIndex;
        }
    }
}
