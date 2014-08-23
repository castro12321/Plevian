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

        public AttackOrder(Tile origin, Tile destination, Army army)
            : base(origin, destination, army, OrderType.ATTACK)
        {

        }

        protected override void onEnd()
        {
            if (isGoingBack == true)
            {
                if (!(Destination is Village))
                    throw new Exception("Army returned to tile which is not Village!");
                Village village = Destination as Village;
                village.addArmy(army);
                village.addResources(loot);
                completed = true;
                Logger.s("AttackOrder completed!");
                
            }
            else
            {
                if (Destination is Village && origin is Village)
                {
                    Village village = Destination as Village;
                    Village attVillage = origin as Village;
                    if(village.Owner == attVillage.Owner)
                    {
                        if(onFriendlyVillageContact())
                        {
                            turnBack();
                            return;
                        }
                    }
                    Report afterReport = makeBattle();

                    if (afterReport.battleResult == BattleState.AttackerVictory)
                    {
                        onFightWin(afterReport);
                        
                    } else
                    {
                        onFightLose(afterReport);
                    }

                    Game.player.SendMessage(new Message("Battle report : " + village.name, "Report", afterReport.ToString(), DateTime.Now));
                }
                else
                {
                    throw new Exception("Destination != Village OR origin != Village in attackOrder");
                }
            }
        }

        protected virtual Report makeBattle()
        {
            Village village = Destination as Village;
            Army defendingArmy = village.army;

            Battle battle = new Battle(army, defendingArmy, getLuck(), getDefense(), getBaseDefense());
            return battle.makeBattle();
        }

        protected virtual void onFightWin(Report report)
        {

            gatherLoot(Destination as Village);
            report.loot = loot;
            turnBack();
        }

        protected virtual void onFightLose(Report report)
        {
            completed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Return true if you want to turn back your army on friendly village encounter</returns>
        protected virtual bool onFriendlyVillageContact()
        {
            return true;
        }

        private int getBaseDefense()
        {
            return 0;
        }

        private float getDefense()
        {
            return 1f;
        }

        private float getLuck()
        {
            return 1f;
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

            int wood = (int)(capacity * woodP);
            int stone = (int)(capacity * woodP);
            int iron = (int)(capacity * woodP);
            int food = (int)(capacity * woodP);


            villageResources -= (new Wood(wood) + new Stone(stone) + new Iron(iron) + new Food(food));

            if(wood+stone+iron+food != capacity)
            {
                int more = (capacity - (wood + stone + iron + food));
                if(biggest == woodP && villageResources.wood > 0)
                {
                    wood += more;
                    villageResources -= new Wood(more);
                }
                else if (biggest == stoneP && villageResources.stone > 0)
                {
                    stone += more;
                    villageResources -= new Stone(more);
                }
                else if (biggest == ironP && villageResources.iron > 0)
                {
                    iron += more;
                    villageResources -= new Iron(more);

                }
                else if (biggest == foodP && villageResources.food > 0)
                {
                    food += more;
                    villageResources -= new Wood(more);

                }
            }

            if (villageResources.wood < 0)
            {
                wood += villageResources.wood;
                villageResources -= new Wood(villageResources.wood);
            }
            if (villageResources.stone < 0)
            {
                stone += villageResources.stone;
                villageResources -= new Stone(villageResources.stone);
            }
            if (villageResources.iron < 0)
            {
                iron += villageResources.iron;
                villageResources -= new Iron(villageResources.iron);
            }
            if (villageResources.food < 0)
            {
                food += villageResources.food;
                villageResources -= new Food(villageResources.food);
            }

            loot = new Wood(wood) + new Stone(stone) + new Iron(iron) + new Food(food);
        }

        public override string getTooltipText()
        {
            string tooltip =  base.getTooltipText();
            if(isGoingBack)
            {
                tooltip += "\n" + loot.ToString();
            }
            return tooltip;
        }

    }
}
