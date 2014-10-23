using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using Plevian.Buildings;
using Plevian.Villages;
using Plevian.Players;
using Plevian.Maps;
using Plevian.Units;
using Plevian.Messages;
using Plevian.TechnologY;

namespace Plevian.Save
{
    public class SaveReader
    {
        private string path;

        public SaveReader(string name)
        {
            this.path = name + "\\";
        }

        private Dictionary<string, Dictionary<string, int>> getCounters(string path)
        {
            Dictionary<string, Dictionary<string, int>> counters = new Dictionary<string, Dictionary<string, int>>();

            XDocument countersXml = XDocument.Load(path);
            XElement counterRoot = countersXml.Root;

            int villageCounter = int.Parse(counterRoot.Element("villageCounter").Value);
            for (int i = 0; i <= villageCounter; i++)
            {
                Dictionary<string, int> buffer = new Dictionary<string, int>();
                if (i == 0)
                {
                    buffer.Add("messageCounter", int.Parse(counterRoot.Element("messageCounter").Value));
                    buffer.Add("technologyCounter", int.Parse(counterRoot.Element("technologyCounter").Value));
                    buffer.Add("villageCounter", villageCounter);
                }
                else
                {
                    buffer.Add("buildingCounter", int.Parse(counterRoot.Element("village" + i).Element("buildingCounter").Value));
                    buffer.Add("armyCounter", int.Parse(counterRoot.Element("village" + i).Element("armyCounter").Value));
                    buffer.Add("buildingQueueCounter", int.Parse(counterRoot.Element("village" + i).Element("buildingQueueCounter").Value));
                    buffer.Add("researchQueueCounter", int.Parse(counterRoot.Element("village" + i).Element("researchQueueCounter").Value));
                    buffer.Add("recruitQueueCounter", int.Parse(counterRoot.Element("village" + i).Element("recruitQueueCounter").Value));
                    buffer.Add("queueCounter", int.Parse(counterRoot.Element("village" + i).Element("queueCounter").Value));
                }

                if (i == 0)
                    counters.Add("basicCounters", buffer);
                else
                    counters.Add("village" + i, buffer);
            }

            return counters;
        }

        private ObservableCollection<Village> getVillages(string path, Dictionary<string, Dictionary<string, int>> counters, Player player)
        {
            ObservableCollection<Village> villages = new ObservableCollection<Village>();
            Dictionary<string, int> basicCounters = counters["basicCounters"];

            XDocument villagesXml = XDocument.Load(path);
            XElement villagesRoot = villagesXml.Root;

            for (int i = 1; i <= basicCounters["villageCounter"]; i++)
            {
                Dictionary<string, int> villageCounters = counters["village" + i];
                XElement villageRoot = villagesRoot.Element("village" + i);

                int x = int.Parse(villageRoot.Element("location").Element("x").Value);
                int y = int.Parse(villageRoot.Element("location").Element("y").Value);

                Village village = new Village(new Location(x, y), player, villageRoot.Element("name").Value);

                village.resources.food = int.Parse(villageRoot.Element("resources").Element("food").Value);
                village.resources.iron = int.Parse(villageRoot.Element("resources").Element("iron").Value);
                village.resources.stone = int.Parse(villageRoot.Element("resources").Element("stone").Value);
                village.resources.wood = int.Parse(villageRoot.Element("resources").Element("wood").Value);
                
                int j = 1;
                var unitType = Enum.GetValues(typeof(UnitType));
                foreach (UnitType unit in unitType)
                {
                    if (village.army[unit].unitType.ToString() == villageRoot.Element("armies").Element("army" + j).Element("unitType").Value &&
                        village.army[unit].name == villageRoot.Element("armies").Element("army" + j).Element("name").Value)
                    {
                        village.army[unit].quantity = int.Parse(villageRoot.Element("armies").Element("army" + j).Element("quantity").Value);
                    }
                    j++;
                }

                int k = 1;
                foreach (KeyValuePair<BuildingType, Building> building in village.buildings)
                {
                    if (building.Key.ToString() == villageRoot.Element("buildings").Element("building" + k).Element("key").Value)
                    {
                        building.Value.level = int.Parse(villageRoot.Element("buildings").Element("building" + k).Element("level").Value);
                    }
                    k++;
                }

                for (int l = 1; l <= villageCounters["queueCounter"]; l++)
                {
                    if (l <= villageCounters["buildingQueueCounter"])
                    {
                        string type = villageRoot.Element("queues").Element("buildingQueue" + l).Element("toBuild").Element("type").Value;
                        Building toBuild;

                        foreach (KeyValuePair<BuildingType, Building> building in village.buildings)
                        {
                            if (building.Key.ToString() == type)
                            {
                                toBuild = building.Value;
                                toBuild.level = int.Parse(villageRoot.Element("queues").Element("buildingQueue" + l).Element("toBuild").Element("level").Value);

                                GameTime start = GameTime.now;
                                GameTime end = GameTime.now;
                                start.time = int.Parse(villageRoot.Element("queues").Element("buildingQueue" + l).Element("Start").Value);
                                end.time = int.Parse(villageRoot.Element("queues").Element("buildingQueue" + l).Element("End").Value);
                                int level = int.Parse(villageRoot.Element("queues").Element("buildingQueue" + l).Element("level").Value);
                                BuildingQueueItem buildingQueue = new BuildingQueueItem(start, end, toBuild, level);

                                village.queues.Add(buildingQueue);

                                break;
                            }
                        }
                    }
                    
                    if (l <= villageCounters["researchQueueCounter"])
                    {
                        Technology researched;
                        for (int m = 1; m <= basicCounters["technologyCounter"]; m++)
                        {
                            if (player.technologies.technologies[m - 1].Name == villageRoot.Element("queues").Element("researchQueue" + l).Element("name").Value)
                            {
                                researched = player.technologies.technologies[m - 1];

                                GameTime start = GameTime.now;
                                GameTime end = GameTime.now;
                                start.time = int.Parse(villageRoot.Element("queues").Element("researchQueue" + l).Element("Start").Value);
                                end.time = int.Parse(villageRoot.Element("queues").Element("researchQueue" + l).Element("End").Value);
                                ResearchQueueItem researchQueue = new ResearchQueueItem(start, end, researched);

                                village.queues.Add(researchQueue);
                                break;
                            }
                        }
                    }

                    if (l <= villageCounters["recruitQueueCounter"])
                    {
                        foreach (UnitType unit in unitType)
                        {
                            Unit toRecruit;
                            if (village.army[unit].name == villageRoot.Element("queues").Element("recruitQueue" + l).Element("name").Value)
                            {
                                toRecruit = village.army[unit].clone();
                                toRecruit.quantity = 1;

                                GameTime start = GameTime.now;
                                GameTime end = GameTime.now;
                                start.time = int.Parse(villageRoot.Element("queues").Element("recruitQueue" + l).Element("Start").Value);
                                end.time = int.Parse(villageRoot.Element("queues").Element("recruitQueue" + l).Element("End").Value);

                                RecruitQueueItem recruitQueue = new RecruitQueueItem(start, end, toRecruit);

                                village.queues.Add(recruitQueue);
                                break;
                            }
                        }
                    }
                }

                village.buildTimeEnd.time = int.Parse(villageRoot.Element("builtTimeEnd").Value);
                village.recruitTimeEnd.time = int.Parse(villageRoot.Element("recruitTimeEnd").Value);
                village.researchTimeEnd.time = int.Parse(villageRoot.Element("builtTimeEnd").Value);

                villages.Add(village);
            }

            for (int i = 0; i < villages.Count; i++)
            {
                if (villages[i].name == villagesRoot.Element("village" + (i + 1)).Element("name").Value &&
                    villagesRoot.Element("village" + (i + 1)).Element("capital").Value == "true")
                {
                    Village buffer = villages[0];
                    villages[0] = villages[i];
                    villages[i] = buffer;

                    return villages;
                }
            }

            return villages;
        }

        private ObservableCollection<Message> getMessages(string path, int messagesCounter)
        {
            ObservableCollection<Message> messages = new ObservableCollection<Message>();
            XDocument messagesXml = XDocument.Load(path);
            XElement messagesRoot = messagesXml.Root;

            for (int i = 1; i <= messagesCounter; i++)
            {
                DateTime date = DateTime.Parse(messagesRoot.Element("message" + i).Element("date").Value);
                Message message = new Message(messagesRoot.Element("message" + i).Element("sender").Value,
                                                                messagesRoot.Element("message" + i).Element("topic").Value,
                                                                messagesRoot.Element("message" + i).Element("text").Value,
                                                                date);
                messages.Add(message);
            }

            return messages;
        }

        private List<Technology> getTechnologies(string path, Dictionary<string, int> basicCounters, Player player)
        {
            XDocument techXml = XDocument.Load(path);
            XElement techRoot = techXml.Root;

            for (int i = 1; i <= basicCounters["technologyCounter"]; i++)
            {
                for (int j = 0; j <= player.technologies.technologies.Count; j++)
                    if (player.technologies.technologies[j].Name == techRoot.Element("technology" + i).Element("name").Value)
                    {
                        player.technologies.technologies[j].researched = bool.Parse(techRoot.Element("technology" + i).Element("researched").Value);
                        break;
                    }
            }

            return player.technologies.technologies;
        }

        public List<Player> getPlayers()
        {
            string playersPath = this.path + "players\\";
            string playersBasicInfo = playersPath + "basicInfo\\";
            string playersMessages = playersPath + "messages\\";
            string playersTechnologies = playersPath + "technologies\\";
            string playersVillages = playersPath + "villages\\";
            string playersCounters = playersPath + "counters\\";

            string[] basicInfoPaths = Directory.GetFiles(playersBasicInfo);
            string[] messagesPaths = Directory.GetFiles(playersMessages);
            string[] technologiesPaths = Directory.GetFiles(playersTechnologies);
            string[] villagesPaths = Directory.GetFiles(playersVillages);
            string[] countersPaths = Directory.GetFiles(playersCounters);

            List<Player> players = new List<Player>();

            Dictionary<string, Dictionary<string, int>> counters;
            Dictionary<string, int> basicCounters;
         
            for (int i = 0; i < basicInfoPaths.Length; i++)
            {
                counters = this.getCounters(countersPaths[i]);
                basicCounters = counters["basicCounters"];

                XDocument basicInfoXml = XDocument.Load(basicInfoPaths[i]);
                XElement basicInfoRoot = basicInfoXml.Root;

                SFML.Graphics.Color color = new SFML.Graphics.Color(Byte.Parse(basicInfoRoot.Element("color").Element("R").Value),
                                                                    Byte.Parse(basicInfoRoot.Element("color").Element("G").Value),
                                                                    Byte.Parse(basicInfoRoot.Element("color").Element("B").Value),
                                                                    Byte.Parse(basicInfoRoot.Element("color").Element("A").Value));
                string name = basicInfoRoot.Element("name").Value;

                Player player;

                if(basicInfoRoot.Element("computer").Value == "true")
                    player = new Players.ComputerPlayer(name, color);
                else
                    player = new Player(name, color);

                player.messages = this.getMessages(messagesPaths[i], basicCounters["messageCounter"]);
                player.technologies.technologies = this.getTechnologies(technologiesPaths[i], basicCounters, player);
                player.villages = this.getVillages(villagesPaths[i], counters, player);

                players.Add(player);
            }

            return players;
        }

        public Map getMap(List<Player> players)
        {
            string mapPath = this.path + "map\\";
            XDocument mapXml = XDocument.Load(mapPath + "map.xml");
            XElement mapRoot = mapXml.Element("map");

            Map map = new Map(int.Parse(mapRoot.Element("size").Element("x").Value), int.Parse(mapRoot.Element("size").Element("y").Value));

            foreach(Player player in players)
                foreach (Village village in player.villages)
                {
                    map.place(village);
                }

            for (int i = 1; i <= (map.sizeX * map.sizeY); i++)
            {
                XElement mapTile = mapRoot.Element("tiles").Element("tile" + i);

                switch (mapTile.Element("type").Value)
                {
                    case "MOUNTAINS":
                        {
                            Location loc = new Location(int.Parse(mapTile.Element("location").Element("x").Value),
                                                        int.Parse(mapTile.Element("location").Element("y").Value));
                            map.place(new Tile(loc, TerrainType.MOUNTAINS));
                            break;
                        }
                    case "LAKES":
                        {
                            Location loc = new Location(int.Parse(mapTile.Element("location").Element("x").Value),
                                                        int.Parse(mapTile.Element("location").Element("y").Value));
                            map.place(new Tile(loc, TerrainType.LAKES));
                            break;
                        }
                }
            }

            return map;
        }

        public int getGameTime()
        {
            string mapPath = this.path + "map\\";
            XDocument mapXml = XDocument.Load(mapPath + "map.xml");
            XElement mapRoot = mapXml.Element("map");


            int time = int.Parse(mapRoot.Element("time").Value);
            GameTime.init(time);

            return time;
        }
    }
}
