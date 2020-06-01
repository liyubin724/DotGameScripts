using System;

namespace DotEngine.Service.Update
{
    public interface IUpdateServicer : IServicer
    {
        void AddUpdateHandler(Action<float> update);
        void AddUnscaleUpdateHandler(Action<float> unscaleUpdate);
        void AddLateUpdateHandler(Action<float> lateUpdate);
        void AddFixedUpdateHandler(Action<float> fixedUpdate);

        void RemoveUpdateHandler(Action<float> update);
        void RemoveUnscaleUpdateHandler(Action<float> unscaleUpdate);
        void RemoveLateUpdateHandler(Action<float> lateUpdate);
        void RemoveFixedUpdateHandler(Action<float> fixedUpdate);

        void DoUpdate(float deltaTime);
        void DoUnscaleUpdate(float deltaTime);
        void DoLateUpdate(float deltaTime);
        void DoFixedUpdate(float deltaTime);
    }
}
