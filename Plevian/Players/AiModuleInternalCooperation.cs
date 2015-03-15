using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Debugging;
using Plevian.Orders;
using Plevian.Resource;
using Plevian.Units;
using Plevian.Villages;

namespace Plevian.Players
{
    /// <summary>
    /// By 'resources' i mean both currency (wood, food, stone, iron) and army
    /// 
    /// This module coordinates economic/military cooperation between AI villages.
    /// Villages that produce too much resources, send their surplus to villages with less production
    /// 
    /// Each village get their surplus level calculated somehow (implementation defined)
    /// If that level differ between villages too much (let's say 25%),
    /// then villages with higher production send resources to those with lower production
    /// 
    /// Surplus calculation ideas:
    /// - (Currently implemented) Saved somewhere (here, or in the AI config file) list of expected maximum resources based on this village's 'Town Hall' level
    /// - Expenditures/production ratio
    /// 
    /// It looks like charity donation xD
    /// </summary>
    public class AiModuleInternalCooperation : AiModule
    {
        private static Random random = new Random();

        public AiModuleInternalCooperation(AiPlayer ai)
            : base(ai)
        {
        }

        /// <summary>
        /// Essentialy, for each village:
        /// - Check if the village has too much resources
        /// - If so, send those resources to a village that needs them
        /// </summary>
        public override void tick()
        {
            foreach(Village village in ai.villages)
            {
                tickResourcesCooperation(village);
                // TODO: similarly add military cooperation
            }
        }

        private void tickResourcesCooperation(Village village)
        {
            // No traders, then cannot send resources, simple
            if (!village.army.contains(Units.UnitType.TRADER))
                return;

            // Do we have more resources than we need?
            Resources expected = getExpectedResourcesFor(village);
            Resources toSend = village.resources - expected;
            if (toSend.food + toSend.wood + toSend.iron + toSend.stone <= 500)
                return; // Don't send negligible amounts of resources

            // Check how much does the random village need
            Village randomAiVillage = ai.villages[random.Next(ai.villages.Count)];
            if (village == randomAiVillage)
                return;
            Resources randomExpected = getExpectedResourcesFor(randomAiVillage);
            if (!randomAiVillage.resources.canAfford(randomExpected * 0.5f))
            {
                // Don't send too much resources though
                if (toSend.food > randomExpected.food)
                    toSend.food = randomExpected.food;
                if (toSend.wood > randomExpected.wood)
                    toSend.wood = randomExpected.wood;
                if (toSend.iron > randomExpected.iron)
                    toSend.iron = randomExpected.iron;
                if (toSend.stone > randomExpected.stone)
                    toSend.stone = randomExpected.stone;

                Army trader = UnitFactory.createArmy(UnitType.TRADER, 1); // We have a trader inside the village (checked above) so it should be safe
                Order order = new TradeOrder(village, village, randomAiVillage, trader, toSend, null);
                village.addOrder(order);
            }
        }

        private Resources getExpectedResourcesFor(Village village)
        {
            int townHallLevel = village.getBuildingLevel(Buildings.BuildingType.TOWN_HALL);
            return expectedResourcesByTownHallLevel[townHallLevel]; // Don't check for bounds. If the app crashes, just add more levels in expectedResourcesByTownHallLevel array
        }

        private static int k = 1000;
        private static Resources[] expectedResourcesByTownHallLevel = 
        {
            // TODO: profile those values for best AI performance (see AI tests on trello?)
            //            food, wood, iron, stone
            new Resources(1000, 1000, 1000, 1000), // Level 0
            new Resources(3000, 3000, 2000, 2000), // Level 1
            new Resources(9000, 9000, 5000, 5000), // Level 2
            new Resources(25*k, 25*k, 12*k, 12*k), // Level 3
            new Resources(90*k, 90*k, 40*k, 40*k), // Level 4
            new Resources(500*k, 500*k, 200*k, 200*k), // Level 5
        };
    }
}
