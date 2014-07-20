using Plevian.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Plevian.Maps
{
    /// <summary>
    /// Interaction logic for MapTab.xaml
    /// </summary>
    public partial class MapTab : UserControl
    {
        private MapView mapView;

        public MapTab(Map map)
        {
            InitializeComponent();

            mapView = new MapView(map);
            sfml_map.Child = mapView;

            mapView.PlevianMouseMovedEvent += mapView_PlevianMouseMovedEvent;
            mapView.PlevianTileClickedEvent += mapView_PlevianTileClickedEvent;
        }
        
        public void handleEvents()
        {
            mapView.handleEvents();
        }

        public void render()
        {
            mapView.render();
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
