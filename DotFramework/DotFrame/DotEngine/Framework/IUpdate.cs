namespace DotEngine.Framework
{
    public interface IUpdate
    {
        void DoUpdate(float deltaTime);
        void DoUnscaleUpdate(float deltaTime);
        void DoLateUpdate(float deltaTime);
        void DoFixedUpdate(float deltaTime);
    }
}
