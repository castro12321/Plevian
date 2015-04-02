using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Debugging;
using Plevian.Resource;
using Plevian.Units;
using Plevian.Villages;

namespace Plevian.Players
{
    /// <summary>
    /// This module recruits warriors for our mighty army
    /// TODO:
    /// Each value is configurable and depends on:
    /// - AI type (aggressive, non-aggressive, economic, political, dunnoMegaProAiType)
    /// - Village type (economic, military, research, dunno)
    /// </summary>
    public class AiModuleRecruiting : AiModule
    {
        private static Random random = new Random();
        private static int k = 1000;

        public AiModuleRecruiting(AiPlayer ai)
            : base(ai)
        {
        }

        public override void tick()
        {
            foreach (Village village in ai.villages)
                tick(village);
        }


        private void tick(Village village)
        {
            // Don't recruit too many units simultanously. Too long queue may lower our response capabilities
            if (village.queues.recruitQueue.Count > GameTime.speed)
                return;

            // 1) What kind of units do we need?
            // 1.1) Well... We need all of them! We'll filter out them below
            List<Unit> toRecruit = UnitFactory.createAllUnits(1);

            // 2) Check our budget
            // 2.1) Don't recruit units if we are poor
            int townHallLevel = village.getBuildingLevel(Buildings.BuildingType.TOWN_HALL);
            if (!village.resources.canAfford(minimumDepositForRecruitingByTownHallLevel[townHallLevel]))
                return;
            // 2.2) Make sure that we can technically recruit the unit (cost, technology requirements, etc)
            foreach (Unit unit in toRecruit)
                if (!village.canRecruit(unit))
                    toRecruit.Remove(unit);
            // 2.3) Don't recruit units that we cannot afford (budget is usually smaller than the resources stored in the village)
            float danger = GameStats.HowDangerousTheAreaIsFor(village); // Moved up to here to avoid rechecking it multiple times in functions below
            Resources budget = getBudgetForRecruiting(village, danger);
            foreach (Unit unit in toRecruit)
                if (!budget.canAfford(unit.getWholeUnitCost()))
                    toRecruit.Remove(unit);
            // 2.4) Don't recruit units that we cannot upkeep (well, units need to eat. Can we provide enough resources for them and they fit in our budget?)
            Resources upkeepBudget = getBudgetForUpkeep(village, danger);
            foreach (Unit unit in toRecruit)
                if (!upkeepBudget.canAfford(unit.baseUpkeepCost))
                    toRecruit.Remove(unit);

            // 3) From the list of available to recruit units, choose only that, wchich is the most important now
            Unit toRecruitUnit = chooseTheMostImportantUnit(village, toRecruit, danger);

            village.recruit(toRecruitUnit);
        }

        // Require to have at least 'X' resources stored in the village for given town hall level
        private static Resources[] minimumDepositForRecruitingByTownHallLevel = 
        {
            new Resources(250, 250, 250, 250), // Level 0
            new Resources(1000, 1000, 500, 500), // Level 1
            new Resources(2000, 2000, 2000, 1000), // Level 2
            new Resources(6000, 6000, 3000, 2000), // Level 3
            new Resources(12*k, 12*k, 8*k, 8*k), // 4
            new Resources(20*k, 20*k, 20*k, 20*k), // 5
        };

        // This block calculates how much of the stored resources can we spend on recruting. 1% precision is enough IMO
        private Resources peaceResourcesBudgetForArmy = new Resources(60, 50, 40, 30);
        private Resources warResourcesBudgetForArmy = new Resources(80, 70, 60, 50);
        private Resources getBudgetForRecruiting(Village village, float danger)
        {
            Resources storedResources = village.resources;

            // Budget for the army is based on the surrounding area. If we are safe, the budget is lower. If we have a war, the budget is higher
            Resources budgetPercent = Interpolate(peaceResourcesBudgetForArmy, warResourcesBudgetForArmy, (float)danger);

            // This is the budget for army. It's internally represented in '%' so we have to convert it to the real Resources
            return (storedResources * 100) / budgetPercent;
        }

        // This block calculates how much of the current production can we spend on upkeep
        // aka Don't spend more than 'X%' prodction on army upkeep
        // Food should be a little higher than other values, because almost every unit in the game eats food lol
        // Wood and Iron: Try different values, dunno
        // Stone: Set rather lower value; it limits number of rams and catapults in the village
        private Resources peaceUpkeepBudget = new Resources(60, 40, 40, 20);
        private Resources warUpkeepBudget = new Resources(80, 60, 60, 40);
        private Resources getBudgetForUpkeep(Village village, float danger)
        {
            Resources production = village.TotalIncome(1);

            // Budget for the army is based on the surrounding area. If we are safe, the budget is lower. If we have a war, the budget is higher
            Resources budgetPercent = Interpolate(peaceUpkeepBudget, warUpkeepBudget, (float)danger);

            Resources productionBudget = (production * 100) / budgetPercent;

            // Part of the production may already be in use by the army upkeep (well, units need to eat)
            // So to get remaining budget, we need to substract current upkeep
            return productionBudget - village.army.Upkeep;
        }

        private const float focusMilitary = 1.0f; // recruit importance modifier set by higher-level AI layer
        private const float focusEconomy = 1.0f; // recruit importance modifier set by higher-level AI layer
        private float getAiImportanceForBase(Village village, Unit unit, float danger)
        {
            // Set base importance
            float importance;
            switch (unit.unitType)
            {
                case UnitType.WARRIOR: importance = 100; break;
                case UnitType.ARCHER: importance = 50; break;
                case UnitType.KNIGHT: importance = 30; break;
                case UnitType.RAM: importance = 3; break;
                case UnitType.TRADER: importance = 1; break;
                case UnitType.DUKE: importance = 0.5f; break;
                case UnitType.SETTLER: importance = 0.1f; break;
                default: throw new ArgumentException("UnitType not supported: " + unit.unitType);
            }

            if (unit.unitPurpose == UnitPurpose.ECONOMIC)
                importance *= focusEconomy;
            else if (unit.unitPurpose == UnitPurpose.MILITARY)
                importance *= focusMilitary;
            else
                throw new ArgumentException("UnitPurpose not supported: " + unit.unitPurpose);

            // Check how dangerous the area around is (in relation to our village)
            // - Dangerous --> Recruit more military, less economic
            // - Safe --> recruit more economic, less military (or go fighting, lol)
            while ((danger -= 0.1f) > 0.0f) // The more dangerous, the more times it'll loop
            {
                switch (unit.unitPurpose)
                {
                    case UnitPurpose.ECONOMIC:
                        importance *= 0.9f;
                        break;
                    case UnitPurpose.MILITARY:
                        importance *= 1.1f;
                        break;
                    default:
                        throw new ArgumentException("UnitPurpose not supported: " + unit.unitPurpose);
                }
            }

            return importance;
        }

        private Unit chooseTheMostImportantUnit(Village village, List<Unit> toRecruit, float danger)
        {
            List<double> toRecruitProbability = new List<double>();
            double probabilitySum = 0.0d;
            foreach (Unit unit in toRecruit)
                toRecruitProbability.Add(probabilitySum += getAiImportanceFor(village, unit, danger));
            double randomized = random.NextDouble() * probabilitySum;
            for (int i = 0; i < toRecruitProbability.Count; ++i)
                if (toRecruitProbability[i] > randomized)
                    return toRecruit[i];
            throw new Exception("You are shit outta luck. I have no idea what happened");
        }

        private float getAiImportanceFor(Village village, Unit unit, float danger)
        {
            float importance = getAiImportanceForBase(village, unit, danger);

            // Gather some statistic data for later calculation
            Army villageArmy = village.army;
            int armySize = villageArmy.size();
            float sumImportance = 0.0f;
            foreach (Unit tmpUnit in villageArmy)
                if(tmpUnit.quantity > 0)
                    sumImportance += getAiImportanceForBase(village, tmpUnit, danger);

            // We are aiming to fullfill "importance / sumImportance = unit.quantity / armySize" equation
            // If there is too much specific units in the army, lower its AiImportance
            float importanceRatio = importance / sumImportance;
            float sizeRatio = (float)villageArmy.get(unit.unitType).quantity / (float)villageArmy.size();

            // For each 10% of 'too much', lower AiImportance by 10%
            while ((sizeRatio -= 0.1f) > 0.0f)
                importance *= 0.9f;

            return importance;
        }

        /// <returns>Value between pointA and pointB specified by betweePercent[0..1] argument</returns>
        private float Interpolate(float pointA, float pointB, float betweenPercent)
        {
            float min = Math.Min(pointA, pointB);
            float max = Math.Max(pointA, pointB);
            float interpolated = ((max - min) / betweenPercent) + min;
            return interpolated;
        }

        /// <returns>Resources with its components (food, wood, iron, stone) interpolated between resA and resB specified by betweenPercent[0..1] argument</returns>
        private Resources Interpolate(Resources resA, Resources resB, float betweenPercent)
        {
            return new Resources(
                (int)Interpolate(resA.food, resB.food, betweenPercent),
                (int)Interpolate(resA.wood, resB.wood, betweenPercent),
                (int)Interpolate(resA.iron, resB.iron, betweenPercent),
                (int)Interpolate(resA.stone, resB.stone, betweenPercent)
            );
        }
    }
}
