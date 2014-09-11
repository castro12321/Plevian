using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Orders;
using Plevian.Players;
using Plevian.Resource;
using Plevian.Units;
using Plevian.Villages;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Plevian.GUI
{
    /// <summary>
    /// Interaction logic for AttackWindow.xaml
    /// </summary>
    public partial class TradeWindow : Window
    {
        public readonly Village target;

        public TradeWindow(Village target)
        {
            if (target == null)
                throw new ArgumentException("Target trade village cannot be null");
            this.target = target;

            InitializeComponent();

            villagePicker.SetPlayer(Game.player);
            villagePicker.SelectedVillage += villagePicker_SelectedVillage;
            SelectedVillage(villagePicker.CurrentlySelectedVillage);
        }

        void villagePicker_SelectedVillage(object sender, SelectedVillageEvent e)
        {
            SelectedVillage(e.SelectedVillage);
        }

        private void SelectedVillage(Village selected)
        {
            maxFood.DataContext = selected.resources;
            maxWood.DataContext = selected.resources;
            maxIron.DataContext = selected.resources;
            maxStone.DataContext = selected.resources;
        }

        private void maxFood_Click(object sender, RoutedEventArgs e)
        {
            foodToSend.Text = villagePicker.CurrentlySelectedVillage.resources.food.ToString();
        }

        private void maxWood_Click(object sender, RoutedEventArgs e)
        {
            woodToSend.Text = villagePicker.CurrentlySelectedVillage.resources.wood.ToString();
        }

        private void maxIron_Click(object sender, RoutedEventArgs e)
        {
            ironToSend.Text = villagePicker.CurrentlySelectedVillage.resources.iron.ToString();
        }

        private void maxStone_Click(object sender, RoutedEventArgs e)
        {
            stoneToSend.Text = villagePicker.CurrentlySelectedVillage.resources.stone.ToString();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            Resources toSend;

            try
            {
                int food = int.Parse(foodToSend.Text);
                int wood = int.Parse(woodToSend.Text);
                int iron = int.Parse(ironToSend.Text);
                int stone = int.Parse(stoneToSend.Text);
                toSend = new Resources(food, wood, iron, stone);
            }
            catch(Exception ex)
            {
                Logger.warn("Cannot send resources in TradeWindow. " + ex.Message + "\n" + ex.StackTrace);
                this.Close();
                return;
            }

            Village selectedVillage = villagePicker.CurrentlySelectedVillage;

            Army trader = new Army();
            trader.add(UnitFactory.createUnit(UnitType.TRADER, 1));

            Order order = new TradeOrder(selectedVillage, target, trader, toSend, null);
            selectedVillage.addOrder(order);

            this.Close();
        }
    }
}
