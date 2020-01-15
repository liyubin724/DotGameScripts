using Dot.Util;
using Dot.Log;
using UnityEngine;
using Dot.Dispatch;
using Game.Dispatch;

namespace Dot.UI
{
    public class UIRoot : MonoBehaviour
    {
        private static UIRoot uiRoot = null;
        public static UIRoot Root { get => uiRoot; }

        [SerializeField]
        private Camera uiCamera = null;
        public Camera UICamera { get => uiCamera; }
        [SerializeField]
        private Canvas uiCanvas = null;
        public Canvas UICanvas { get => uiCanvas; }
        [SerializeField]
        private UIManager uiMgr;
        public UIManager UIMgr { get => uiMgr; }

        private void Awake()
        {
            if (uiRoot != null)
            {
                LogUtil.LogError(typeof(UIRoot), "UIRoot has been initized!");
                Destroy(this);
                return;
            }

            if (uiCamera == null)
            {
                LogUtil.LogError(typeof(UIRoot), "UICamera is Null");
                return;
            }

            if (uiCanvas == null)
            {
                LogUtil.LogError(typeof(UIRoot), "UICanvas is Null");
                return;
            }
            if(uiMgr == null)
            {
                LogUtil.LogError(typeof(UIRoot), "UIMgr is Null");
                return;
            }

            uiRoot = this;
            DontDestroyHandler.AddTransform(transform);

            EventManager.GetInstance().RegisterEvent(GameEventConst.CONTROLLER_INIT, OnControllerInit);
        }

        private void OnControllerInit(EventData eventData)
        {
            uiMgr.DoInit();
        }
    }
}
