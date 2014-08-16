using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Villages;
using Plevian.Maps;
using Plevian.Players;
using Plevian.Units;
using Plevian.Orders;
using SFML.Graphics;

namespace Plevian
{
    public class Game
    {
        /// <summary>The main human-player that is playing the game right now</summary>
        public static readonly Player player  = new Player("Magnus", new Color(255, 106, 0));
        public readonly List<Player> players = new List<Player>();
        public readonly GameTime gameTime;
        public readonly Map map;

        /// <summary>
        /// Initializes brand new game
        /// </summary>
        public Game()
        {
            GameTime.init(0);
            map = new MapGenerator().Generate(30, 30);

            Tile village1Tile = map.FindEmptyTile();
            Tile village2Tile = map.FindEmptyTile();
            Tile village3Tile = map.FindEmptyTile();
            Village village1 = new Village(village1Tile.location, player, "Capital");
            Village village2 = new Village(village2Tile.location, player, "Luxemburg");
            Village village3 = new Village(village3Tile.location, player, "Warszawa" );
            map.place(village1);
            map.place(village2);
            map.place(village3);
            player.addVillage(village1);
            player.addVillage(village2);
            player.addVillage(village3);
            addPlayer(player);

            Player enemy = new Player("Hitler", SFML.Graphics.Color.Red);
            Tile berlinTile    = map.FindEmptyTile();
            Tile frankfurtTile = map.FindEmptyTile();
            Tile hamburgTile   = map.FindEmptyTile();
            Village berlin     = new Village(berlinTile.location, enemy, "Berlin");
            Village frankfurt  = new Village(frankfurtTile.location, enemy, "Frankfurt");
            Village hamburger  = new Village(hamburgTile.location, enemy, "Hamburger");
            map.place(berlin);
            map.place(frankfurt);
            map.place(hamburger);
            enemy.addVillage(berlin);
            enemy.addVillage(frankfurt);
            enemy.addVillage(hamburger);
            addPlayer(enemy);

            Army army = new Army();
            army += new Knight(100);

            AttackOrder order = new AttackOrder(village1, berlin, 0.1f, army);
            village1.addUnit(new Knight(1000));
            village2.addUnit(new Knight(200));
            village2.addUnit(new Archer(500));
            village3.addResources(new Resource.Resources(999999, 999999, 999999, 999999));
            village3.recruit(new Warrior(1000));
            village1.addOrder(order);
            berlin.addUnit(new Knight(500));
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
                foreach (Player player in players)
                    foreach (Village village in player.Villages)
                        village.tick();
            }
        }

        public void addPlayer(Player player)
        {
            players.Add(player);
        }
    }
}
