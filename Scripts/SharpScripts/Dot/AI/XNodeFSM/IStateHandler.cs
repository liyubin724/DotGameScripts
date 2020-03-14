using System;
using System.Collections.Generic;
using System.Reflection;
using SystemObject = System.Object;

namespace Dot.AI.XNodeFSM
{
    public interface IStateHandler
    {
        void DoInitilized(SystemObject context);
        void DoEnter(StateBase from);
        void DoExist(StateBase to);
        void DoUpdate(float deltaTime);
    }

    public static class StateHanlderCache
    {
        private static Dictionary<string, Type> handlerTypeDic = new Dictionary<string, Type>();

        public static IStateHandler GetStateHandler(string handlerClassName)
        {
            if(!handlerTypeDic.TryGetValue(handlerClassName,out Type type))
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach(var assembly in assemblies)
                {
                    string assemblyName = assembly.GetName().Name;
                    if(assemblyName.StartsWith("UnityEngine")
                        || assemblyName.StartsWith("System"))
                    {
                        continue;
                    }

                    Type[] types = assembly.GetTypes();
                    foreach(var t  in types)
                    {
                        if(t.Name == handlerClassName && typeof(IStateHandler).IsAssignableFrom(t))
                        {
                            type = t;
                            break;
                        }
                    }

                    if(type!=null)
                    {
                        break;
                    }
                }
                handlerTypeDic.Add(handlerClassName, type);
            }

            if(type == null)
            {
                return null;
            }

            return Activator.CreateInstance(type) as IStateHandler;

        }
    }
}
