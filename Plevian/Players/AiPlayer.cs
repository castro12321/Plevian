using Plevian.Buildings;
using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Orders;
using Plevian.Resource;
using Plevian.TechnologY;
using Plevian.Units;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Players
{
    public class AiPlayer : Player
    {
        private static bool disabled = false;
        private static Random random = new Random();

        public AiModuleRelations relations;

        public AiPlayer(String name, SFML.Graphics.Color color)
            : base(name, color)
        {
            relations = new AiModuleRelations(this);
        }

        public override void tick()
        {
            base.tick();
            if (disabled)
                return;

            Logger.AI("AI tick " + name);
            relations.tick();


            foreach (Village village in villages)
            {
                Logger.AI(name + " village tick:");
                DoBuilding(village);
                DoRecruiting(village);
                DoResearching(village);
                DoAttacks(village);
                EstablishVillages(village);
            }
        }

        #region building
        /* Idea outline:
         * Just build a random building.
         * Build only if we have some spare resources
         * (upper don't apply to resource-related buildings - mine/farm/etc)
         */
        private BuildingType RandomBuildingType()
        {
            Array values = Enum.GetValues(typeof(BuildingType));
            return (BuildingType)values.GetValue(random.Next(values.Length));
        }

        private void DoBuilding(Village village)
        {
            // First of all, handle up to two buildings in the queue.
            // It provides us better flexibility/responsibility to changes
            if (village.queues.buildingQueue.Count >= 2)
                return;

            // Find a random building to build.
            BuildingType toBuildType = RandomBuildingType();

            // Check if we are able to build it (requirements, resources, space, etc)
            if (village.canBuild(toBuildType))
            {
                Resources basePrice = village.getPriceForNextLevel(toBuildType);
                Resources price = basePrice * village.getBuilding(toBuildType).getAiResourceModifier();
                if (village.resources.canAfford(price))
                {
                    Logger.AI("Building " + toBuildType);
                    village.build(toBuildType);
                }
            }
        }
        #endregion

        #region recruiting
        /* Idea outline:
         * Do we need to recruit more units?
         * let globalAverage be 100 - the global average of units per village
         * let x be units in our city
         * while(x < 50%) --> recruit a lot of units (95% chance of recruiting)
         * while(x < 95%) --> recruit only if we have free resources (30%)
         * while(x > 95%) --> avoid recruiting if not neccessary (5%)
         * 
         * Pick a random unit.
         * Check if we are able to build it (requirements, resources, space, etc)
         * If so, check if we should recruit by comparing some random number to the unit importance (the chance of building it in %)
         * Yes? Do it
         */

        private Unit RandomUnit(Village village)
        {
            Array values = Enum.GetValues(typeof(UnitType));
            UnitType type = (UnitType)values.GetValue(random.Next(values.Length));
            int quantityToRecruit = GameTime.speed;
            return UnitFactory.createUnit(type, quantityToRecruit);
        }

        private double RelativeArmySizeToAverage(Village village)
        {
            double armySize = village.army.size();
            return armySize / GameStats.AverageUnitCountPerVillage;
        }

        private void DoRecruiting(Village village)
        {
            double relativeArmySize = RelativeArmySizeToAverage(village);
            double chance = random.NextDouble();

            // TODO: profile <relativeArmySize> and <chance> values
            // Also add some function of region 'safety'. Safe regions don't need that much army
            if (relativeArmySize < 0.5d) // Do we have less than 50% of avg units per village?
            {
                if (chance < 0.5d) // 50% chance of recruiting
                    return;
            }
            else if(relativeArmySize < 0.9d) // 90%?
            {
                if (chance < 0.66d) // 33% chance of recruiting
                    return;
            }
            else if(relativeArmySize < 1.1d) // 110%?
            {
                if (chance < 0.85d) // 15% chance of recruiting
                    return;
            }
            else // Do we have more than 110% of avg units per village?
            {
                if (chance < 0.95d) // 5% chance of recruiting
                    return;
            }
            
            if (village.queues.recruitQueue.Count <= GameTime.speed)
            {
                Unit toRecruit = RandomUnit(village);
                
                if (village.canRecruit(toRecruit) // First of all. Are we able to recruit it?
                && random.NextDouble() < toRecruit.getAiImportance() // Some randomness
                && village.resources.canAfford(toRecruit.getWholeUnitCost() * toRecruit.getAiResourceModifier())) // Do we have spare resources?
                {
                    Logger.AI("Recruiting " + toRecruit);
                    village.recruit(toRecruit);
                }
            }
        }
        #endregion

        #region researching
        /* Main idea:
         * Just randomly research whatever you can...
         * Reasearch only if you have free resources
         */

        private Technology RandomTechnology(Village village)
        {
            return technologies.technologies[random.Next(technologies.technologies.Count)];
        }

        private void DoResearching(Village village)
        {
            if (village.queues.researchQueue.Count == 0)
            {
                Technology toResearch = RandomTechnology(village);
                if (village.canResearch(toResearch)
                && village.resources.canAfford(toResearch.Price * toResearch.getAiResourceModifier()))
                {
                    Logger.AI("Researching " + toResearch);
                    village.research(toResearch);
                }
            }
        }
        #endregion

        #region attacks
        /* Main ideas:
         * 
         * Deciding if we should attack
         * - Calculate average unit count out of the all villages on the map (once every 5 minute or so)
         * - If we have more than average; send 25%-75% of our army to steal cookies
         * 
         * Choosing target (priorities)
         * - Attacking enemy at war (~95% chance)
         * - Attacking neutral (~5% chance)
         * - Attacking ally (Less than 1% chance)
         * 
         * Relationship:
         * Add Map<Player, Relationship> for chances?
         * The worse the relationship, the more chance that we attack the player
         * -10 --> +10
         * When -10 --> ~95% chance of being attacked
         * When 0 --> Around 5% chance
         * When +10 --> Less than 1%
         * The relationship is going to be 0 over time.
         * Like relationship *= 0.999 per tick?
         */

        private void DoAttacks(Village village)
        {
            Logger.AI("Doing attacks");
            double relativeArmySize = RelativeArmySizeToAverage(village);
            double chance = random.NextDouble(); // Chance of attacking

            // TODO: profile <relativeArmySize> and <chance> values
            if (relativeArmySize < 0.5d) // Do we have less than 50% of avg units per village?
            {
                if (chance < 0.99d) // 1% chance of attacking
                    return;
            }
            else if (relativeArmySize < 0.9d) // 90%?
            {
                if (chance < 0.95d) // 5% chance of attacking
                    return;
            }
            else if (relativeArmySize < 1.1d) // 110%?
            {
                if (chance < 0.85d) // 15% chance of attacking
                    return;
            }
            else // Do we have more than 110% of avg units per village?
            {
                if (chance < 0.75d) // 25% chance of attacking
                    return;
            }
            
            // Didn't quit earlier? attack!
            //ComputerPlayer owner = village.Owner as ComputerPlayer;
            int randomized = random.Next(Game.game.players.Count);
            Player toAttack = Game.game.players[randomized];
            if (toAttack == this)
                return; // Don't attack ourselves xD
            if (toAttack.villages.Count == 0)
                return; // Well, shit happens. TODO: Remove players with no villages? (they actually lost the game, so no need to keep them)

            randomized = random.Next(-10, 10);
            if (randomized < relations.get(toAttack)) // The better your relations
                return; // The less chance being attacked

            var villageArmy = village.army.getUnits();
            Army attacking = new Army();
            
            foreach(var unit in villageArmy)
            {
                Unit toAdd = unit.Value.clone();
                toAdd.quantity = random.Next(toAdd.quantity/2, toAdd.quantity);
                attacking.add(toAdd);
            }

            if (attacking.size() < 10)
                return; // Don't send (almost) empty armies lol, that's dumb. Hey, AI! Don't be dumb!

            Village targetVillage = toAttack.villages[random.Next(0, toAttack.villages.Count)];
            Order order = new AttackOrder(village, village, targetVillage, attacking);
            village.addOrder(order);
        }
        #endregion

        #region villages
        /* Idea outline:
         * Do we have a settler? Send him to create a village! That's easy :P
         */
        private void EstablishVillages(Village village)
        {
            Logger.AI("Doing villages");

            if(village.army.contains(UnitType.SETTLER))
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
        #endregion
    }
}
