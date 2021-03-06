﻿using Plevian;
using Plevian.Buildings;
using Plevian.Debugging;
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

        public delegate void cancelEventHandler(VillageQueues.QueueItem queueItem);
        public event cancelEventHandler Cancelled;

        public BuildingTask()
        {
            InitializeComponent();

            this.DataContextChanged += BuildingTask_DataContextChanged;
        }

        void BuildingTask_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Object newValue = e.NewValue;
            setData(newValue as VillageQueues.QueueItem);
        }

        public void setData(VillageQueues.QueueItem data)
        {
            if (data == null)
                Logger.warn("QueueItem in QueueTask.xaml.cs is null!");
            StackPanel.DataContext = model;
            model.setData(data);
        }

        public VillageQueues.QueueItem getData()
        {
            return model.data;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Cancelled != null)
                Cancelled(model.data);
        }
    }

    public class BuildingTaskModel : INotifyPropertyChanged
    {
        public VillageQueues.QueueItem data;

        public GameTime RemainingTime
        {
            get
            {
                return data.End.diffrence(GameTime.now);
            }
        }

        public GameTime EndTime
        {
            get
            {
                return data.End;
            }
        }

        public String Name
        {
            get
            {
                return data.Name;
            }
        }

        public String Extra
        {
            get
            {
                return data.Extra;
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

        public void setData(VillageQueues.QueueItem data)
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
