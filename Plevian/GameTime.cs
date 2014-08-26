using Plevian.Debugging;
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
        public static int speed = 2;
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
            init(new Seconds(time));
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
