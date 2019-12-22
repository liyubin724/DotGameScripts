using Dot.Lua.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dot.UI
{
    public class LuaUIButton : Selectable, IPointerClickHandler
    {
        public LuaEventData eventData = new LuaEventData();

        public LuaUIButton()
        {
        }

        public void OnPointerClick(PointerEventData e)
        {
            if (e.button != PointerEventData.InputButton.Left)
                return;
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("LuaButton.OnPointerClick", this);
            eventData.Invoke();
        }
    }
}
