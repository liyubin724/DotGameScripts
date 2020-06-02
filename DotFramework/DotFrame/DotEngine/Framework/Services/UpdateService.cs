using System;
using System.Collections.Generic;

namespace DotEngine.Framework.Services
{
    public class UpdateService : IUpdateService
    {
        private List<Action<float>> updateHandlers = new List<Action<float>>();
        private List<Action<float>> unscaleUpdateHandlers = new List<Action<float>>();
        private List<Action<float>> lateUpdateHandlers = new List<Action<float>>();
        private List<Action<float>> fixedUpdateHandlers = new List<Action<float>>();

        public void AddUpdateHandler(Action<float> update)
        {
            updateHandlers.Add(update);
        }

        public void AddUnscaleUpdateHandler(Action<float> unscaleUpdate)
        {
            unscaleUpdateHandlers.Add(unscaleUpdate);
        }

        public void AddLateUpdateHandler(Action<float> lateUpdate)
        {
            lateUpdateHandlers.Add(lateUpdate);
        }

        public void AddFixedUpdateHandler(Action<float> fixedUpdate)
        {
            fixedUpdateHandlers.Add(fixedUpdate);
        }

        public void DoUpdate(float deltaTime)
        {
            foreach (var handler in updateHandlers)
            {
                handler.Invoke(deltaTime);
            }
        }

        public void DoUnscaleUpdate(float deltaTime)
        {
            foreach (var handler in unscaleUpdateHandlers)
            {
                handler.Invoke(deltaTime);
            }
        }

        public void DoFixedUpdate(float deltaTime)
        {
            foreach (var handler in fixedUpdateHandlers)
            {
                handler.Invoke(deltaTime);
            }
        }

        public void DoLateUpdate(float deltaTime)
        {
            foreach (var handler in lateUpdateHandlers)
            {
                handler.Invoke(deltaTime);
            }
        }

        public void RemoveFixedUpdateHandler(Action<float> fixedUpdate)
        {
            fixedUpdateHandlers.Remove(fixedUpdate);
        }

        public void RemoveLateUpdateHandler(Action<float> lateUpdate)
        {
            lateUpdateHandlers.Remove(lateUpdate);
        }

        public void RemoveUnscaleUpdateHandler(Action<float> unscaleUpdate)
        {
            unscaleUpdateHandlers.Remove(unscaleUpdate);
        }

        public void RemoveUpdateHandler(Action<float> update)
        {
            updateHandlers.Remove(update);
        }

        public void DoRegister()
        {
            
        }

        public void DoRemove()
        {
            updateHandlers.Clear();
            unscaleUpdateHandlers.Clear();
            lateUpdateHandlers.Clear();
            fixedUpdateHandlers.Clear();
        }
    }
}
