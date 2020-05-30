using Dot.GOPool;
using Dot.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Entity
{
    public class EntityManager : Singleton<EntityManager>
    {
        private GameObjectPool entityGameObjectPool = null;

        protected override void DoInit()
        {
            
        }

    }
}
