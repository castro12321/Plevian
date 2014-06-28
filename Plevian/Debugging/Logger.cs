using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Debugging
{
    class Logger
    {
        private static const bool c = true;
        /// <summary>
        /// Logger for castro
        /// </summary>
        public static void c(String msg)
        {
            if (c)
                Console.WriteLine(msg);
        }
    }
}
