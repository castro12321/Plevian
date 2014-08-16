using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Villages;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Plevian.GUI;

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

        private static MainWindow instance;

        public MainWindow()
        {
            // Initialize game
            game = new Game();

            // Initialize GUI
            InitializeComponent();

            mapTabItem.Content = (mapTab = new MapTab(game.map));
            villageTabItem.Content = (villageTab = new VillageTab(game));
            villageTab.Village = game.player.Capital;
           
            // Listen to some events
            Closed += new EventHandler(OnClose);
            PreviewKeyDown += KeyDownHandler;

            instance = this;
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

        public static MainWindow getInstance()
        {
            return instance;
        }

        public static void SwitchToVillage(Village to)
        {
            getInstance().villageTab.Village = to;
            changeTab(TabType.VILLAGE);
        }

        public static void changeTab(TabType type)
        {
            TabControl tabs = getInstance().MainWindowTabs;
            switch(type)
            {
                case TabType.MAP :
                    {
                        tabs.SelectedItem = getInstance().mapTabItem;
                        break;
                    }
                case TabType.VILLAGE :
                    {
                        tabs.SelectedItem = getInstance().villageTabItem;
                        break;
                    }
            }
        }
    }
}
