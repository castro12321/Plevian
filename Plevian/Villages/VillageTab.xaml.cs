using Plevian.Buildings;
using Plevian.Debugging;
using Plevian.Maps;
using Plevian.TechnologY;
using Plevian.Units;
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

namespace Plevian.Villages
{
    /// <summary>
    /// Interaction logic for VillageTab.xaml
    /// </summary>
    public partial class VillageTab : UserControl
    {
        private VillageView villageView;
        /// <summary>Currently active village</summary>
        private Village activeVillage;

        public Village Village
        {
            get
            {
                return activeVillage;
            }

            set
            {
                activeVillage = value;
                villageView.village = value;
                VillageName.Content = value.name;
                ResourcesControl.DataContext = value.resources;
                OrdersItemControl.ItemsSource = value.orders;
                BuildingsItemControl.ItemsSource = value.buildings;
                BuildingsQueueControl.ItemsSource = value.buildingsQueue;

                foreach (UnitControl control in UnitsRecruitStackPanel.Children)
                {
                    control.setVillage(value);
                    control.recruitEvent -= recruitEvent; // To remove old event handler (it's weird but works)
                    control.recruitEvent += recruitEvent; // To add current object handler
                }
            }
        }

        void recruitEvent(UnitType type, int quanity)
        {
           if(Village != null)
           {
               Village.recruit(UnitFactory.createUnit(type, quanity));
           }
        }

        public VillageTab(Game game)
        {
            InitializeComponent();

            villageView = new VillageView(game);
            sfml_village.Child = villageView;
        }

        public void handleEvents()
        {
            // Allow SFML to handle their events
            villageView.handleEvents();
        }

        public void render()
        {
            coords.Content = "X:" + Village.location.x + " Y:" + Village.location.y;
            
            // Render SFML
            villageView.render();
        }

        private void RecruitWarrior_Click(object sender, RoutedEventArgs e)
        {
            Village.recruit(new Warrior(1));
        }

        private void RecruitArcher_Click(object sender, RoutedEventArgs e)
        {
            Village.recruit(new Archer(1));
        }

        private void RecruitKnight_Click(object sender, RoutedEventArgs e)
        {
            Village.recruit(new Knight(1));
        }

        private void RecruitSettler_Click(object sender, RoutedEventArgs e)
        {
            Village.recruit(new Settler(1));
        }

        private void RecruitTrader_Click(object sender, RoutedEventArgs e)
        {
            Village.recruit(new Trader(1));
        }

        private void RecruitDuke_Click(object sender, RoutedEventArgs e)
        {
            Village.recruit(new Duke(1));
        }

        private void RecruitRam_Click(object sender, RoutedEventArgs e)
        {
            Village.recruit(new Ram(1));
        }

        private void ResearchLasers_Click(object sender, RoutedEventArgs e)
        {
            Village.research(new TechnologyLasers());
        }

        private void ResearchNukes_Click(object sender, RoutedEventArgs e)
        {
            Village.research(new TechnologyNukes());
        }

        private void onLabelClick(object sender, MouseButtonEventArgs e)
        {
            VillageNameTextbox.Text = VillageName.Content.ToString();
            VillageName.Visibility = System.Windows.Visibility.Collapsed;
            VillageNameTextbox.Visibility = System.Windows.Visibility.Visible;
        }


        private void VillageTextBoxHide()
        {
            VillageName.Content = VillageNameTextbox.Text;
            VillageName.Visibility = System.Windows.Visibility.Visible;
            VillageNameTextbox.Visibility = System.Windows.Visibility.Collapsed;
            Village.name = VillageNameTextbox.Text;
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if(sender == VillageNameTextbox)
            {
                if(e.Key == Key.Return)
                {
                    VillageTextBoxHide();
                }
            }
        }

        private void upgradeBuilding(Object sender, Building building)
        {
            Village.build(building.type);
        }

        private void onVillageNameFocusKeyboardLost(object sender, KeyboardFocusChangedEventArgs e)
        {
            VillageTextBoxHide();
        }

        private void onVillageNameFocusLost(object sender, RoutedEventArgs e)
        {
            VillageTextBoxHide();
        }

    }
}
