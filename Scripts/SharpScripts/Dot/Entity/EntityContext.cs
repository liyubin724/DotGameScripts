using Dot.Core.Generic;
using Dot.Entity.Factory;
using Dot.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dot.Entity
{
    public class EntityContext
    {
        private Transform rootTran = null;
        public Transform RootTransfrom { get => rootTran; }

        private UniqueIDCreator idCreator = new UniqueIDCreator();
        private EntityObjectFactory entityObjectFactory = new EntityObjectFactory();
        public EntityContext()
        {
            rootTran = DontDestroyHandler.CreateTransform("Entity Context Root");

        }
    }
}
