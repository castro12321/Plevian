using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Plevian.Messages
{
    public class DataGridElement : DockPanel
    {
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                if (Children.Count > 0)
                    Children.RemoveAt(0);
                if (e.NewValue != null)
                    Children.Add(e.NewValue as UIElement);
            }
        }
    }
}
