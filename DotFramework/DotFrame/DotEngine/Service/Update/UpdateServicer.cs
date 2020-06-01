using System;
using System.Collections.Generic;

namespace DotEngine.Service.Update
{
    public class UpdateServicer : IUpdateServicer
    {
        private List<Action<float>> updateHandlers = new List<Action<float>>();
        private List<Action<float>> unscaleUpdateHandlers = new List<Action<float>>();
        private List<Action<float>> lateUpdateHandlers = new List<Action<float>>();
        private List<Action<float>> fixedUpdateHandlers = new List<Action<float>>();

        public void AddFixedUpdateHandler(Action<float> fixedUpdate)
        {
            fixedUpdateHandlers.Add(fixedUpdate);
        }

        public void AddLateUpdateHandler(Action<float> lateUpdate)
        {
            lateUpdateHandlers.Add(lateUpdate);
        }

        public void AddUnscaleUpdateHandler(Action<float> unscaleUpdate)
        {
            unscaleUpdateHandlers.Add(unscaleUpdate);
        }

        public void AddUpdateHandler(Action<float> update)
        {
            updateHandlers.Add(update);
        }

        public void DoDispose()
        {
            updateHandlers.Clear();
            unscaleUpdateHandlers.Clear();
            lateUpdateHandlers.Clear();
            fixedUpdateHandlers.Clear();
        }

        public void DoFixedUpdate(float deltaTime)
        {
            foreach(var handler in fixedUpdateHandlers)
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

        public void DoStart()
        {
            
        }

        public void DoUnscaleUpdate(float deltaTime)
        {
            foreach (var handler in unscaleUpdateHandlers)
            {
                handler.Invoke(deltaTime);
            }
        }

        public void DoUpdate(float deltaTime)
        {
            foreach (var handler in updateHandlers)
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
    }
}
