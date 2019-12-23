using System;

namespace Dot.UI.Panel
{
    [Serializable]
    public class PanelAnimationTriggers
    {
        private const string kDefaultOpenAnimName = "Open";
        private const string kDefaultCloseAnimName = "Close";

        public string OpenTrigger = kDefaultOpenAnimName;
        public string CloseTrigger = kDefaultCloseAnimName;
    }
}
