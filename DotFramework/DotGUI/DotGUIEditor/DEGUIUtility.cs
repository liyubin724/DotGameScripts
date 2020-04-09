using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace DotEditor.EGUI
{
    public static class DEGUIUtility
    {
        public static readonly float singleLineHeight = EditorGUIUtility.singleLineHeight;
        public static readonly float standSpacing = EditorGUIUtility.standardVerticalSpacing;
        public static readonly float boxFrameSize = 6.0f;
    }
}
