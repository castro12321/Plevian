﻿using Plevian.Buildings;
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
                OrdersItemControl.ItemsSource = value.orders;
                MainWindow.getInstance().statusText.Text = value.name;
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
                if (queueItem.toBuild == type)
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
                if (queueItem.unit.getUnitType() == type)
                {
                    Seconds left = GameTime.now.diffrence(queueItem.endRecruiting);
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

            ResourcesFood.Content  = Village.resources.food;
            ResourcesWood.Content  = Village.resources.wood;
            ResourcesIron.Content  = Village.resources.iron;
            ResourcesStone.Content = Village.resources.stone;

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

            setBuildingLevel(LevelBarracks, BuildingType.BARRACKS);
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
            setBuildingProgress(UpgradeMineProgress, BuildingType.MINE);

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


        private void UpgradeBarracks_Click(object sender, RoutedEventArgs e)
        {
            Village.build(BuildingType.BARRACKS);
        }

        private void UpgradeTownHall_Click(object sender, RoutedEventArgs e)
        {
            Village.build(BuildingType.TOWN_HALL);
        }

        private void UpgradeStable_Click(object sender, RoutedEventArgs e)
        {
            Village.build(BuildingType.STABLE);
        }

        private void UpgradeFarm_Click(object sender, RoutedEventArgs e)
        {
            Village.build(BuildingType.FARM);
        }

        private void UpgradeLumberMill_Click(object sender, RoutedEventArgs e)
        {
            Village.build(BuildingType.LUMBER_MILL);
        }

        private void UpgradeMine_Click(object sender, RoutedEventArgs e)
        {
            Village.build(BuildingType.MINE);
        }
    }
}
