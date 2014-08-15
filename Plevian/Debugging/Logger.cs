using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Debugging
{
    class Logger
    {
        private static bool cas = false;
        private static bool shot = true;
        private static bool logVillage = false;
        private static bool logGraphics = false;

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
    }
}
