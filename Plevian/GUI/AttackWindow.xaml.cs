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
using System.Windows.Threading;

namespace Plevian.GUI
{
    /// <summary>
    /// Interaction logic for AttackWindow.xaml
    /// </summary>
    public partial class AttackWindow : Window
    {
        VillageRecord selectedRecord = null;
        Village selectedVillage = null;
        Player player;
        DispatcherTimer timer = new DispatcherTimer();

        public AttackWindow(Player player)
        {
            this.player = player;

            InitializeComponent();
            fillAttackWindow();
            initTimer();
          
        }

        private void fillAttackWindow()
        {
            fillVillages(player);
        }

        private void initTimer()
        {
            timer.Tick += new EventHandler(tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void tick(object sender, EventArgs e)
        {
            if(selectedRecord != null)
                updateUnits(selectedRecord.village);
        }

        private void fillVillages(Player player)
        {
            VillagePanel.Children.Clear();

            foreach(Village village in player.Villages)
            {
                VillageRecord record = new VillageRecord(village);
                VillagePanel.Children.Add(record);

                record.onRecordClick += new VillageRecord.RecordClickedHandler(onRecordClick);
            }

            if(selectedRecord != null)
                selectedRecord.select();
        }

        private void onRecordClick(VillageRecord record)
        {
            if (selectedRecord != null)
                selectedRecord.unSelect();
            selectedRecord = record;
            selectedRecord.select();

            selectedVillage = selectedRecord.village;
            fillUnits(selectedVillage);
            updateUnits(selectedVillage);
        }

        private void fillUnits(Village village)
        {
            UnitPanel.Children.Clear();

            foreach (UnitType unitType in (UnitType[])Enum.GetValues(typeof(UnitType)))
            {
                UnitSelector selector = new UnitSelector(Enum.GetName(typeof(UnitType), unitType), 0, unitType);
                UnitPanel.Children.Add(selector);
            }
        }

        private void updateUnits(Village village)
        {
            Army army = village.army;
            foreach (var pair in army.getUnits())
            {
                for(int i = 0;i < UnitPanel.Children.Count;++i)
                {
                    UnitSelector selector = UnitPanel.Children[i] as UnitSelector;
                    if(selector.type == pair.Key)
                    {
                        selector.updateMaxQuanity(pair.Value.quanity);
                        break;
                    }
                }
            }
        }

        private void onWindowClose(object sender, EventArgs e)
        {
            MainWindow.getInstance().IsEnabled = true;
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(MainWindow.getInstance());
            timer.Stop();
        }
    }
}
