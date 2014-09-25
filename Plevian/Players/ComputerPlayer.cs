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
    }
}
