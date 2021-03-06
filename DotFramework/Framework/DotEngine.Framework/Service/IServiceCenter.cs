﻿namespace DotEngine.Framework
{
    public interface IServiceCenter : IUpdate,ILateUpdate,IUnscaleUpdate,IFixedUpdate
    {
        void RegisterService(string serviceName,IService service);
        IService RetrieveService(string name);
        void RemoveService(string name);
        bool HasService(string name);
    }
}
