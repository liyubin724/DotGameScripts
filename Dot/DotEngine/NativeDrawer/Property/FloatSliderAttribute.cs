using System;

namespace Dot.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FloatSliderAttribute : PropertyDrawerAttribute
    {
        public float LeftValue { get; private set; }
        public float RightValue { get; private set; }
        public bool ShowTextField { get; private set; }
        public FloatSliderAttribute(float leftValue,float rightValue,bool showTextField = true)
        {
            LeftValue = leftValue;
            RightValue = rightValue;
            ShowTextField = showTextField;
        }
    }
}
