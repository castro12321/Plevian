﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian
{
    public class GameTime
    {
        private static DateTime epoch = new DateTime(1970, 1, 1);
        private ulong systemTime = 0;
        public static LocalTime gameTime { get; private set; }

        public GameTime(LocalTime gameTime)
        {
            GameTime.gameTime = gameTime;
            systemTime = currentTimeSeconds();
        }

        public ulong updateTime()
        {
            ulong currentSystemTime = currentTimeSeconds();
            ulong datediff = currentSystemTime - systemTime;
            systemTime = currentSystemTime;
            gameTime.addSeconds(datediff);
            return datediff;
        }

        public ulong currentTimeSeconds()
        {
            DateTime currentTime = DateTime.UtcNow;
            return (ulong)((currentTime - epoch).TotalSeconds);
        }

        public static LocalTime add(LocalTime time)
        {
            return gameTime + time;
        }
    }
}