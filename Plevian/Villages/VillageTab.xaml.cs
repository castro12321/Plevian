using Plevian.Buildings;
using Plevian.Debugging;
using Plevian.Maps;
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

        private void setUnitCount(Label label, UnitType type)
        {
            try
            {
                label.Content = Village.army.get(type).quanity;
            }
            catch(KeyNotFoundException)
            {
                label.Content = "0";
            }
        }

        private void setBuildingLevel(Label label, BuildingType type)
        {
            if (Village.isBuilt(type))
                label.Content = Village.getBuilding(type).level;
            else
                label.Content = "0";
        }

        private void setBuildingProgress(Label label, BuildingType type)
        {
            foreach (BuildingQueueItem queueItem in Village.buildingsQueue)
            {
                if (queueItem.toBuild.type == type)
                {
                    Seconds left = GameTime.now.diffrence(queueItem.end);
                    int minutes = left.seconds / 60;
                    int seconds = left.seconds % 60;
                    label.Content = minutes + ":" + seconds;
                    return;
                }
            }
            label.Content = "";
        }

        private void setRecruitProgress(Label label, UnitType type)
        {
            foreach (RecruitQueueItem queueItem in Village.recruitQueue)
            {
                if (queueItem.toRecruit.getUnitType() == type)
                {
                    Seconds left = GameTime.now.diffrence(queueItem.end);
                    int minutes = left.seconds / 60;
                    int seconds = left.seconds % 60;
                    label.Content = minutes + ":" + seconds;
                    return;
                }
            }
            label.Content = "";
        }

        public void render()
        {
            coords.Content = "X:" + Village.location.x + " Y:" + Village.location.y;

           // ResourcesFood.Content  = Village.resources.food;
           // ResourcesWood.Content  = Village.resources.wood;
           // ResourcesIron.Content  = Village.resources.iron;
           // ResourcesStone.Content = Village.resources.stone;

            setUnitCount(ResourcesWarriors, UnitType.WARRIOR);
            setUnitCount(ResourcesArchers, UnitType.ARCHER);
            setUnitCount(ResourcesKnights, UnitType.KNIGHT);
            setUnitCount(ResourcesSettlers, UnitType.SETTLER);
            setUnitCount(ResourcesTraders, UnitType.TRADER);

            setRecruitProgress(RecruitWarriorProgress, UnitType.WARRIOR);
            setRecruitProgress(RecruitArcherProgress, UnitType.ARCHER);
            setRecruitProgress(RecruitKnightProgress, UnitType.KNIGHT);
            setRecruitProgress(RecruitSettlerProgress, UnitType.SETTLER);
            setRecruitProgress(RecruitTraderProgress, UnitType.TRADER);

            /*setBuildingLevel(LevelBarracks, BuildingType.BARRACKS);
            setBuildingLevel(LevelStable  , BuildingType.STABLE);
            setBuildingLevel(LevelTownHall, BuildingType.TOWN_HALL);
            setBuildingLevel(LevelFarm    , BuildingType.FARM);
            setBuildingLevel(LevelLumberMill, BuildingType.LUMBER_MILL);
            setBuildingLevel(LevelMine    , BuildingType.MINE);

            setBuildingProgress(UpgradeBarracksProgress, BuildingType.BARRACKS);
            setBuildingProgress(UpgradeStableProgress, BuildingType.STABLE);
            setBuildingProgress(UpgradeTownHallProgress, BuildingType.TOWN_HALL);
            setBuildingProgress(UpgradeFarmProgress, BuildingType.FARM);
            setBuildingProgress(UpgradeLumberMillProgress, BuildingType.LUMBER_MILL);
            setBuildingProgress(UpgradeMineProgress, BuildingType.MINE);*/

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

        private void ResearchLasers_Click(object sender, RoutedEventArgs e)
        {
            // TODO: start researching lasers
        }

        private void ResearchNukes_Click(object sender, RoutedEventArgs e)
        {
            // TODO: start researching nukes
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
