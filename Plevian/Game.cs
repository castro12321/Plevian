using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Villages;

namespace Plevian
{
    public class Game
    {
        public readonly GameTime gameTime;

        public Game()
        {
            gameTime = new GameTime(new LocalTime(0));
        }

        public Game(Save save)
        {
            // initialize from save
            gameTime = save.getGameTime();
        }

        private Village village = new Village();
        public void tick()
        {
            System.Threading.Thread.Sleep(250);
            ulong timediff = gameTime.updateTime();
            while (timediff --> 0)
            {
                village.tick();
            }
        }
    }
}
