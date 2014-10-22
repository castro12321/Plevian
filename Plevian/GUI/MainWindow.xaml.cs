using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Villages;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Plevian.GUI;
using Plevian.Messages;
using System.Collections.Generic;
using Plevian.Players;

// TODO: Plevian main TODO board
// Known issues:
// - Skok w przyszlosc; Przykład:
//      - W game ticku, gracz A robi tick swoich wiosek; Wywolano tick dla wioski A
//      - Nastepnie podczas ticku dla gracza B wywolano tick wioski B, ktora zajela wioske A
//      - W tym momencie, do listy wiosek gracza B, dodano wioske A, przez co pod koniec petli tick dla wioski A wywola sie drugi raz
//      - Ergo wioska A zrobila tick w przyszlosc!

namespace Plevian
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private bool running = true;
        private static Random random = new Random();
        private Game game;

        private MapTab mapTab;
        public VillageTab villageTab;
        private MessagesTab messagesTab;
        private SettingsTab settingsTab;

        private static MainWindow instance;

        public MainWindow(List<Player> players, Map map)
        {
            instance = this;
            // Initialize GUI
            InitializeComponent();

            // Initialize game
            if (players != null && map != null)
                game = new Game(players, map);
            else
                game = new Game();

            // Initialize map tab
            mapTabItem.Content = (mapTab = new MapTab(game));

            // Initialize village tab
            villageTabItem.Content = (villageTab = new VillageTab(game));
            villageTab.Village = Game.player.Capital;
           
            // Initialize messages tab
            messagesTabItem.Content = (messagesTab = new MessagesTab(Game.player));

            // Initialize settings tab
            settingsTabItem.Content = (settingsTab = new SettingsTab());

            // Listen to some events
            Closed += new EventHandler(OnClose);
            PreviewKeyDown += KeyDownHandler;

            //get keyboard focus :F
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(MainWindow.getInstance());

            statusText.DataContext = villageTab.Village;
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

        private void OnClose(object sender, EventArgs e)
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
            changeTab(TabType.Village);
        }

        public static void changeTab(TabType type)
        {
            TabControl tabs = getInstance().MainWindowTabs;
            switch(type)
            {
                case TabType.Map:
                    {
                        tabs.SelectedItem = getInstance().mapTabItem;
                        break;
                    }
                case TabType.Village:
                    {
                        tabs.SelectedItem = getInstance().villageTabItem;
                        break;
                    }
                case TabType.Message:
                    {
                        tabs.SelectedItem = getInstance().messagesTabItem;
                        break;
                    }
            }
        }
    }
}
