﻿using Plevian.Battles;
using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Messages;
using Plevian.Players;
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
    public class AttackOrder : Order
    {
        private static Random rand = new Random();

        Resources loot = new Resources();

        protected AttackOrder(Village owner, Tile origin, Tile destination, Army army, OrderType type)
            : base(owner, origin, destination, army, type)
        {
        }
        public AttackOrder(Village owner, Tile origin, Tile destination, Army army)
            : this(owner, origin, destination, army, OrderType.ATTACK)
        {
        }

        protected override void onEnd()
        {
            if (Destination.type == TerrainType.PLAINS
            && army.contains(UnitType.SETTLER))
            {
                // Somebody taken over this tile? abort
                if (Destination.type != TerrainType.PLAINS)
                {
                    Game.Player.SendMessage("System", "Failed to create village", "The village has been created by someone else");
                    turnBack();
                    return;
                }

                Player player = Game.Player;
                Map map = Game.game.map;

                Village newVillage = new Village(Destination.location, player, "New village");
                map.place(newVillage);
                player.addVillage(newVillage);

                /* TODO: doesn't work
                Army settler = new Army();
                settler += UnitFactory.createUnit(UnitType.SETTLER, 1);
                army -= settler;
                newVillage.addArmy(army);
                */

                Game.Player.SendMessage("System", "Created village", newVillage.name + " created at " + newVillage.location);

                completed = true;
            }
            else if (isGoingBack == true)
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
                        onFightWin(afterReport);
                    else
                        onFightLose(afterReport);

                    Game.Player.SendMessage(new Message("Battle report : " + village.name, "Report", afterReport.ToString(), DateTime.Now));
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

            Battle battle = new Battle(army, defendingArmy, getLuck(), getDefense(village), getBaseDefense(village));
            return battle.makeBattle();
        }

        protected void onFightWin(Report report)
        {
            if (army.contains(UnitType.DUKE))
            {
                Village village = Destination as Village;
                Village attVillage = origin as Village;
                Player attacker = attVillage.Owner;
                attacker.CaptureVillage(village);
                completed = true;
                village.addArmy(army);
                report.villageCaptured = true;
            }
            else
            {
                gatherLoot(Destination as Village);
                report.loot = loot;
                turnBack();
            }
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

        private int getBaseDefense(Village village)
        {
            return village.getBaseDefense();
        }

        private float getDefense(Village village)
        {
            return village.getDefense();
        }

        private float getLuck()
        {
            return 0.5f + (float)rand.NextDouble();
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
                tooltip += "\n" + loot.ToString();
            return tooltip;
        }

    }
}
