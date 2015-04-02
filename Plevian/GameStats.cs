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
            counter = 10;
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
            float relativeArmySizeToGlobalAverage = (float)village.army.size() / AverageUnitCountPerVillage;

            // TODO: Think of some better algorithm?
            float danger = 1 - relativeArmySizeToGlobalAverage;

            if (danger < 0)
                danger = 0;
            else if (danger > 1) // In case of magic
                danger = 1;
            return danger;
        }
    }
}