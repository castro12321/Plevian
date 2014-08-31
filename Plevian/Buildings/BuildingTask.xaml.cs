using Plevian;
using Plevian.Buildings;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Plevian.Buildings
{
    /// <summary>
    /// Interaction logic for BuildingTask.xaml
    /// </summary>
    public partial class BuildingTask : UserControl
    {
        BuildingTaskModel model = new BuildingTaskModel();

        public BuildingTask()
        {
            InitializeComponent();

            this.DataContextChanged += BuildingTask_DataContextChanged;
        }

        void BuildingTask_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Object newValue = e.NewValue;
            if (newValue is BuildingQueueItem)
            {
                setData(newValue as BuildingQueueItem);
            }
        }

        public void setData(BuildingQueueItem data)
        {
            StackPanel.DataContext = model;
            model.setData(data);
        }

        public BuildingQueueItem getData()
        {
            return model.data;
        }
    }

    public class BuildingTaskModel : INotifyPropertyChanged
    {
        public BuildingQueueItem data;

        public Seconds RemainingTime
        {
            get
            {
                return data.end.diffrence(GameTime.now);
            }
        }

        public GameTime EndTime
        {
            get
            {
                return data.end;
            }
        }

        public String BuildingName
        {
            get
            {
                return data.toBuild.getDisplayName();
            }
        }

        public int Level
        {
            get
            {
                return data.level;
            }
        }
        

        public BuildingTaskModel()
        {
            GameTime.PropertyChanged += GameTime_PropertyChanged;
        }

        void GameTime_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if(propertyName == "now")
            {
                NotifyPropertyChanged("RemainingTime");
            }
        }

        public void setData(BuildingQueueItem data)
        {
            this.data = data;
            allChanged();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void allChanged()
        {
            NotifyPropertyChanged("Level");
            NotifyPropertyChanged("EndTime");
            NotifyPropertyChanged("RemainingTime");
            NotifyPropertyChanged("BuildingName");
        }
    }
}
