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

        public static SFML.Graphics.Image ToSFMLImage(System.Drawing.Bitmap bmp)
        {
            SFML.Graphics.Color[,] sfmlcolorarray = new SFML.Graphics.Color[bmp.Height, bmp.Width];
            SFML.Graphics.Image newimage = null;
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    System.Drawing.Color csharpcolor = bmp.GetPixel(x, y);
                    sfmlcolorarray[y, x] = new SFML.Graphics.Color(csharpcolor.R, csharpcolor.G, csharpcolor.B, csharpcolor.A);
                }
            }
            newimage = new SFML.Graphics.Image(sfmlcolorarray);
            return newimage;
        }

        public static SFML.Graphics.Texture ToSFMLTexture(System.Drawing.Bitmap bmp)
        {
            return new SFML.Graphics.Texture(ToSFMLImage(bmp));
        }

        public static String secondsToHumanTime(int seconds)
        {
            int hours = seconds / 3600;
            seconds %= 3600;
            int minutes = seconds / 60;
            seconds %= 60;

            String sHours = hours.ToString();
            if (hours < 10)
                sHours = "0" + sHours;
            String sMinutes = minutes.ToString();
            if (minutes < 10)
                sMinutes = "0" + sMinutes;
            String sSeconds = seconds.ToString();
            if (seconds < 10)
                sSeconds = "0" + sSeconds;

            return sHours + ":" + sMinutes + ":" + sSeconds;
        }
    }
}
