using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Plevian.Debugging;
using SFML.Graphics;
using SFML.Window;
using Plevian.Utils;
using System.Windows.Forms.Integration;
using Plevian.Maps;
using Plevian.Villages;

namespace Plevian
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private static bool running = true;
        private static Random random = new Random();
        private Game game;

        private MapTab mapTab;
        private VillageTab villageTab;

        public MainWindow()
        {
            // Initialize game
            game = new Game();

            // Initialize GUI
            InitializeComponent();

            mapTabItem.Content = (mapTab = new MapTab(game.map));
            villageTabItem.Content = (villageTab = new VillageTab(game));
           
            // Listen to some events
            Closed += new EventHandler(OnClose);
            PreviewKeyDown += KeyDownHandler;
        }

        void KeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Logger.c("Pressed12");
            Logger.c("Pressed " + e.Key);
        }

        byte oldR = 128;
        byte oldG = 128;
        byte oldB = 128;
        private Color randomColor()
        {
            oldR = (byte)random.Next(oldR - 2, oldR + 3);
            oldG = (byte)random.Next(oldG - 2, oldG + 3);
            oldB = (byte)random.Next(oldB - 2, oldB + 3);
            return new Color(oldR, oldG, oldB);
        }

        
        public void run()
        {
            while (running)
            {
                System.Threading.Thread.Sleep(10); // Some fake lag (if needed)
                System.Windows.Forms.Application.DoEvents(); // handle form events

                // Process events
                mapTab.handleEvents();
                villageTab.handleEvents();

                // Do the logic
                game.tick();

                // Render
                mapTab.render();
                villageTab.render();
            }
        }

        static void OnClose(object sender, EventArgs e)
        {
            running = false;
        }
    }
}
