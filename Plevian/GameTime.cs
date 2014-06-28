using System;
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
        public static LocalTime time { get; private set; }

        public GameTime(LocalTime gameTime)
        {
            GameTime.time = gameTime;
            systemTime = currentTimeSeconds();
        }

        public ulong updateTime()
        {
            ulong currentSystemTime = currentTimeSeconds();
            ulong datediff = currentSystemTime - systemTime;
            systemTime = currentSystemTime;
            time.addSeconds(datediff);
            return datediff;
        }

        private ulong currentTimeSeconds()
        {
            DateTime currentTime = DateTime.UtcNow;
            return (ulong)((currentTime - epoch).TotalSeconds);
        }

        public static LocalTime add(LocalTime time)
        {
            return GameTime.time + time;
        }
    }
}
