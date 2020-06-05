using System;
using System.Collections.Generic;

namespace DotEngine.Framework
{
    public class ServiceCenter : IServiceCenter
    {
        private Dictionary<string, IService> serviceDic = new Dictionary<string, IService>();

        private List<string> updateServices = null;
        private List<string> lateUpdateServices = null;
        private List<string> unscaleUpdateServices = null;
        private List<string> fixedUpdateServices = null;

        public ServiceCenter()
        {
            serviceDic = new Dictionary<string, IService>();
            updateServices = new List<string>();
            lateUpdateServices = new List<string>();
            unscaleUpdateServices = new List<string>();
            fixedUpdateServices = new List<string>();

            InitilizeCenter();
        }

        protected virtual void InitilizeCenter()
        {
        }

        public virtual void DoUpdate(float deltaTime)
        {
            for (int i = updateServices.Count - 1; i >= 0; --i)
            {
                string name = updateServices[i];
                if (serviceDic.TryGetValue(name, out IService value))
                {
                    ((IUpdate)value).DoUpdate(deltaTime);
                }
                else
                {
                    updateServices.RemoveAt(i);
                }
            }
        }

        public virtual void DoUnscaleUpdate(float deltaTime)
        {
            for (int i = unscaleUpdateServices.Count - 1; i >= 0; --i)
            {
                string name = unscaleUpdateServices[i];
                if (serviceDic.TryGetValue(name, out IService value))
                {
                    ((IUnscaleUpdate)value).DoUnscaleUpdate(deltaTime);
                }
                else
                {
                    unscaleUpdateServices.RemoveAt(i);
                }
            }
        }

        public virtual void DoLateUpdate(float deltaTime)
        {
            for (int i = lateUpdateServices.Count - 1; i >= 0; --i)
            {
                string name = lateUpdateServices[i];
                if (serviceDic.TryGetValue(name, out IService value))
                {
                    ((ILateUpdate)value).DoLateUpdate(deltaTime);
                }
                else
                {
                    lateUpdateServices.RemoveAt(i);
                }
            }
        }

        public virtual void DoFixedUpdate(float deltaTime)
        {
            for (int i = fixedUpdateServices.Count - 1; i >= 0; --i)
            {
                string name = fixedUpdateServices[i];
                if (serviceDic.TryGetValue(name, out IService value))
                {
                    ((IFixedUpdate)value).DoFixedUpdate(deltaTime);
                }
                else
                {
                    fixedUpdateServices.RemoveAt(i);
                }
            }
        }


        public virtual bool HasService(string name)
        {
            return serviceDic.ContainsKey(name);
        }

        public virtual void RegisterService(string serviceName,IService service)
        {
            if (service == null || string.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException("The service or the name of service is empty");
            }

            if (serviceDic.ContainsKey(serviceName))
            {
                throw new Exception($"The name of service has been added.name = {serviceName}.");
            }

            serviceDic.Add(serviceName, service);

            Type serviceType = service.GetType();
            if (typeof(IUpdate).IsAssignableFrom(serviceType))
            {
                updateServices.Add(serviceName);
            }
            else if (typeof(IUnscaleUpdate).IsAssignableFrom(serviceType))
            {
                unscaleUpdateServices.Add(serviceName);
            }
            else if (typeof(ILateUpdate).IsAssignableFrom(serviceType))
            {
                lateUpdateServices.Add(serviceName);
            }
            else if (typeof(IFixedUpdate).IsAssignableFrom(serviceType))
            {
                fixedUpdateServices.Add(serviceName);
            }

            service.DoRegister();
        }

        public virtual void RemoveService(string name)
        {
            if(serviceDic.TryGetValue(name,out IService servicer))
            {
                serviceDic.Remove(name);

                servicer.DoRemove();
            }
        }

        public virtual IService RetrieveService(string name)
        {
            return serviceDic.TryGetValue(name, out IService servicer) ? servicer : null;
        }
    }
}
