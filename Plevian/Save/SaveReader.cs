using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace Plevian.Save
{
    class SaveReader
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
                Dictionary<string, int> test = new Dictionary<string, int>();
                if (i == 0)
                {
                    test.Add("messageCounter", int.Parse(counterRoot.Element("messageCounter").Value));
                    test.Add("technologyCounter", int.Parse(counterRoot.Element("technologyCounter").Value));
                    test.Add("villageCounter", villageCounter);
                }
                else
                {
                    test.Add("buildingCounter", int.Parse(counterRoot.Element("village" + i).Element("buildingCounter").Value));
                    test.Add("armyCounter", int.Parse(counterRoot.Element("village" + i).Element("armyCounter").Value));
                    test.Add("buildingQueueCounter", int.Parse(counterRoot.Element("village" + i).Element("buildingQueueCounter").Value));
                    test.Add("researchQueueCounter", int.Parse(counterRoot.Element("village" + i).Element("researchQueueCounter").Value));
                    test.Add("recruitQueueCounter", int.Parse(counterRoot.Element("village" + i).Element("recruitQueueCounter").Value));
                    test.Add("queueCounter", int.Parse(counterRoot.Element("village" + i).Element("queueCounter").Value));
                }

                if (i == 0)
                    counters.Add("basicCounters", test);
                else
                    counters.Add("village" + i, test);
            }

            return counters;
        }

        private ObservableCollection<Villages.Village> getVillages(string path, Dictionary<string, Dictionary<string, int>> counters, Players.Player player)
        {
            ObservableCollection<Villages.Village> villages = new ObservableCollection<Villages.Village>();
            Dictionary<string, int> basicCounters = counters["basicCounters"];

            XDocument villagesXml = XDocument.Load(path);
            XElement villagesRoot = villagesXml.Root;

            for (int i = 1; i <= basicCounters["villageCounter"]; i++)
            {
                Dictionary<string, int> villageCounters = counters["village" + i];
                XElement villageRoot = villagesRoot.Element("village" + i);

                int x = int.Parse(villageRoot.Element("location").Element("x").Value);
                int y = int.Parse(villageRoot.Element("location").Element("y").Value);

                Villages.Village village = new Villages.Village(new Maps.Location(x, y), player, villageRoot.Element("name").Value);

                village.resources.food = int.Parse(villageRoot.Element("resources").Element("food").Value);
                village.resources.iron = int.Parse(villageRoot.Element("resources").Element("iron").Value);
                village.resources.stone = int.Parse(villageRoot.Element("resources").Element("stone").Value);
                village.resources.wood = int.Parse(villageRoot.Element("resources").Element("wood").Value);
                
                int j = 1;
                var unitType = Enum.GetValues(typeof(Units.UnitType));
                foreach (Units.UnitType unit in unitType)
                {
                    if (village.army[unit].unitType.ToString() == villageRoot.Element("armies").Element("army" + j).Element("unitType").Value &&
                        village.army[unit].name == villageRoot.Element("armies").Element("army" + j).Element("name").Value)
                    {
                        village.army[unit].quantity = int.Parse(villageRoot.Element("armies").Element("army" + j).Element("quantity").Value);
                    }
                    j++;
                }

                int k = 1;
                foreach (KeyValuePair<Buildings.BuildingType, Buildings.Building> building in village.buildings)
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
                        Buildings.Building toBuild;

                        foreach (KeyValuePair<Buildings.BuildingType, Buildings.Building> building in village.buildings)
                        {
                            if (building.Key.ToString() == type)
                            {
                                toBuild = building.Value;
                                toBuild.level = int.Parse(villageRoot.Element("queues").Element("buildingQueue" + l).Element("toBuild").Element("level").Value);

                                GameTime start = GameTime.now;
                                GameTime end = GameTime.now;
                                start.time = int.Parse(villageRoot.Element("queues").Element("buildingQueue" + l).Element("Start").Value);
                                end.time = int.Parse(villageRoot.Element("queues").Element("buildingQueue" + l).Element("End").Value);

                                Villages.Queues.QueueItem buildingQueue = new Buildings.BuildingQueueItem(start,
                                                                                                          end,
                                                                                                          toBuild,
                                                                                                          int.Parse(villageRoot.Element("queues").Element("buildingQueue" + l).Element("level").Value));

                                village.queues.queue.Add(buildingQueue);
                                village.queues.buildingQueue.Add(buildingQueue as Buildings.BuildingQueueItem);

                                break;
                            }
                        }
                    }
                    
                    if (l <= villageCounters["researchQueueCounter"])
                    {
                        TechnologY.Technology researched;
                        for (int m = 1; m <= basicCounters["technologyCounter"]; m++)
                        {
                            if (player.technologies.technologies[m - 1].Name == villageRoot.Element("queues").Element("researchQueue" + l).Element("name").Value)
                            {
                                researched = player.technologies.technologies[m - 1];

                                GameTime start = GameTime.now;
                                GameTime end = GameTime.now;
                                start.time = int.Parse(villageRoot.Element("queues").Element("researchQueue" + l).Element("Start").Value);
                                end.time = int.Parse(villageRoot.Element("queues").Element("researchQueue" + l).Element("End").Value);
                                Villages.Queues.QueueItem researchQueue = new TechnologY.ResearchQueueItem(start, end, researched);

                                village.queues.queue.Add(researchQueue);
                                village.queues.researchQueue.Add(researchQueue as TechnologY.ResearchQueueItem);

                                break;
                            }
                        }
                    }

                    if (l <= villageCounters["recruitQueueCounter"])
                    {
                        Units.Unit toRecruit;
                        foreach (Units.UnitType unit in unitType)
                        {
                            if (village.army[unit].name == villageRoot.Element("queues").Element("recruitQueue" + l).Element("name").Value)
                            {
                                toRecruit = village.army[unit];
                                toRecruit.quantity = 1;

                                GameTime start = GameTime.now;
                                GameTime end = GameTime.now;
                                start.time = int.Parse(villageRoot.Element("queues").Element("recruitQueue" + l).Element("Start").Value);
                                end.time = int.Parse(villageRoot.Element("queues").Element("recruitQueue" + l).Element("End").Value);

                                Villages.Queues.QueueItem recruitQueue = new Units.RecruitQueueItem(start, end, toRecruit);

                                village.queues.queue.Add(recruitQueue);
                                village.queues.recruitQueue.Add(recruitQueue as Units.RecruitQueueItem);

                                break;
;                            }
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
                    Villages.Village buffer = villages[0];
                    villages[0] = villages[i];
                    villages[i] = buffer;

                    return villages;
                }
            }

            return villages;
        }

        private ObservableCollection<Messages.Message> getMessages(string path, int messagesCounter)
        {
            ObservableCollection<Messages.Message> messages = new ObservableCollection<Messages.Message>();
            XDocument messagesXml = XDocument.Load(path);
            XElement messagesRoot = messagesXml.Root;

            for (int i = 1; i <= messagesCounter; i++)
            {
                System.DateTime date = System.DateTime.Parse(messagesRoot.Element("message" + i).Element("date").Value);
                Messages.Message message = new Messages.Message(messagesRoot.Element("message" + i).Element("sender").Value,
                                                                messagesRoot.Element("message" + i).Element("topic").Value,
                                                                messagesRoot.Element("message" + i).Element("text").Value,
                                                                date);
                messages.Add(message);
            }

            return messages;
        }

        private List<TechnologY.Technology> getTechnologies(string path, Dictionary<string, int> basicCounters, Players.Player player)
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

        public List<Players.Player> getPlayers()
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

            List<Players.Player> players = new List<Players.Player>();

            Dictionary<string, Dictionary<string, int>> counters;
            Dictionary<string, int> basicCounters;

            //test ----------
            for (int i = 0; i < basicInfoPaths.Length; i++)
            {
                counters = this.getCounters(countersPaths[i]);
                basicCounters = counters["basicCounters"];

                XDocument basicInfoXml = XDocument.Load(basicInfoPaths[i]);
                XElement basicInfoRoot = basicInfoXml.Root;

                SFML.Graphics.Color color = new SFML.Graphics.Color(System.Byte.Parse(basicInfoRoot.Element("color").Element("R").Value),
                                                                    System.Byte.Parse(basicInfoRoot.Element("color").Element("G").Value),
                                                                    System.Byte.Parse(basicInfoRoot.Element("color").Element("B").Value),
                                                                    System.Byte.Parse(basicInfoRoot.Element("color").Element("A").Value));
                string name = basicInfoRoot.Element("name").Value;

                Players.Player player;

                if(basicInfoRoot.Element("computer").Value == "true")
                    player = new Players.ComputerPlayer(name, color);
                else
                    player = new Players.Player(name, color);

                player.messages = this.getMessages(messagesPaths[i], basicCounters["messageCounter"]);
                player.technologies.technologies = this.getTechnologies(technologiesPaths[i], basicCounters, player);
                player.villages = this.getVillages(villagesPaths[i], counters, player);

                players.Add(player);
            }
            return players;
        }

        public Maps.Map getMap(List<Players.Player> players)
        {
            string mapPath = this.path + "map\\";
            XDocument mapXml = XDocument.Load(mapPath + "map.xml");
            XElement mapRoot = mapXml.Element("map");

            Maps.Map map = new Maps.Map(int.Parse(mapRoot.Element("size").Element("x").Value), int.Parse(mapRoot.Element("size").Element("y").Value));

            foreach(Players.Player player in players)
                foreach (Villages.Village village in player.villages)
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
                            Maps.Location loc = new Maps.Location(int.Parse(mapTile.Element("location").Element("x").Value),
                                                                  int.Parse(mapTile.Element("location").Element("y").Value));
                            map.place(new Maps.Tile(loc, Maps.TerrainType.MOUNTAINS));
                            break;
                        }
                    case "LAKES":
                        {
                            Maps.Location loc = new Maps.Location(int.Parse(mapTile.Element("location").Element("x").Value),
                                                                  int.Parse(mapTile.Element("location").Element("y").Value));
                            map.place(new Maps.Tile(loc, Maps.TerrainType.LAKES));
                            break;
                        }
                }
            }

            int time = int.Parse(mapRoot.Element("time").Value);
            GameTime.init(time);

            return map;
        }
    }
}
