using Plevian.Players;
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
using System.Windows.Shapes;

namespace Plevian.GUI
{
    /// <summary>
    /// Interaction logic for AttackWindow.xaml
    /// </summary>
    public partial class AttackWindow : Window
    {
        VillageRecord selectedRecord = null;
        public AttackWindow()
        {
            InitializeComponent();
            fillAttackWindow();
        }

        private void fillAttackWindow()
        {
            Player player;
            fillVillages();
            fillUnits();
            
        }

        private void fillVillages()
        {
            for (int i = 0; i < 50; ++i)
            {
                VillageRecord record = new VillageRecord(i.ToString(), null);
                record.onRecordClick += new VillageRecord.RecordClickedHandler(onRecordClick);

                VillagePanel.Children.Add(record);

            }
        }

        private void onRecordClick(VillageRecord record)
        {
            if (selectedRecord != null)
                selectedRecord.unSelect();
            selectedRecord = record;
            selectedRecord.select();
        }

        private void fillUnits()
        {
            foreach (UnitType unitType in (UnitType[])Enum.GetValues(typeof(UnitType)))
            {
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
