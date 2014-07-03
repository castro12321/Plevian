using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.UI
{
    public class UiPosition
    {
        int x, y;

        public UiPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Creates copy of object
        /// </summary>
        /// <param name="other">Object to copy</param>
        public UiPosition(UiPosition other)
        {
            this.x = other.x;
            this.y = other.y;
        }


        public static UiPosition operator +(UiPosition rh, UiPosition lh)
        {
            return new UiPosition(rh.x + rh.x,
                                  lh.y + rh.y);
        }
    }
}
