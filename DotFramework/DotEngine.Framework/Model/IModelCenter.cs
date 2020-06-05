﻿namespace DotEngine.Framework
{
    public interface IModelCenter
    {
        void RegisterProxy(IProxy proxy);

        IProxy RetrieveProxy(string proxyName);

        IProxy RemoveProxy(string proxyName);

        bool HasProxy(string proxyName);
    }
}
