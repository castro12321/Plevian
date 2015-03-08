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
using Plevian.Util;

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
                TechnologyItemsControl.ItemsSource = value.Owner.technologies.technologies;
                QueueControl.ItemsSource = value.queues.queue;

                foreach (UnitControl control in UnitsRecruitStackPanel.Children)
                {
                    control.setVillage(value);
                    control.recruitEvent -= recruitEvent; // To remove old event handler (it's weird but works)
                    control.recruitEvent += recruitEvent; // To add current object handler
                }
            }
        }

        void recruitEvent(UnitType type, int quantity)
        {
           if(Village != null)
               Village.recruit(UnitFactory.createUnit(type, quantity));
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
            time.Content = Utils.secondsToHumanTime(GameTime.now);

            // Render SFML
            villageView.render();
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

        private void discoverTechnology(Object sender, Technology technology)
        {
            Village.research(technology);
        }

        private void onVillageNameFocusKeyboardLost(object sender, KeyboardFocusChangedEventArgs e)
        {
            VillageTextBoxHide();
        }

        private void onVillageNameFocusLost(object sender, RoutedEventArgs e)
        {
            VillageTextBoxHide();
        }

        private void BuildingTask_Cancelled(Queues.QueueItem queueItem)
        {
            activeVillage.queues.Remove(queueItem);
            /* Don't allow refunds? >:D
            activeVillage.resources.Add(queueItem.price);
            if(queueItem is BuildingQueueItem)
            {
                BuildingQueueItem item = queueItem as BuildingQueueItem;
                activeVillage.resources.Add(item.price)
            } */
        }

    }
}
