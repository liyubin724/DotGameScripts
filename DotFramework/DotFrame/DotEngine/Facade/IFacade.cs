using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Facade
{
    public interface IFacade
    {
        void DoUpdate(float detalTime);
        void DoUnscaleUpdate(float deltaTime);
        void DoLateUpdate(float deltaTime);
        void DoFixedUpdate(float deltaTime);

    }
}
