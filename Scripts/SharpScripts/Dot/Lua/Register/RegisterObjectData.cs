using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Dot.Lua.Register
{
    [Serializable]
    public class RegisterObjectData
    {
        public string name;
        public GameObject obj;
        public UnityObject regObj;
        public string typeName;
    }
}
