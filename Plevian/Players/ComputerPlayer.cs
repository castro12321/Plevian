using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Players
{
    class ComputerPlayer : Player
    {
        public override void tick()
        {
            base.tick();

            foreach (Village village in villages)
                DoVillage(village);
        }

        private void DoVillage(Village village)
        {
        }
    }
}
