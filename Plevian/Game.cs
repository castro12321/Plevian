using Plevian.Buildings;
using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Messages;
using Plevian.Players;
using Plevian.RequirementS;
using Plevian.Units;
using Plevian.Villages;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plevian
{
    public class Game
    {
        /// <summary>The main human-player that is playing the game right now</summary>
        public static Player player  = new Player("Magnus", new Color(255, 106, 0));
        public static Game game;
        public readonly List<Player> players = new List<Player>();
        public readonly GameTime gameTime;
        public readonly Map map;
        
        /// <summary>
        /// Initializes brand new game
        /// </summary>
        public Game()
        {
            Game.game = this;
            GameTime.init(0);
            map = new MapGenerator().Generate(30, 30);

            player.SendMessage(new Message("System", "Welcome", "Welcome to the game!", DateTime.Parse("2014-08-13")));
            player.SendMessage(new Message("God", "Meaning of the life", "Win the game", DateTime.Parse("2014-08-14 13:52")));
            player.SendMessage(new Message("Hitler", "Message to you", "I'll kill you", DateTime.Now));

            Tile village1Tile = map.FindEmptyTile();
            Tile village2Tile = map.FindEmptyTile();
            Tile village3Tile = map.FindEmptyTile();
            Village village1 = new Village(village1Tile.location, player, "Capital");
            //Village village2 = new Village(village2Tile.location, player, "Luxemburg");
            //Village village3 = new Village(village3Tile.location, player, "Warszawa" );
            map.place(village1);
            //map.place(village2);
            //map.place(village3);
            player.addVillage(village1);
            //player.addVillage(village2);
            //player.addVillage(village3);
            addPlayer(player);

            Player enemy = new ComputerPlayer("Hitler", SFML.Graphics.Color.Red);
            player = enemy;
            Tile berlinTile    = map.FindEmptyTile();
            Tile frankfurtTile = map.FindEmptyTile();
            Tile hamburgTile   = map.FindEmptyTile();
            Village berlin     = new Village(berlinTile.location, enemy, "Berlin");
            //Village frankfurt  = new Village(frankfurtTile.location, enemy, "Frankfurt");
            //Village hamburger  = new Village(hamburgTile.location, enemy, "Hamburger");
            map.place(berlin);
            //map.place(frankfurt);
            //map.place(hamburger);
            enemy.addVillage(berlin);
            //enemy.addVillage(frankfurt);
            //enemy.addVillage(hamburger);
            addPlayer(enemy);

            village1.addUnit(new Trader(3));
            village1.addUnit(new Knight(10));
            village1.build(BuildingType.TOWN_HALL);
            village1.addUnit(new Duke(100));
            //village2.addUnit(new Knight(200));
            //village2.addUnit(new Archer(500));
            //village3.addResources(new Resource.Resources(999999, 999999, 999999, 999999));
            //village3.getBuilding(BuildingType.BARRACKS).level = 1;
            //village3.recruit(new Warrior(1000));
            berlin.addUnit(new Knight(1000));
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
            if(timediff > 0)
            {
                foreach (Player player in players)
                    player.tick();
            }
            GameStats.collect();
        }

        public void addPlayer(Player player)
        {
            players.Add(player);
        }
    }
}
