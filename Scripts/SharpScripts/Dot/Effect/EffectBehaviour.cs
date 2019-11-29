using Dot.Core.Pool;
using UnityEngine;

namespace Dot.Core.Effect
{
    public class EffectBehaviour : GameObjectPoolItem
    {
        public void Play()
        {
            if (!CachedGameObject.activeSelf)
            {
                CachedGameObject.SetActive(true);
            }

        }

        public void Stop()
        {

        }

        public void Dead()
        {
            CachedGameObject.SetActive(false);
        }

        public override void DoSpawned()
        {
            
        }

        public override void DoDespawned()
        {

        }
    }
}
