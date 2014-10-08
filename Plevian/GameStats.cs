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
        public static int AverageUnitCountPerVillage { get; private set; }
        public static int SumVillages { get; private set; }
        public static int SumUnits { get; private set; }

        private static int counter = 0;
        public static void collect()
        {
            // Don't collect stats every tick
            if (counter --> 0)
                return;
            counter = 10;
            Logger.log("Collecting stats");

            SumVillages = SumUnits = 0;

            foreach (Player player in Game.game.players)
            {
                foreach (Village village in player.villages)
                {
                    SumVillages++;
                    foreach (Unit unit in village.army)
                    {
                        SumUnits += unit.quantity;
                    }
                }
            }

            Logger.log("SumVillages = " + SumVillages);
            Logger.log("SumUnits = " + SumUnits);
            Logger.log("Avg = " + SumUnits / SumVillages);
            AverageUnitCountPerVillage = SumUnits / SumVillages;
        }
    }
}