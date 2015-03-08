using Plevian.Buildings;
using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Messages;
using Plevian.Players;
using Plevian.RequirementS;
using Plevian.Save;
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
        private Player mainPlayer = new Player("Magnus", new Color(255, 106, 0));
        public static Player Player { get { return game.mainPlayer; } }
        public static Game game;
        public readonly List<Player> players = new List<Player>();
        //public readonly GameTime gameTime;
        public readonly Map map;

        public Game()
        {
            Game.game = this;
            GameTime.init(0);
            GameTime.setSpeed(GameTime.setSpeedToAfterGameStarted);
            this.map = new MapGenerator().Generate(30, 30);
            
            mainPlayer.SendMessage(new Message("System", "Welcome", "Welcome to the game!", DateTime.Parse("2014-08-13")));
            mainPlayer.SendMessage(new Message("God", "Meaning of the life", "Win the game", DateTime.Parse("2014-08-14 13:52")));
            mainPlayer.SendMessage(new Message("Hitler", "Message to you", "I'll kill you", DateTime.Now));

            Tile village1Tile = map.FindEmptyTile();
            Village village1 = new Village(village1Tile.location, mainPlayer, "Capital");
            village1.addResources(new Resource.Resources(1, 501, 1001, 1501));
            //village1.build(BuildingType.TOWN_HALL);
            village1.addUnit(new Warrior(3));
            map.place(village1);
            mainPlayer.addVillage(village1);
            addPlayer(mainPlayer);

            Player enemy = new AiPlayer("Hitler", SFML.Graphics.Color.Red);
            //mainPlayer = enemy;
            Tile berlinTile = map.FindEmptyTile();
            Village berlin = new Village(berlinTile.location, enemy, "Berlin");
            berlin.addUnit(new Warrior(10));
            map.place(berlin);
            enemy.addVillage(berlin);
            
            addPlayer(enemy);
            /*
            for (int i = 0; i < 10; ++i)
                foreach (var pair in berlin.buildings)
                {
                    Building building = pair.Value;
                    if (building.getMaxLevel() <= i)
                        building.upgrade();
                }
             */
        }
        
        /// <summary>
        /// Initializes brand new game
        /// </summary>
        public Game(SaveReader save)
        {
            Game.game = this;
            GameTime.init(save.getGameTime());
            this.players = save.getPlayers();
            this.mainPlayer = players[0];
            this.map = save.getMap(players);
        }

        /// <summary>
        /// Initializes a game from save
        /// </summary>
        /// <param name="save">save to initialize from</param>

        public void tick()
        {
            ulong timediff = GameTime.update();
            if(timediff > 0)
            {
                foreach (Player player in players)
                    player.tick();
                GameStats.collect();
                Logger.tick();
            }
        }

        public void addPlayer(Player player)
        {
            players.Add(player);
        }
    }
}
