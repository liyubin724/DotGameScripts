using System;
using UnityEngine;

namespace Dot.UI.Panel
{
    [Serializable]
    public class PanelAnimationTriggers
    {
        private const string kDefaultOpenAnimName = "Open";
        private const string kDefaultCloseAnimName = "Close";

        [SerializeField]
        private string m_OpenTrigger = kDefaultOpenAnimName;

        [SerializeField]
        private string m_CloseTrigger = kDefaultCloseAnimName;

        public string OpenTrigger { get => m_OpenTrigger; set => m_OpenTrigger = value; }
        public string CloseTrigger { get => m_CloseTrigger; set => m_CloseTrigger = value; }
    }
}
