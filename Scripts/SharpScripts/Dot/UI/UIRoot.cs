using Dot.Core.Util;
using Dot.Log;
using UnityEngine;

namespace Dot.UI
{
    public enum UIRootLayerType
    {

    }

    public class UIRoot : MonoBehaviour
    {
        private static UIRoot m_root = null;

        public static UIRoot Root
        {
            get
            {
                return m_root;
            }
        }

        private void Awake()
        {
            if(m_root!=null)
            {
                LogUtil.LogError(typeof(UIRoot),"UIRoot has been initized!");
                Destroy(this);
                return;
            }

            m_root = this;
            DontDestroyHandler.AddTransform(transform);
        }
    }
}
