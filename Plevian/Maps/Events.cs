using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Maps
{
    public class TileClickedEventArgs
    {
        public Location clickedTileLocation { get; private set; }
        public Tile clickedTile { get; private set; }

        public TileClickedEventArgs(Location clickedTileLocation, Tile clicked)
        {
            this.clickedTileLocation = clickedTileLocation;
            this.clickedTile = clicked;
        }
    }

    public class MouseMovedEventArgs
    {
        public Location mouseLocation { get; private set; }

        public MouseMovedEventArgs(Location mouseLocation)
        {
            this.mouseLocation = mouseLocation;
        }
    }
}
