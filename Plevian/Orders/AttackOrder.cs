using Plevian.Battles;
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

        public Resources loot = new Resources();

        public AttackOrder(Village owner, Tile origin, Tile destination, Army army)
            : base(owner, origin, destination, army, OrderType.ATTACK)
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
                        turnBack();
                        return;
                    }

                    Report afterReport = makeBattle();
                    if (afterReport.result == Report.BattleResult.AttackerVictory)
                        onFightWin(afterReport);
                    else
                        onFightLose(afterReport);
                    owner.Owner.SendMessage(new Message("Battle report : " + village.name, "Report", afterReport.ToString(), DateTime.Now));
                }
                else
                {
                    throw new Exception("Destination != Village OR origin != Village in attackOrder");
                }
            }
        }

        protected virtual Report makeBattle()
        {
            Village defending = Destination as Village;
            Army defenders = defending.army;
            float luck = 0.5f + (float)rand.NextDouble();
            int defendingBaseDefense = defending.getBaseDefense();
            float defendingDefense = defending.getDefense();
            return BattleResolver.Fight(army, defenders);
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

        private void gatherLoot(Village village)
        {
            Resources villageResources = village.resources;
            long sumResources = villageResources.sumResources();
            long capacity = army.getLootCapacity();
            if (capacity >= sumResources)
            {
                loot = villageResources;
                return;
            }

            // Concrete resource percent of all resources
            double foodP = (double)villageResources.food / (double)sumResources;
            double woodP = (double)villageResources.wood / (double)sumResources;
            double ironP = (double)villageResources.iron / (double)sumResources;
            double stoneP = (double)villageResources.stone / (double)sumResources;

            Logger.log("loot vr: " + villageResources);
            Logger.log("loot sr: " + sumResources);
            Logger.log("loot lc: " + capacity);
            Logger.log("% " + foodP + "; " + woodP + "; " + ironP + "; " + stoneP + " sum: " + (foodP+woodP+ironP+stoneP));
            
            // Take resources accordingly to the percents above
            long food = (int)(capacity * foodP);
            long wood = (int)(capacity * woodP);
            long iron = (int)(capacity * ironP);
            long stone = (int)(capacity * stoneP);
            Logger.log("loot1: " + food + "; " + wood + "; " + iron + "; " + stone);

            // Check if we took more resources than the village have
            bool retry = true;
            while (retry)
            {
                retry = false;
                if (food > villageResources.food)
                {
                    long free = food - villageResources.food;
                    food -= free;
                    wood += free / 3;
                    iron += free / 3;
                    stone += free / 3;
                    retry = true;
                }
                if (wood > villageResources.wood)
                {
                    long free = wood - villageResources.wood;
                    food += free / 3;
                    wood -= free;
                    iron += free / 3;
                    stone += free / 3;
                    retry = true;
                }
                if(iron > villageResources.iron)
                {
                    long free = iron - villageResources.iron;
                    food += free / 3;
                    wood += free / 3;
                    iron -= free;
                    stone += free / 3;
                    retry = true;
                }
                if (stone > villageResources.stone)
                {
                    long free = stone - villageResources.stone;
                    food += free / 3;
                    wood += free / 3;
                    iron += free / 3;
                    stone -= free;
                    retry = true;
                }
            }
            
            // All is done!
            Logger.log("loot2: " + food + "; " + wood + "; " + iron + "; " + stone);
            villageResources -= (new Wood(wood) + new Stone(stone) + new Iron(iron) + new Food(food));
            loot = new Wood(wood) + new Stone(stone) + new Iron(iron) + new Food(food);
        }

        public override string getTooltipText()
        {
            if (isGoingBack)
                return base.getTooltipText() + "\n" + loot.ToString();
            return base.getTooltipText();
        }

    }
}
