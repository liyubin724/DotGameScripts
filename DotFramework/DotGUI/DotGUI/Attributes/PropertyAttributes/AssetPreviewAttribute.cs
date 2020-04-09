using System;

namespace Dot.GUI.Attributes
{
    /// <summary>
    /// 仅可添加UnityEngine.Object字段上，用于显示对象的预览
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AssetPreviewAttribute : EGUIPropertyAttribute
    {
        public bool UseLabel { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public AssetPreviewAttribute(float width = 64, float height = 64, bool useLabel = true)
        {
            UseLabel = useLabel;
            Width = width;
            Height = height;
        }
    }
}
