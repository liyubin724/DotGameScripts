namespace DotEngine.Framework.Update
{
    public interface IUpdate
    {
        void DoUpdate(float deltaTime);
    }

    public interface IUnscaleUpdate
    {
        void DoUnscaleUpdate(float deltaTime);
    }

    public interface ILateUpdate
    {
        void DoLateUpdate(float deltaTime);
    }

    public interface IFixedUpdate
    {
        void DoFixedUpdate(float deltaTime);
    }
}
