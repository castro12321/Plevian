using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Players;

namespace Plevian.Debugging
{
    public class Logger
    {
        private static bool ai = false;
        private static bool cas = false;
        private static bool shot = false;
        private static bool logVillage = false;
        private static bool logGraphics = false;
        private static bool logStats = false;

        public static void AI(String msg) { if (ai) Console.WriteLine(msg); }
        public static void c(String msg) { if (cas) Console.WriteLine(msg); }
        public static void s(String msg) { if (shot) Console.WriteLine(msg); }
        public static void village(String msg) { if (logVillage) Console.WriteLine(msg); }
        public static void graphics(String msg) { if (logGraphics) Console.WriteLine(msg); }
        public static void stats(String msg) { if (logStats) Console.WriteLine(msg); }

        /// <summary>
        /// Always output as debug message
        /// </summary>
        public static void log(string msg)
        {
            Console.WriteLine(msg);
        }

        public static void warn(String msg)
        {
            Console.WriteLine("Warn: " + msg);
        }

        public static void turnOff()
        {
            ai = cas = shot = logVillage = logGraphics = false;
        }

        /// <summary>
        /// Called every tick from Game. It's just a place to collect most temporary logs easily in one place
        /// </summary>
        public static void tick()
        {
            //logRelations();
            //log("Speed: " + GameTime.speed + "; " + GameTime.uspeed);
        }

        #region logRelations
        public static void logRelations()
        {
            log("Relations:");
            foreach (Player p in Game.game.players)
            {
                AiPlayer player = p as AiPlayer;
                if (player == null)
                    continue;

                log("- " + player.name);
                foreach (KeyValuePair<Player, float> relation in player.relations.relations)
                    log("    - " + relation.Key + " --> " + relation.Value);
            }
        }
        #endregion
    }
}