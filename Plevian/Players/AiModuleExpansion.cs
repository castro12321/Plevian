using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Orders;
using Plevian.Units;
using Plevian.Villages;

namespace Plevian.Players
{
    class AiModuleExpansion : AiModule
    {
        private static Random random = new Random();

        public AiModuleExpansion(AiPlayer ai)
            : base(ai)
        {
        }

        public override void tick()
        {
            foreach(Village village in ai.villages)
                EstablishVillages(village);
        }

        /* Idea outline:
         * Do we have a settler? Send him to create a village! That's easy :P
         */
        private void EstablishVillages(Village village)
        {
            Logger.AI("Doing villages");

            if (village.army.contains(UnitType.SETTLER))
            {
                Logger.AI("- Got settler. Looking for target");
                Army settler = new Army();
                settler.add(new Settler(1));

                Tile target = Game.game.map.FindEmptyTile();
                Logger.AI("- Will establish a village at " + target.location.x + "/" + target.location.y);
                Order establishOrder = new AttackOrder(village, village, target, settler);
                village.addOrder(establishOrder);
            }
        }

    }
}
