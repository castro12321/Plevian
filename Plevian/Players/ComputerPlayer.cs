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
                DoVillage(village);
                DoAttacks(village);
            }
        }

        private BuildingType RandomBuildingType()
        {
            Array values = Enum.GetValues(typeof(BuildingType));
            return (BuildingType)values.GetValue(random.Next(values.Length));
        }

        private Unit RandomUnit(Village village)
        {
            Array values = Enum.GetValues(typeof(UnitType));
            UnitType type = (UnitType)values.GetValue(random.Next(values.Length));
            return UnitFactory.createUnit(type, 1);
        }

        private Technology RandomTechnology(Village village)
        {
            return technologies.technologies[random.Next(technologies.technologies.Count)];
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
            Logger.AI(name + " village tick:");

            // Most important - Buildings
            if(village.queues.buildingQueue.Count == 0)
            {
                BuildingType toBuildType = RandomBuildingType();
                Logger.AI("Trying build " + toBuildType);
                if(village.canBuild(toBuildType))
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

            // Second most important - Army
            if(village.queues.recruitQueue.Count == 0)
            {
                Unit toRecruit = RandomUnit(village);
                Logger.AI("Trying recruit " + toRecruit);
                if(village.canRecruit(toRecruit)
                && village.resources.canAfford(toRecruit.getWholeUnitCost() * toRecruit.getAiImportance()))
                {
                    Logger.AI("Recruiting " + toRecruit);
                    village.recruit(toRecruit);
               } 
            }

            // Least important - Technology
            if(village.queues.researchQueue.Count == 0)
            {
                Technology toResearch = RandomTechnology(village);
                Logger.AI("Trying research " + toResearch);
                if(village.canResearch(toResearch)
                && village.resources.canAfford(toResearch.Price * toResearch.getAiImportance()))
                {
                    Logger.AI("Researching " + toResearch);
                    village.research(toResearch);
                }
            }
        }

        private void DoAttacks(Village village)
        {
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

            ComputerPlayer owner = village.Owner as ComputerPlayer;

            foreach(var relation in relations.relations)
            {

            }
        }
    }
}
