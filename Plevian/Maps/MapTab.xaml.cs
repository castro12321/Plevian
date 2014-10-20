using Plevian.Battles;
using Plevian.Debugging;
using Plevian.GUI;
using Plevian.Villages;
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
        private Game game;
        private int currentX = 0, currentY = 0;
        private Tile clickedTile = null;

        public MapTab(Game game)
        {
            InitializeComponent();
            this.game = game;
        }
        
        public void handleEvents()
        {
            if(mapView == null)
            {
                mapView = new MapView(game.map, sfml_map);
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
            VillageName.Content = "";

            EnterVillageButton.IsEnabled = false;
            SendUnitsButton.IsEnabled = false;
            SendResourcesButton.IsEnabled = false;
            
            if(tile.type == TerrainType.PLAINS || tile.type == TerrainType.VILLAGE)
            {
                SendUnitsButton.IsEnabled = true;
                SendUnitsButton.DataContext = tile;
            }

            if(tile.type == TerrainType.VILLAGE)
            {
                SendResourcesButton.IsEnabled = true;

                Village village = tile as Village;
                owner.Content = village.Owner.name;
                VillageName.Content = village.name;
                VillageNameStackPanel.Visibility = System.Windows.Visibility.Visible;
                if (village.Owner == Game.player || true) // Remove "|| true"
                {
                    EnterVillageButton.IsEnabled = true;
                    EnterVillageButton.DataContext = tile;
                }
            }
            else
            {
                owner.Content = "Nature";
                VillageNameStackPanel.Visibility = System.Windows.Visibility.Collapsed;
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
                Button button = sender as Button;
                Village entered = button.DataContext as Village;
                MainWindow.SwitchToVillage(entered);
            }
        }

        private void OnSendUnitsClick(object sender, RoutedEventArgs e)
        {
            SendUnitsWindow window = new SendUnitsWindow(Game.player, clickedTile);
            WindowMgr.Show(window);
        }

        private void OnSendResourcesClick(object sender, RoutedEventArgs e)
        {
            TradeWindow window = new TradeWindow(clickedTile as Village);
            WindowMgr.Show(window);
        }
    }
}
