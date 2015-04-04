using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Buildings;
using Plevian.Debugging;
using Plevian.Resource;
using Plevian.Villages;

namespace Plevian.Players
{
    public class AiModuleBuilding : AiModule
    {
        private static Random random = new Random();

        public AiModuleBuilding(AiPlayer ai)
            : base(ai)
        {
        }

        public override void tick()
        {
            foreach(Village village in ai.villages)
                DoBuilding(village);
        }

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
    }
}
