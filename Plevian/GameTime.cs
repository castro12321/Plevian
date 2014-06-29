﻿using System;
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
        private static ulong lastSystemTime;
        private static int gameTime;
        private int time;
        public static GameTime now
        {
            get
            {
                return new GameTime(gameTime);
            }
        }

        protected GameTime(int time)
        {
            this.time = time;
        }

        public static void init(int time)
        {
            GameTime.gameTime = time;
            lastSystemTime = SystemTime.now;
        }

        public override string ToString()
        {
            return time.ToString();
        }

        /// <summary>
        /// </summary>
        /// <returns>Diff in seconds</returns>
        public static ulong update()
        {
            ulong systemTime = SystemTime.now;
            ulong datediff = systemTime - lastSystemTime;
            gameTime += (int)datediff;
            lastSystemTime = systemTime;
            return datediff;
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
