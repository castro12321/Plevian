using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Villages;
using Plevian.Maps;
using Plevian.Players;

namespace Plevian
{
    public class Game
    {
        /// <summary>The main human-player that is playing the game right now</summary>
        public readonly Player player;
        public readonly GameTime gameTime;
        public readonly Map map;

        /// <summary>
        /// Initializes brand new game
        /// </summary>
        public Game()
        {
            GameTime.init(0);
            map = new MapGenerator().Generate(60, 60);
            player = new Player("Magnus", SFML.Graphics.Color.Blue);

            Tile capitalTile = map.FindEmptyTile();
            Village capital = new Village(capitalTile.location);
            map.place(capital);
            player.addVillage(capital);
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

        public void tick()
        {
            ulong timediff = GameTime.update();
            while (timediff --> 0)
            {
                player.Capital.tick();
            }
        }
    }
}
