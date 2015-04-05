using Plevian.Debugging;
using Plevian.Maps;
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

namespace Plevian.GUI.TechnologiesView
{
    /// <summary>
    /// Interaction logic for TechnologiesTab.xaml
    /// </summary>
    public partial class TechnologiesTab : UserControl
    {
        private MapView mapView;
        private Game game;
        private int currentX = 0, currentY = 0;
        private Tile clickedTile = null;

        public TechnologiesTab()
        {
            InitializeComponent();
            this.game = Game.game;
        }
        
        public void handleEvents()
        {
            if(mapView == null)
            {
                mapView = new MapView(game.technologiesMap, sfml_map);
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
            
            if(tile.type == TerrainType.TEHNOLOGY)
            {
                //fire event Technology upgrade: tile.technology;
            }
        }

        void mapView_PlevianMouseMovedEvent(object sender, MouseMovedEventArgs e)
        {
            Plevian.Debugging.Logger.c("event move to " + e.mouseLocation.x + " " + e.mouseLocation.y);
            //coords.Content = "X:" + e.mouseLocation.x + " Y:" + e.mouseLocation.y;
            // TODO: Modify tile, add animation for tile or sth to highlight selected tile
        }
    }
}
