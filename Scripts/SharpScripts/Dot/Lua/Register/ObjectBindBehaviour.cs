namespace Dot.Lua.Register
{
    public class ObjectBindBehaviour : LuaScriptBindBehaviour
    {
        public RegisterObjectData registerObjectData = new RegisterObjectData();

        protected override void OnInitFinished()
        {
            registerObjectData?.RegisterToLua(luaEnv, ObjTable);
        }
    }
}
