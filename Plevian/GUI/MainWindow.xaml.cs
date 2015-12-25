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
using Plevian.Save;
using Plevian.GUI.TechnologiesView;

namespace Plevian
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private bool running = true;
        private static Random random = new Random();

        private OverlayWindow overlay;

        public MapTab mapTab;
        public VillageTab villageTab;
        public TechnologiesTab technologiesTab;
        public MessagesTab messagesTab;
        public SettingsTab settingsTab;

        private static MainWindow instance;

        public MainWindow(SaveReader save)
        {
            instance = this;
            InitializeComponent();
            Show();
            overlay = new OverlayWindow(this);

            // Initialize game
            Game game = new Game(save);
            // Initialize tabs
            villageTab = new VillageTab();
            mapTab = new MapTab();
            technologiesTab = new TechnologiesTab();
            messagesTab = new MessagesTab();
            settingsTab = new SettingsTab();

            // Listen to some events
            Closed += (x, y) => running = false;
            //PreviewKeyDown += (x, y) => Logger.c("Pressed " + y.Key);

            // Get keyboard focus :F
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(MainWindow.getInstance());

            //StatusText.DataContext = villageTab.Village;
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
                technologiesTab.handleEvents();

                // Do the logic
                Game.game.tick();

                // Render
                mapTab.render();
                villageTab.render();
                technologiesTab.render();
            }
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
            switch(type)
            {
                case TabType.Map:
                    {
                        getInstance().overlay.Content = getInstance().mapTab;
                        break;
                    }
                case TabType.Village:
                    {
                        getInstance().overlay.Content = getInstance().villageTab;
                        break;
                    }
                case TabType.Message:
                    {
                        getInstance().overlay.Content = getInstance().messagesTab;
                        break;
                    }
            }
        }
    }
}
