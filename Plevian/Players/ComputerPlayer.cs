using Plevian.Buildings;
using Plevian.Debugging;
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

        public ComputerPlayer(String name, SFML.Graphics.Color color)
            : base(name, color)
        {
        }

        public override void tick()
        {
            base.tick();

            foreach (Village village in villages)
                DoVillage(village);
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
            Logger.AI(name + " village tick:");

            // Most important - Buildings
            if(village.queues.buildingQueue.Count == 0)
            {
                BuildingType toBuildType = RandomBuildingType();
                Logger.AI("Trying build " + toBuildType);
                if(village.canBuild(toBuildType))
                {
                    switch(toBuildType)
                    {
                        // Allow buildings that produce resources
                        case BuildingType.MINE:
                        case BuildingType.FARM:
                        case BuildingType.LUMBER_MILL:
                            Logger.AI("Building res " + toBuildType);
                            village.build(toBuildType);
                            break;
                        // For other buildings make sure that you have at least 5x more resources than needed
                        default:
                        {
                            if (village.resources.canAfford(village.getPriceForNextLevel(toBuildType) * 5))
                            {
                                Logger.AI("Building " + toBuildType);
                                village.build(toBuildType);
                            }
                            break;
                        }
                    }
                }
            }

            // Second most important - Army
            if(village.queues.recruitQueue.Count == 0)
            {
                Unit toRecruit = RandomUnit(village);
                Logger.AI("Trying recruit " + toRecruit);
                if(village.canRecruit(toRecruit)
                && village.resources.canAfford(village.resources * 15))
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
                && village.resources.canAfford(toResearch.Price * 5))
                {
                    Logger.AI("Researching " + toResearch);
                    village.research(toResearch);
                }
            }
        }
    }
}
