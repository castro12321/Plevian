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
        private Player mainPlayer;
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
            
            mainPlayer = createPlayer("Player", new Color(255, 106, 0));
            mainPlayer.SendMessage(new Message("System", "Welcome", "Welcome to the game!", DateTime.Parse("2014-08-13")));
            mainPlayer.SendMessage(new Message("God", "Meaning of the life", "Win the game", DateTime.Parse("2014-08-14 13:52")));
            mainPlayer.SendMessage(new Message("Enemy", "Message to you", "I'll kill you", DateTime.Now));

            Village playerCapital = createVillage(mainPlayer, "Capital");
            playerCapital.addResources(new Resource.Resources(9999, 9999, 9999, 9999));
            //playerCapital.build(BuildingType.TOWN_HALL);
            //playerCapital.addUnit(new Warrior(10));

            Player enemy = createPlayer("Enemy", SFML.Graphics.Color.Red, true);
            Village enemyCapital = createVillage(enemy, "EnemyLand");
            //enemyCapital.addUnit(new Warrior(10));

            /**
            Village hamburger = createVillage(enemy, "Hamburger");
            hamburger.takeResources(hamburger.resources);
            /**/

            //mainPlayer = enemy;

            /**
            for (int i = 0; i < 3; ++i)
                foreach (Player p in players)
                    foreach (Village v in p.villages)
                        foreach (var pair in v.buildings)
                        {
                            Building building = pair.Value;
                            if (building.getMaxLevel() >= i)
                                building.upgrade();
                        }
             /**/
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
            // We assume that a tick = 1 second
            ulong ticks = GameTime.update();

            if(ticks > 0)
            {
                foreach (Player player in players)
                    player.tick(ticks);
                GameStats.collect();
                Logger.tick();
            }
        }

        public Player createPlayer(String nick, SFML.Graphics.Color color, bool ai = false)
        {
            Player player = ai ? new AiPlayer(nick, color) : new Player(nick, color);
            players.Add(player);
            return player;
        }

        public Village createVillage(Player owner, String name)
        {
            Village village = new Village(map.FindEmptyTile().location, owner, name);
            map.place(village);
            owner.addVillage(village);
            return village;
        }
    }
}
