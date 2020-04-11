using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace DotEditor.EGUI.ColorfullFolders
{
    public class FolderConfig : ScriptableObject
    {

        [Serializable]
        public class FolderData
        {
            public FolderCategory category = FolderCategory.Color;
            public int categoryValue = 0;
            public Texture2D smallIcon;
            public Texture2D largeIcon;
        }
    }
}
