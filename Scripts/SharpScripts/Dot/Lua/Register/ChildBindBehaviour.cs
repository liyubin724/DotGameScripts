namespace Dot.Lua.Register
{
    public class ChildBindBehaviour : LuaScriptBindBehaviour
    {
        public RegisterBehaviourData registerBehaviourData = new RegisterBehaviourData();

        protected override void OnInitFinished()
        {
            if(ObjTable != null)
            {
                registerBehaviourData.RegisterToLua(luaEnv, ObjTable);
            }
        }
    }
}
