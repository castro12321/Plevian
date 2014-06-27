using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public void tick()
        {
            gameTime.updateTime();
        }
    }
}
