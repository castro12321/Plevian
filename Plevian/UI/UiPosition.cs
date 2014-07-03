using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.UI
{
    class UiPosition
    {
        int x, y;

        public UiPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }


        public static UiPosition operator +(UiPosition rh, UiPosition lh)
        {
            return new UiPosition(rh.x + rh.x,
                                  lh.y + rh.y);
        }
    }
}
