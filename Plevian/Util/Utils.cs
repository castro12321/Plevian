using Plevian.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Util
{
    public static class Utils
    {
        public static Random random = new Random();

        private static double rtick, gtick, btick,
            rstep = random.NextDouble() / 400 + 0.005,
            gstep = random.NextDouble() / 400 + 0.005,
            bstep = random.NextDouble() / 400 + 0.005;
        /// <summary>Generates smooth color transition</summary>
        /// <returns>Generated color</returns>
        public static SFML.Graphics.Color smoothSFMLColor()
        {
            double r = Math.Abs(Math.Sin(rtick+=rstep) * 254);
            double g = Math.Abs(Math.Sin(gtick+=gstep) * 254);
            double b = Math.Abs(Math.Sin(btick+=bstep) * 254);
            return new SFML.Graphics.Color(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
        }
    }
}
