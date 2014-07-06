using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Villages;
using Plevian.Maps;

namespace Plevian
{
    public class Game
    {
        public readonly GameTime gameTime;
        public readonly Map map;

        public Game()
        {
            GameTime.init(0);
            map = new MapGenerator().Generate(10, 10);
        }

        /// <summary>
        /// Initializes a game from save
        /// </summary>
        /// <param name="save">save to initialize from</param>
        public Game(Save save)
        {
            gameTime = save.getGameTime();
            map = new MapFileReader().read(save);
        }

        private Village village = new Village();
        public void tick()
        {
            ulong timediff = GameTime.update();
            while (timediff --> 0)
            {
                village.tick();
            }
        }
    }
}
