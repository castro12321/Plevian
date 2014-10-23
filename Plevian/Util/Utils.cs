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
            rstep = random.NextDouble() / 800 + 0.003,
            gstep = random.NextDouble() / 800 + 0.003,
            bstep = random.NextDouble() / 800 + 0.003;
        /// <summary>Generates smooth color transition</summary>
        /// <returns>Generated color</returns>
        public static SFML.Graphics.Color smoothSFMLColor()
        {
            //return new SFML.Graphics.Color(235, 235, 235, 235);
            double r = Math.Abs(Math.Sin(rtick+=rstep) * 254);
            double g = Math.Abs(Math.Sin(gtick+=gstep) * 254);
            double b = Math.Abs(Math.Sin(btick+=bstep) * 254);
            return new SFML.Graphics.Color(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
        }
    }
}
