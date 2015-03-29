using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Debugging;
using Plevian.Units;
using Plevian.Villages;

namespace Plevian.Players
{
    public class AiModuleRecruiting : AiModule
    {
        private static Random random = new Random();

        public AiModuleRecruiting(AiPlayer ai)
            : base(ai)
        {
        }

        public override void tick()
        {
            foreach(Village village in ai.villages)
                DoRecruiting(village);
        }

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
            else if (relativeArmySize < 0.9d) // 90%?
            {
                if (chance < 0.66d) // 33% chance of recruiting
                    return;
            }
            else if (relativeArmySize < 1.1d) // 110%?
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
    }
}
