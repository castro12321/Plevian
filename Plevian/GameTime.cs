using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian
{
    /// <summary>
    /// Represents virtual seconds since game start
    /// </summary>
    public class GameTime
    {
        public static GameTime now { get; protected set; }
        private int time;
        
        protected GameTime(int time)
        {
            this.time = time;
        }

        public static void init(int time)
        {
            GameTime.now = new Seconds(time);
            update();
        }

        public override string ToString()
        {
            return time.ToString();
        }
        
        public static ulong update()
        {
            ulong datediff = SystemTime.update();
            GameTime.now.time += (int)datediff;
            return datediff;
        }
        
        private static class SystemTime
        {
            private static DateTime epoch = new DateTime(1970, 1, 1);
            private static ulong lastSystemTime = SystemTime.now;

            /// <summary>
            /// </summary>
            /// <returns>Diff in seconds since last call</returns>
            public static ulong update()
            {
                ulong systemTime = SystemTime.now;
                ulong datediff = systemTime - lastSystemTime;
                lastSystemTime = systemTime;
                return datediff;
            }

            private static ulong now
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
            return lh.time == rh.time;
        }

        public static bool operator !=(GameTime lh, GameTime rh)
        {
            return lh.time != rh.time;
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
    }
}
