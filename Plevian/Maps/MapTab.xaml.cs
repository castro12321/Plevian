using Plevian.Debugging;
using Plevian.GUI;
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
        private Map map;
        private int currentX = 0, currentY = 0;
        private Tile clickedTile = null;

        public MapTab(Map map)
        {
            InitializeComponent();
            this.map = map;
        }
        
        public void handleEvents()
        {
            if(mapView == null)
            {
                mapView = new MapView(map, sfml_map);
                sfml_map.Child = mapView;

                mapView.PlevianMouseMovedEvent += mapView_PlevianMouseMovedEvent;
                mapView.PlevianTileClickedEvent += mapView_PlevianTileClickedEvent;
                
            }

            mapView.handleEvents();
        }

        public void render()
        {
            mapView.render();
        }

        void mapView_PlevianTileClickedEvent(object sender, TileClickedEventArgs e)
        {
            Tile tile = clickedTile = e.clickedTile;
            currentX = tile.location.x;
            currentY = tile.location.y;
            Logger.s("event click at " + tile.type + " " + tile.location.x + " " + tile.location.y);
            if (tile.type == TerrainType.VILLAGE)
            {
                owner.Content = "Player";
                EnterVillageButton.IsEnabled = true;
            }
            else
            {
                owner.Content = "Nature";
                EnterVillageButton.IsEnabled = false;
            }

            type.Content = Enum.GetName(typeof(TerrainType), tile.type);
        }

        void mapView_PlevianMouseMovedEvent(object sender, MouseMovedEventArgs e)
        {
            Plevian.Debugging.Logger.c("event move to " + e.mouseLocation.x + " " + e.mouseLocation.y);
            coords.Content = "X:" + e.mouseLocation.x + " Y:" + e.mouseLocation.y;
        }

        private void enterVillageClick(object sender, RoutedEventArgs e)
        {
           
            if(EnterVillageButton.IsEnabled)
            {
                MainWindow.changeTab(TabType.VILLAGE, new EnterVillageArgs(null));
            }
        }
    }
}
