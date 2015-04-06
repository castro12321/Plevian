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
using Plevian.TechnologY;

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

            tooltipWindow = new Window();
            tooltipWindow.WindowStyle = WindowStyle.None;
            tooltipWindow.Hide();
            tooltipWindow.MouseEnter += (x, y) => tooltipWindow.Hide();
        }
        
        public void handleEvents()
        {
            if(mapView == null)
            {
                mapView = new MapView(new TechnologiesMapLoader().map, sfml_map);
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
                TechnologyTile techTile = tile as TechnologyTile;
                if(techTile == null)
                    throw new ArgumentException("Tile is not technology tile");

                Village village = MainWindow.getInstance().villageTab.Village;

                if (village.canResearch(techTile.technology))
                    village.research(techTile.technology);
            }
        }

        private class Wpf32Window : System.Windows.Forms.IWin32Window
        {
            public IntPtr Handle { get; private set; }

            public Wpf32Window(Window wpfWindow)
            {
                Handle = new System.Windows.Interop.WindowInteropHelper(wpfWindow).Handle;
            }
        }

        private Window tooltipWindow;
        private TechnologyTile hoveredTechnologyTile;
        void mapView_PlevianMouseMovedEvent(object sender, MouseMovedEventArgs e)
        {
            Plevian.Debugging.Logger.c("event move to " + e.mouseLocation.x + " " + e.mouseLocation.y);
            
            Tile tile = e.hoveredTile;
            TechnologyTile tech = tile as TechnologyTile;
            if (tech == null)
            {
                if (hoveredTechnologyTile != null)
                    hoveredTechnologyTile.hovered = false;
                hoveredTechnologyTile = null;
                tooltipWindow.Hide();
            }
            else
            {
                tech.hovered = true;
                hoveredTechnologyTile = tech;

                TextBox winContent = new TextBox();
                winContent.Text = tech.technology.Name;
                tooltipWindow.Content = winContent;
                tooltipWindow.Show();
                tooltipWindow.SizeToContent = SizeToContent.WidthAndHeight;
                System.Drawing.Point mousePos = System.Windows.Forms.Cursor.Position;
                tooltipWindow.Left = mousePos.X - tooltipWindow.Width - 15;
                tooltipWindow.Top = mousePos.Y - tooltipWindow.Height - 15;
            }
            
            //coords.Content = "X:" + e.mouseLocation.x + " Y:" + e.mouseLocation.y;
            // TODO: Modify tile, add animation for tile or sth to highlight selected tile
        }
    }
}
