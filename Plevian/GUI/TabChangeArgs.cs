using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.GUI
{
    public abstract class TabChangeArgs
    {    }

    public class EnterVillageArgs : TabChangeArgs
    {
        public Village villageEntered { get; private set; }
        public EnterVillageArgs( Village villageEntered )
        {
            this.villageEntered = villageEntered;
        }
    }
}
