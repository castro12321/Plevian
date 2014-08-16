using Plevian.Battles;
using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Resource;
using Plevian.Units;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Orders
{
    class AttackOrder : Order
    {
        Resources loot = new Resources();

        public AttackOrder(Tile origin, Tile destination, float timePerTile, Army army)
            : base(origin, destination, army)
        {

        }

        protected override void onEnd()
        {
            if (isGoingBack == true)
            {
                if (!(destination is Village))
                    throw new Exception("Army returned to tile which is not Village!");
                Village village = destination as Village;
                village.addArmy(army);
                village.addResources(loot);
                completed = true;
                Logger.s("AttackOrder completed!");
                
            }
            else
            {
                if (destination is Village)
                {
                    Village village = destination as Village;
                    Army defendingArmy = village.army;

                    Battle battle = new Battle(army, defendingArmy, 1f, 1f, 0);
                    Report afterReport = battle.makeBattle();
                    
                    //Todo -> send report to some kind of message system.
                    Logger.s(afterReport.ToString());
                    if (afterReport.battleResult == BattleState.AttackerVictory)
                    {
                        gatherLoot();
                        turnBack();
                    } else
                    {
                        completed = true;
                    }
                    
                }
            }
        }

        private void gatherLoot()
        {
            Logger.s("GatherLoot() in AttackOrder not implemented");
        }

    }
}
