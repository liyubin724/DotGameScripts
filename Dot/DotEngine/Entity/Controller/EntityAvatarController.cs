using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dot.Entity.Controller
{
    public class EntityAvatarController : EntityController
    {
        protected override void OnInitialized()
        {
            
        }

        protected Transform RootTransform
        {
            get
            {
                return entity.GetController<EntityGameObjectController>().RootTransform;
            }
        }


    }
}
