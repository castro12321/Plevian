using Plevian.Maps;
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
        public Village village { get; set; }

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

        private void setUnitCount(Label label, Units.UnitType type)
        {
            try
            {
                label.Content = village.army.get(type).quanity;
            }
            catch(KeyNotFoundException ex)
            {
                label.Content = "0";
            }
        }

        private void setBuildingLevel(Label label, Buildings.BuildingType type)
        {
            if (village.isBuilt(type))
                LevelBarracks.Content = village.getBuilding(type).level;
            else
                label.Content = "0";
        }

        public void render()
        {
            // Update GUI
            ResourcesFood.Content  = village.resources.food;
            ResourcesWood.Content  = village.resources.wood;
            ResourcesIron.Content  = village.resources.iron;
            ResourcesStone.Content = village.resources.stone;

            setUnitCount(ResourcesWarriors, Units.UnitType.WARRIOR);
            setUnitCount(ResourcesArchers, Units.UnitType.ARCHER);
            setUnitCount(ResourcesKnights, Units.UnitType.KNIGHT);
            //ResourcesWarriors.Content = village.army.get(Units.UnitType.WARRIOR).quanity;
            //ResourcesArchers.Content  = village.army.get(Units.UnitType.ARCHER).quanity;
            //ResourcesKnights.Content  = village.army.get(Units.UnitType.KNIGHT).quanity;

            setBuildingLevel(LevelBarracks, Buildings.BuildingType.BARRACKS);
            setBuildingLevel(LevelStable  , Buildings.BuildingType.STABLE);
            setBuildingLevel(LevelTownHall, Buildings.BuildingType.TOWN_HALL);
            //LevelFarm.Content     = village.getBuilding(Buildings.BuildingType.FARM).level;
            //LevelMine.Content     = village.getBuilding(Buildings.BuildingType.MINE).level;
            //LevelTownHall.Content = village.getBuilding(Buildings.BuildingType.TOWN_HALL).level;

            //ResourcesFood.Content = village.army.

            // Render SFML
            villageView.render();
        }

        private void RecruitWarrior_Click(object sender, RoutedEventArgs e)
        {
            village.recruit(new Units.Warrior(1));
        }

        private void RecruitArcher_Click(object sender, RoutedEventArgs e)
        {
            village.recruit(new Units.Archer(1));
        }

        private void RecruitKnight_Click(object sender, RoutedEventArgs e)
        {
            village.recruit(new Units.Knight(1));
        }


        private void UpgradeBarracks_Click(object sender, RoutedEventArgs e)
        {
            village.build(Buildings.BuildingType.BARRACKS);
        }

        private void UpgradeTownHall_Click(object sender, RoutedEventArgs e)
        {
            village.build(Buildings.BuildingType.TOWN_HALL);
        }

        private void UpgradeStable_Click(object sender, RoutedEventArgs e)
        {
            village.build(Buildings.BuildingType.STABLE);
        }
    }
}
