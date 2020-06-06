using System;
using UnityEngine;

namespace DotEngine.GOPool
{
    public static class GameObjectPoolConst
    {
        internal static readonly string LOGGER_NAME = "GOPool";

        internal static readonly string MANAGER_NAME = "GOService";
        internal static readonly string GROUP_NAME_FORMAT = "Group{0}";

        public static Func<string, GameObject, GameObject> InstantiateAsset;
    }
}
