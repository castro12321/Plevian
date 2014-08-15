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

        private void setUnitCount(Label label, UnitType type)
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

        private void setBuildingLevel(Label label, BuildingType type)
        {
            if (village.isBuilt(type))
                LevelBarracks.Content = village.getBuilding(type).level;
            else
                label.Content = "0";
        }

        private void setBuildingProgress(Label label, BuildingType type)
        {
            foreach(BuildingQueueItem queueItem in village.buildingsQueue)
            {
                if (queueItem.toBuild == type)
                {
                    /**/
                    Seconds left = GameTime.now.diffrence(queueItem.end);
                    int minutes = left.seconds / 60;
                    int seconds = left.seconds % 60;
                    label.Content = minutes + ":" + seconds;
                    /*/
                    Seconds current = GameTime.now.diffrence(queueItem.start);
                    Seconds end     = queueItem.start.diffrence(queueItem.end);

                    double currentD = Convert.ToDouble(current.seconds);
                    double endD = Convert.ToDouble(end.seconds);
                    double progress = Math.Round((currentD / endD) * 100d, 2);
                    label.Content = progress + "%";
                    /**/
                    return;
                }
            }
            label.Content = "";
        }

        public void render()
        {
            ResourcesFood.Content  = village.resources.food;
            ResourcesWood.Content  = village.resources.wood;
            ResourcesIron.Content  = village.resources.iron;
            ResourcesStone.Content = village.resources.stone;

            setUnitCount(ResourcesWarriors, UnitType.WARRIOR);
            setUnitCount(ResourcesArchers, UnitType.ARCHER);
            setUnitCount(ResourcesKnights, UnitType.KNIGHT);

            setBuildingLevel(LevelBarracks, BuildingType.BARRACKS);
            setBuildingLevel(LevelStable  , BuildingType.STABLE);
            setBuildingLevel(LevelTownHall, BuildingType.TOWN_HALL);

            setBuildingProgress(UpgradeBarracksProgress, BuildingType.BARRACKS);
            setBuildingProgress(UpgradeStableProgress, BuildingType.STABLE);
            setBuildingProgress(UpgradeTownHallProgress, BuildingType.TOWN_HALL);

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
