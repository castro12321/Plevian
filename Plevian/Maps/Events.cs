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
        public TerrainType clicked { get; private set; }

        public TileClickedEventArgs(Location clickedTileLocation, TerrainType clicked)
        {
            this.clickedTileLocation = clickedTileLocation;
            this.clicked = clicked;
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
