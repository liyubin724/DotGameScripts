using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public class AvatarCreatorWindow : EditorWindow
    {
        [MenuItem("Game/Entity/Avatar Part Creator")]
        static void ShowWin()
        {
            EditorWindow.GetWindow<AvatarCreatorWindow>().Show();
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

        void OnEnable()
        {

        }
    }
}
