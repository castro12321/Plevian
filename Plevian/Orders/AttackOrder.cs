using Plevian.Battles;
using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Messages;
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
                    if (afterReport.battleResult == BattleState.AttackerVictory)
                    {
                        gatherLoot(village);
                        afterReport.loot = loot;
                        turnBack();
                    } else
                    {
                        completed = true;
                    }

                    Game.player.SendMessage(new Message("Battle report : " + village.name, "Report", afterReport.ToString(), DateTime.Now));
                    
                }
            }
        }

        private void gatherLoot(Village village)
        {
            Resources villageResources = village.resources;
            int sumResources = villageResources.sumResources();
            int capacity = army.getLootCapacity();
            double woodP = (float)villageResources.wood / sumResources;
            double stoneP = (float)villageResources.stone / sumResources;
            double ironP = (float)villageResources.iron / sumResources;
            double foodP = (float)villageResources.food / sumResources;

            double biggest = woodP;
            if (stoneP > woodP) biggest = stoneP;
            if (ironP > biggest) biggest = ironP;
            if (foodP > biggest) biggest = foodP;

            int wood = (int)(villageResources.wood * woodP);
            int stone = (int)(villageResources.stone * woodP);
            int iron = (int)(villageResources.iron * woodP);
            int food = (int)(villageResources.food * woodP);


            villageResources -= (new Wood(wood) + new Stone(stone) + new Iron(iron) + new Food(food));

            if(wood+stone+iron+food != capacity)
            {
                int more = (capacity - (wood + stone + iron + food));
                if(biggest == woodP)
                {
                    wood += more;
                    villageResources -= new Wood(more);
                    if(villageResources.wood < 0)
                    {
                        wood += villageResources.wood;
                        villageResources -= new Wood(villageResources.wood);
                    }
                }
                else if(biggest == stoneP)
                {
                    stone += more;
                    villageResources -= new Stone(more);
                    if(villageResources.stone < 0)
                    {
                        stone += villageResources.stone;
                        villageResources -= new Stone(villageResources.stone);
                    }
                }
                else if(biggest == ironP)
                {
                    iron += more;
                    villageResources -= new Iron(more);
                    if(villageResources.iron < 0)
                    {
                        iron += villageResources.iron;
                        villageResources -= new Iron(villageResources.iron);
                    }
                }
                else if(biggest == foodP)
                {
                    food += more;
                    villageResources -= new Wood(more);
                    if(villageResources.food < 0)
                    {
                        food += villageResources.food;
                        villageResources -= new Food(villageResources.food);
                    }
                }
            }

            loot = new Wood(wood) + new Stone(stone) + new Iron(iron) + new Food(food);
        }

    }
}
