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
        public static int speed = 10;
        public static ulong uspeed = Convert.ToUInt32(speed);
        private static ulong lastSystemTime;
        private static GameTime gameTime;

        public int time;

        public static GameTime now
        {
            get
            {
                //return gameTime;
                return new GameTime(gameTime.time);
            }
        }

        protected GameTime(int time)
        {
            this.time = time;
        }

        public static void init(int time)
        {
            init(new GameTime(time));
        }

        public static void init(GameTime time)
        {
            GameTime.gameTime = time;
            lastSystemTime = SystemTime.now;
        }

        public override string ToString()
        {
            return time.ToString();
        }

        public virtual GameTime copy()
        {
            return new GameTime(time);
        }

        /// <summary>
        /// </summary>
        /// <returns>Diff in seconds</returns>
        public static ulong update()
        {
            ulong systemTime = SystemTime.now;
            ulong datediff = (systemTime - lastSystemTime);// *uspeed;
            gameTime.time += (int)datediff;
            lastSystemTime = systemTime;
            NotifyPropertyChanged("now");
            return datediff;
        }

        public Seconds diffrence(GameTime other)
        {
            return new Seconds(Math.Abs(time - other.time) * speed);
        }

        private static class SystemTime
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

        public static GameTime operator +(GameTime lh, GameTime rh)
        {
            return new GameTime(lh.time + rh.time);
        }

        public static GameTime operator -(GameTime lh, GameTime rh)
        {
            if (lh < rh) throw new Exception("Subtracting bigger time from lesser time");
            return new GameTime(lh.time - rh.time);
        }

        public static GameTime operator *(GameTime lh, int rh)
        {
            return new GameTime(lh.time * rh);
        }

        public static GameTime operator *(GameTime lh, float rh)
        {
            return new GameTime((int)((float)lh.time * rh));
        }

        public static bool operator >(GameTime lh, GameTime rh)
        {
            return lh.time > rh.time;
        }

        public static bool operator >=(GameTime lh, GameTime rh)
        {
            return lh.time >= rh.time;
        }

        public static bool operator <(GameTime lh, GameTime rh)
        {
            return lh.time < rh.time;
        }

        public static bool operator <=(GameTime lh, GameTime rh)
        {
            return lh.time <= rh.time;
        }

        public static bool operator ==(GameTime lh, GameTime rh)
        {
            if (object.Equals(lh, rh))
                return true;
            if (object.Equals(lh, null) || object.Equals(rh, null))
                return false;
            return lh.time.Equals(rh.time);
        }

        public static bool operator !=(GameTime lh, GameTime rh)
        {
            return lh.time != rh.time;
        }

        public override int GetHashCode()
        {
            return time.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is GameTime)
            {
                GameTime other = (GameTime)obj;
                return time.Equals(other.time);
            }
            return false;
        }

        public static event PropertyChangedEventHandler PropertyChanged;

        protected static void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(GameTime.now, new PropertyChangedEventArgs(propertyName));
        }
    }



    public class GameTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0";
            GameTime gameTime = value as GameTime;
            int time = gameTime.time;
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType,
         object parameter, CultureInfo culture)
        {
            return null; //NO CONVERSION
        }
    }
}
