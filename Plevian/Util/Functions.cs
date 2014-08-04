using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Util
{
    public class Functions
    {
        public static bool Collision(int x1, int x2, int y1, int y2, int w1, int w2, int h1, int h2)
        {
            /*
             * x,y           w
             *   ----------------------
             *   -
             *  h-
             *   -
             *   ----------------------*/
            if (x1 + w1 >= x2 && 
                x1 <= x2 + w2 && 
                y1 + h1 >= y2 && 
                y1 <= y2 + h2)
                return true;


            return false;
        }

    }
}
