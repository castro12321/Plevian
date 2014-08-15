using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Villages;
using System;
using System.Windows.Controls;
using System.Windows.Media;

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
            villageTab.village = game.village; // TODO: Temporary test village; delete later
           
            // Listen to some events
            Closed += new EventHandler(OnClose);
            PreviewKeyDown += KeyDownHandler;
        }

        void KeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Logger.c("Pressed12");
            Logger.c("Pressed " + e.Key);
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
