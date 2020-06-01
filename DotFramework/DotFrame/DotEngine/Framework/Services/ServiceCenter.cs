using System;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Framework.Services
{
    public class ServiceCenter : IServiceCenter
    {
        private Dictionary<string, IService> serviceDic = new Dictionary<string, IService>();

        private List<string> updateServiceNames = new List<string>();
        private List<string> unscaleUpdateServiceNames = new List<string>();
        private List<string> lateUpdateServiceNames = new List<string>();
        private List<string> fixedUpdateServiceNames = new List<string>();

        public ServiceCenter()
        {
        }

        public bool Has(string name)
        {
            return serviceDic.ContainsKey(name);
        }

        public void Register(string name, IService service)
        {
            if (!serviceDic.ContainsKey(name))
            {
                serviceDic.Add(name, service);

                Type serviceType = service.GetType();
                if (typeof(IUpdate).IsAssignableFrom(serviceType))
                {
                    updateServiceNames.Add(name);
                }
                else if (typeof(IUnscaleUpdate).IsAssignableFrom(serviceType))
                {
                    unscaleUpdateServiceNames.Add(name);
                }
                else if (typeof(ILateUpdate).IsAssignableFrom(serviceType))
                {
                    lateUpdateServiceNames.Add(name);
                }
                else if (typeof(IFixedUpdate).IsAssignableFrom(serviceType))
                {
                    fixedUpdateServiceNames.Add(name);
                }

                service.DoRegister();
            }
        }

        public void Remove(string name)
        {
            if (serviceDic.TryGetValue(name, out IService service))
            {
                service.DoRemove();
                serviceDic.Remove(name);
            }
        }

        public IService Retrieve(string name)
        {
            if (serviceDic.TryGetValue(name, out IService service))
            {
                return service;
            }
            return default;
        }

        public T Retrieve<T>(string name) where T : IService
        {
            object service = Retrieve(name);
            if (service != null)
            {
                return (T)service;
            }
            return default;
        }

        public void Clear()
        {
            List<IService> servicers = serviceDic.Values.ToList();
            servicers.ForEach((service) =>
            {
                service.DoRemove();
            });

            updateServiceNames.Clear();
            unscaleUpdateServiceNames.Clear();
            lateUpdateServiceNames.Clear();
            fixedUpdateServiceNames.Clear();

            serviceDic.Clear();
        }

        public void DoUpdate(float deltaTime)
        {
            for (int i = updateServiceNames.Count - 1; i >= 0; --i)
            {
                IService service = Retrieve(updateServiceNames[i]);
                if (service != null)
                {
                    ((IUpdate)service).DoUpdate(deltaTime);
                }
                else
                {
                    updateServiceNames.RemoveAt(i);
                }
            }
        }

        public void DoUnscaleUpdate(float deltaTime)
        {
            for (int i = unscaleUpdateServiceNames.Count - 1; i >= 0; --i)
            {
                IService service = Retrieve(unscaleUpdateServiceNames[i]);
                if (service != null)
                {
                    ((IUnscaleUpdate)service).DoUnscaleUpdate(deltaTime);
                }
                else
                {
                    unscaleUpdateServiceNames.RemoveAt(i);
                }
            }
        }

        public void DoLateUpdate(float deltaTime)
        {
            for (int i = lateUpdateServiceNames.Count - 1; i >= 0; --i)
            {
                IService service = Retrieve(lateUpdateServiceNames[i]);
                if (service != null)
                {
                    ((ILateUpdate)service).DoLateUpdate(deltaTime);
                }
                else
                {
                    lateUpdateServiceNames.RemoveAt(i);
                }
            }
        }

        public void DoFixedUpdate(float deltaTime)
        {
            for (int i = fixedUpdateServiceNames.Count - 1; i >= 0; --i)
            {
                IService service = Retrieve(fixedUpdateServiceNames[i]);
                if (service != null)
                {
                    ((IFixedUpdate)service).DoFixedUpdate(deltaTime);
                }
                else
                {
                    fixedUpdateServiceNames.RemoveAt(i);
                }
            }
        }
    }
}
