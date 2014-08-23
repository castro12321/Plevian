using Plevian.Debugging;
using Plevian.Players;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Plevian.GUI
{
    public class SelectedVillageEvent
    {
        public Village SelectedVillage { get; private set; }

        public SelectedVillageEvent(Village clicked)
        {
            this.SelectedVillage = clicked;
        }
    }

    /// <summary>
    /// Interaction logic for VillagePicker.xaml
    /// </summary>  
    public partial class VillagePicker : UserControl
    {
        private Player source;
        public Village CurrentlySelectedVillage;
        public event EventHandler<SelectedVillageEvent> SelectedVillage;

        public VillagePicker()
        {
            InitializeComponent();
        }

        /// <summary></summary>
        /// <param name="player">Player source to use to populate the villages list</param>
        public void SetPlayer(Player player)
        {
            this.source = player;

            //villages.ItemsSource = null;
            villages.ItemsSource = player.villages;
            //villages.Items.Refresh();
        }

        private void villages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentlySelectedVillage = e.AddedItems[0] as Village;
            if(e.AddedItems.Count > 0)
                SelectedVillage(sender, new SelectedVillageEvent(CurrentlySelectedVillage));
        }
    }
}
