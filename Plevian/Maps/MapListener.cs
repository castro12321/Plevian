using Plevian.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Maps
{
    public class MapListener
    {
        public MapListener(MapView mapView)
        {
            mapView.PlevianMouseMovedEvent += mapView_PlevianMouseMovedEvent;
            mapView.PlevianTileClickedEvent += mapView_PlevianTileClickedEvent;
        }

        void mapView_PlevianTileClickedEvent(object sender, TileClickedEventArgs e)
        {
            Logger.c("event click at " + e.clicked + " " + e.clickedTileLocation.x + " " + e.clickedTileLocation.y);
        }

        void mapView_PlevianMouseMovedEvent(object sender, MouseMovedEventArgs e)
        {
            Plevian.Debugging.Logger.c("event move to " + e.mouseLocation.x + " " + e.mouseLocation.y);
        }
    }
}
