using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Debugging
{
    public class Logger
    {
        private static bool ai = true;
        private static bool cas = false;
        private static bool shot = false;
        private static bool logVillage = false;
        private static bool logGraphics = false;

        public static void AI(String msg)
        {
            if (ai) Console.WriteLine(msg);
        }

        public static void c(String msg)
        {
            if (cas)
                Console.WriteLine(msg);
        }

        public static void s(String msg)
        {
            if (!shot) return;
            Console.WriteLine(msg);
        }

        public static void village(String msg)
        {
            if (logVillage)
                Console.WriteLine(msg);
        }

        public static void graphics(String msg)
        {
            if (logGraphics)
                Console.WriteLine(msg);
        }

        /// <summary>
        /// Always output a debug message
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
    }
}
