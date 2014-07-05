using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Debugging
{
    class Logger
    {
        private static bool cas = true;
        private static bool shot = true;
        /// <summary>
        /// Logger for castro
        /// </summary>
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
    }
}
