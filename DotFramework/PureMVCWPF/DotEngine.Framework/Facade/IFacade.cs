﻿using System;

namespace DotEngine.Framework
{
    public interface IFacade: INotifier,IUpdate,IUnscaleUpdate,ILateUpdate,IFixedUpdate
    {
        void RegisterService(IService service);
        IService RetrieveService(string name);
        void RemoveService(string name);
        bool HasService(string name);

        void RegisterProxy(IProxy proxy);
        IProxy RetrieveProxy(string proxyName);
        IProxy RemoveProxy(string proxyName);
        bool HasProxy(string proxyName);

        void RegisterCommand(string notificationName, ICommand command);
        void RemoveCommand(string notificationName);
        bool HasCommand(string notificationName);

        void RegisterMediator(IViewController mediator);
        IViewController RetrieveMediator(string mediatorName);
        IViewController RemoveMediator(string mediatorName);
        bool HasMediator(string mediatorName);

        void RegisterObserver(string notificationName, Action<INotification> notifyMethod);
        void RemoveObserver(string notificationName, Action<INotification> notifyMethod);
    }
}
