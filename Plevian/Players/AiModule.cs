using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Players
{
    public abstract class AiModule
    {
        protected readonly AiPlayer ai;

        public AiModule(AiPlayer ai)
        {
            this.ai = ai;
        }

        public abstract void tick();
    }
}
