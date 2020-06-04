using System;
using System.Collections.Generic;

namespace DotEngine.Framework
{
    public class ServiceCenter : IServiceCenter
    {
        private Dictionary<string, IService> services = new Dictionary<string, IService>();

        private List<string> updateServices = null;
        private List<string> lateUpdateServices = null;
        private List<string> unscaleUpdateServices = null;
        private List<string> fixedUpdateServices = null;

        public ServiceCenter()
        {
            services = new Dictionary<string, IService>();
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
                if (services.TryGetValue(name, out IService value))
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
                if (services.TryGetValue(name, out IService value))
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
                if (services.TryGetValue(name, out IService value))
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
                if (services.TryGetValue(name, out IService value))
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
            return services.ContainsKey(name);
        }

        public virtual void RegisterService(IService service)
        {
            if(service!=null)
            {
                string name = service.Name;

                if (!HasService(name))
                {
                    services.Add(name, service);

                    Type serviceType = service.GetType();
                    if (typeof(IUpdate).IsAssignableFrom(serviceType))
                    {
                        updateServices.Add(name);
                    }
                    else if (typeof(IUnscaleUpdate).IsAssignableFrom(serviceType))
                    {
                        unscaleUpdateServices.Add(name);
                    }
                    else if (typeof(ILateUpdate).IsAssignableFrom(serviceType))
                    {
                        lateUpdateServices.Add(name);
                    }
                    else if (typeof(IFixedUpdate).IsAssignableFrom(serviceType))
                    {
                        fixedUpdateServices.Add(name);
                    }

                    service.DoRegister();
                }
            }
        }

        public virtual void RemoveService(string name)
        {
            if(services.TryGetValue(name,out IService servicer))
            {
                services.Remove(name);

                servicer.DoRemove();
            }
        }

        public virtual IService RetrieveService(string name)
        {
            return services.TryGetValue(name, out IService servicer) ? servicer : null;
        }
    }
}
