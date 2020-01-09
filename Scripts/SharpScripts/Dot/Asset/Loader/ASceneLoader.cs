using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Asset
{
    public abstract class ASceneLoader
    {
        protected AAssetLoader assetLoader = null;

        protected ASceneLoader(AAssetLoader loader)
        {
            assetLoader = loader;
        }

        internal void LoadScene()
        {

        }

        internal void UnloadScene()
        {

        }
    }
}
