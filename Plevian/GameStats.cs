using Plevian.Buildings;
using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Messages;
using Plevian.Players;
using Plevian.RequirementS;
using Plevian.Units;
using Plevian.Villages;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plevian
{
    public class GameStats
    {
        private static Random random = new Random();

        public static float AverageUnitCountPerVillage { get; private set; }
        public static int SumVillages { get; private set; }
        public static int SumUnits { get; private set; }

        private static int counter = 0;
        public static void collect()
        {
            // Don't collect stats every tick
            if (counter --> 0)
                return;
            counter = 0; // Delay stats collecting (Maybe delay for 1 tick every 10 villages are created up to 10 ticks?)
            Logger.stats("Collecting stats");

            SumVillages = SumUnits = 0;

            foreach (Player player in Game.game.players)
            {
                foreach (Village village in player.villages)
                {
                    SumVillages++;
                    SumUnits += village.army.size();
                }
            }

            Logger.stats("SumVillages = " + SumVillages);
            Logger.stats("SumUnits = " + SumUnits);
            Logger.stats("Avg = " + SumUnits / SumVillages);
            AverageUnitCountPerVillage = (float)SumUnits / (float)SumVillages;
        }

        /// <param name="village">Village to check the dangerous level against</param>
        /// <returns>Dangerous level from 0.0f to 1.0f</returns>
        public static float HowDangerousTheAreaIsFor(Village village)
        {
            if (AverageUnitCountPerVillage <= 0)
                return 0;

            // First of all, we must remove our village from calculations
            int SumUnits2 = SumUnits - village.army.size();
            int SumVillages2 = SumVillages - 1;
            float AverageUnitCountPerVillage2 = (float)SumUnits2 / (float)SumVillages2;

            // Now we can assess our strength
            float relativeArmySizeToGlobalAverage = (float)village.army.size() / AverageUnitCountPerVillage2;

            // Ignore negligible differences
            if (relativeArmySizeToGlobalAverage >= 0.95)
                return 1.0f;

            // TODO: Think of some better algorithm?
            // enemy power = 5 nearest enemy villages army power
            // our power = this village + nearest our villages (not further away than enemy ones)
            // ally power = nearest ally villages (not further away than enemy villages)
            // danger = enemy power - our power - (ally power / some_magic_number like 4)
            float safe = relativeArmySizeToGlobalAverage;
            if (relativeArmySizeToGlobalAverage < 0.9)
                safe *= relativeArmySizeToGlobalAverage * relativeArmySizeToGlobalAverage;
            if (relativeArmySizeToGlobalAverage < 0.83)
                safe *= relativeArmySizeToGlobalAverage * relativeArmySizeToGlobalAverage;
            if (relativeArmySizeToGlobalAverage < 0.75)
                safe *= relativeArmySizeToGlobalAverage * relativeArmySizeToGlobalAverage;

            float danger = 1 - safe;

            if (danger < 0)
                danger = 0;
            else if (danger > 1) // In case of magic
                danger = 1;
            return danger;
        }
    }
}