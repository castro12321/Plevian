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
    /// <summary>
    /// Interaction logic for VillageRecord.xaml
    /// </summary>
    public partial class VillageRecord : UserControl
    {
        public delegate void RecordClickedHandler(VillageRecord record);
        public RecordClickedHandler onRecordClick;
        public Village village;

        public VillageRecord(Village village)
        {
            InitializeComponent();
            this.VillageName.Content = village.name;
            this.village = village;
        }

        private void onMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                if(onRecordClick != null)
                    onRecordClick(this);
            }
        }

        public void select()
        {
            VillageName.FontWeight = FontWeights.Bold;
            StackPanel.Background = Brushes.Blue;
        }

        public void unSelect()
        {
            VillageName.FontWeight = FontWeights.Normal;
            StackPanel.Background = Brushes.Transparent;
        }
    }
}
