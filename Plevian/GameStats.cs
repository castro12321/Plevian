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
        public static int AverageUnitCount { get; private set; }

        private static int counter = 0;
        public static void collect()
        {
            if(counter --> 0)
                break;
            counter = 10;
            Logger.log("Collecting stats");
            asd;
        }
}
