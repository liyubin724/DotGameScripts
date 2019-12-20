namespace Dot.Lua.Register
{
    public partial class ComposeBindBehaviour : LuaScriptBindBehaviour
    {
        public RegisterBehaviourData registerBehaviourData = new RegisterBehaviourData();
        public RegisterObjectData registerObjectData = new RegisterObjectData();

        protected override void OnInitFinished()
        {
            if (ObjTable != null)
            {
                registerObjectData.RegisterToLua(luaEnv, ObjTable);
                registerBehaviourData.RegisterToLua(luaEnv, ObjTable);
            }
        }
    }
}
