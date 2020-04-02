using System;
using UnityEngine;

namespace Dot.Map.Objects
{
    [Serializable]
    public class ObjectPlacementData
    {
        public string parentName = string.Empty;
        public Vector3 localPosition = Vector3.zero;
        public Vector3 localRotation = Vector3.zero;
        public Vector3 localScale = Vector3.one;
    }
}
