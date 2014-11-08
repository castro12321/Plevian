using Plevian.Debugging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Plevian
{
    /// <summary>
    /// Represents virtual seconds since game start
    /// </summary>
    public class GameTime
    {
        public static int speed = 1;
        public static ulong uspeed = Convert.ToUInt32(speed);
        private static ulong lastSystemTime;
        private static GameTime gameTime;

        public int time;

        public static GameTime now
        {
            get
            {
                return gameTime.time;
            }
        }

        public GameTime(int time)
        {
            this.time = time;
        }
        
        // Conversions from-to GameTime
        public static implicit operator int(GameTime time) { return time.time; }
        public static implicit operator GameTime(int time) { return new GameTime(time); }

        public static void init(GameTime time)
        {
            GameTime.gameTime = time;
            lastSystemTime = SystemTime.now;
        }

        public override string ToString()
        {
            return time.ToString();
        }

        public GameTime copy()
        {
            return time;
        }

        /// <summary>
        /// </summary>
        /// <returns>Diff in seconds</returns>
        public static ulong update()
        {
            ulong systemTime = SystemTime.now;
            ulong datediff = (systemTime - lastSystemTime) * uspeed;
            gameTime.time += (int)datediff;
            lastSystemTime = systemTime;
            NotifyPropertyChanged("now");
            return datediff;
        }

        public GameTime diffrence(GameTime other)
        {
            return Math.Abs(time - other.time);
        }

        // Operators
        public static GameTime operator + (GameTime lh, GameTime rh) { return lh.time + rh.time; }
        public static GameTime operator - (GameTime lh, GameTime rh) { if (lh < rh) throw new Exception("Subtracting bigger time from lesser time"); return lh.time - rh.time; }
        public static GameTime operator * (GameTime lh, GameTime rh) { return lh.time * rh; }
        public static GameTime operator * (GameTime lh, float rh)    { return (int)((float)lh.time * rh); }
        public static bool operator > (GameTime lh, GameTime rh) { return lh.time > rh.time; }
        public static bool operator < (GameTime lh, GameTime rh) { return lh.time < rh.time; }
        public static bool operator >=(GameTime lh, GameTime rh) { return lh.time >= rh.time; }
        public static bool operator <=(GameTime lh, GameTime rh) { return lh.time <= rh.time; }
        public static bool operator ==(GameTime lh, GameTime rh) { return Equals(lh, rh); }
        public static bool operator !=(GameTime lh, GameTime rh) { return !Equals(lh, rh); }

        public override int GetHashCode()
        {
            return time.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GameTime))
                return false;
            return time.Equals(((GameTime)obj).time);
        }

        public static event PropertyChangedEventHandler PropertyChanged;

        protected static void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(GameTime.now, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class SystemTime
    {
        private static DateTime epoch = new DateTime(1970, 1, 1);

        public static ulong now
        {
            get
            {
                DateTime currentTime = DateTime.UtcNow;
                return (ulong)((currentTime - epoch).TotalSeconds);
            }
        }
    }
    /*public class GameTimeToStringConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value != null)
                return value.ToString();
            return "0";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null; //NO CONVERSION
        }
    }*/

    public class UserTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0";
            GameTime time = value as GameTime;
            return new GameTime(time.time / GameTime.speed).ToString();
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null; //NO CONVERSION
        }
    }
}
