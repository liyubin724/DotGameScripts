using System;

namespace DotEngine.Framework.Services
{
    public interface IUpdateService : IService, IUpdate, IUnscaleUpdate, ILateUpdate, IFixedUpdate
    {
        void AddUpdateHandler(Action<float> update);
        void RemoveUpdateHandler(Action<float> update);

        void AddUnscaleUpdateHandler(Action<float> unscaleUpdate);
        void RemoveUnscaleUpdateHandler(Action<float> unscaleUpdate);

        void AddLateUpdateHandler(Action<float> lateUpdate);
        void RemoveLateUpdateHandler(Action<float> lateUpdate);

        void AddFixedUpdateHandler(Action<float> fixedUpdate);
        void RemoveFixedUpdateHandler(Action<float> fixedUpdate);

    }
}
