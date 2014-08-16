using Plevian.Players;
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

namespace Plevian.GUI
{
    /// <summary>
    /// Interaction logic for AttackWindow.xaml
    /// </summary>
    public partial class AttackWindow : Window
    {
        VillageRecord selectedRecord = null;
        Player player;
        public AttackWindow(Player player)
        {
            this.player = player;

            InitializeComponent();
            fillAttackWindow();
        }

        private void fillAttackWindow()
        {
            fillVillages(player);
            
        }

        private void fillVillages(Player player)
        {
            foreach(Village village in player.Villages)
            {
                VillageRecord record = new VillageRecord(village);
                VillagePanel.Children.Add(record);

                record.onRecordClick += new VillageRecord.RecordClickedHandler(onRecordClick);
            }
        }

        private void onRecordClick(VillageRecord record)
        {
            if (selectedRecord != null)
                selectedRecord.unSelect();
            selectedRecord = record;
            selectedRecord.select();

            Village village = selectedRecord.village;
            fillUnits(village);
        }

        private void fillUnits(Village village)
        {
            UnitPanel.Children.Clear();
            foreach (var pair in village.army.getUnits())
            {
                UnitType unitType = pair.Key;
                UnitSelector selector = new UnitSelector(Enum.GetName(typeof(UnitType), unitType), 100, unitType);
                UnitPanel.Children.Add(selector);
            }
        }

        private void onWindowClose(object sender, EventArgs e)
        {
            MainWindow.getInstance().IsEnabled = true;
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(MainWindow.getInstance());
        }
    }
}
