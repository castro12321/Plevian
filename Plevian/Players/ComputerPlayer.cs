using Plevian.Buildings;
using Plevian.Debugging;
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
    public class ComputerPlayer : Player
    {
        private static Random random = new Random();
        private AiRelations relations = new AiRelations();

        public ComputerPlayer(String name, SFML.Graphics.Color color)
            : base(name, color)
        {
        }

        public override void tick()
        {
            base.tick();
            relations.step();

            foreach (Village village in villages)
            {
                Logger.AI(name + " village tick:");
                DoBuilding(village);
                DoRecruiting(village);
                DoResearching(village);
                DoAttacks(village);
            }
        }

        private void DoVillage(Village village)
        {
            

            /* Main ideas:
             * 
             * - Build/Recruit/Research only one unit at a time
             * - should we Build/Recruit/Research? = price * AiImportance 
             * --> The bigger the number, the more spare resources we need to process
             * --> So Farm/Mine/etc are the number 1 AiImportance so build them as soon as we have resources
             * --> Other are less important
             */
        }

        #region building
        /* Idea outline:
         * Just pick a random building.
         * Check if we are able to build it (requirements, resources, space, etc)
         * If so, check if we should build it by comparing some random number
         *     to the building importance (the chance of building it in %)
         * Yes? Do it
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
            // Maybe modify it later but for now it's good enough
            BuildingType toBuildType = RandomBuildingType();

            // Check if we are able to build it (requirements, resources, space, etc)
            Logger.AI("Trying build " + toBuildType);
            if (village.canBuild(toBuildType))
            {
                Resources basePrice = village.getPriceForNextLevel(toBuildType);
                Resources price = basePrice * village.getBuilding(toBuildType).getAiImportance();
                if (village.resources.canAfford(price))
                {
                    Logger.AI("Building " + toBuildType);
                    village.build(toBuildType);
                }
            }
        }
        #endregion

        #region recruiting
        /* Main idea:
         * 
         */

        private Unit RandomUnit(Village village)
        {
            Array values = Enum.GetValues(typeof(UnitType));
            UnitType type = (UnitType)values.GetValue(random.Next(values.Length));
            return UnitFactory.createUnit(type, 1);
        }

        private void DoRecruiting(Village village)
        {
            if (village.queues.recruitQueue.Count == 0)
            {
                Unit toRecruit = RandomUnit(village);
                Logger.AI("Trying recruit " + toRecruit);
                if (village.canRecruit(toRecruit)
                && village.resources.canAfford(toRecruit.getWholeUnitCost() * toRecruit.getAiImportance()))
                {
                    Logger.AI("Recruiting " + toRecruit);
                    village.recruit(toRecruit);
                }
            }
        }
        #endregion

        #region researching
        /* Main idea:
         * 
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
                Logger.AI("Trying research " + toResearch);
                if (village.canResearch(toResearch)
                && village.resources.canAfford(toResearch.Price * toResearch.getAiImportance()))
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
            ComputerPlayer owner = village.Owner as ComputerPlayer;

            foreach(var relation in relations.relations)
            {

            }
        }
        #endregion
    }
}
