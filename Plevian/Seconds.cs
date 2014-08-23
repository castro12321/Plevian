using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Plevian
{
    public class SecondsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            Seconds second = value as Seconds;
            int seconds = second.seconds;
            int hours = seconds / 60 / 60;
            int minutes = seconds / 60;
            return hours.ToString() + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        public object ConvertBack(object value, Type targetType,
         object parameter, CultureInfo culture)
        {
            return null; //NO CONVERSION
        }
    }


    public class Seconds : GameTime
    {
        public int seconds { get { return time; } set { time = value; } }

        public Seconds(int seconds)
            : base(seconds)
        {
        }

        public override string ToString()
        {
            return seconds.ToString();
        }

        public override GameTime copy()
        {
            return new Seconds(this.seconds);
        }

    }
}
