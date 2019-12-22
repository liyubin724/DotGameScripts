using Dot.Lua.Event;
using UnityEngine;

public class TestLuaEventParam : MonoBehaviour
{
    public LuaEventParam eventParam = new LuaEventParam();
    public LuaEventData eventData2 = new LuaEventData();

    public void Awake()
    {
        Debug.Log(UnityEditor.EditorGUIUtility.singleLineHeight);
    }
}
