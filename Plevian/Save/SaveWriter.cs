﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Plevian.Save
{
    public class SaveWriter
    {
        private readonly String path;
        private int messageCounter;
        private int technologyCounter;
        private int villageCounter;
        private int buildingCounter;
        private int armyCounter;
        private int buildingQueueCounter;
        private int queueCounter;
        private int tilesCounter;
        //private int playerCounter = 1;

        public SaveWriter(String path)
        {
            Directory.CreateDirectory("Save\\");

            this.path = "Save\\" + path + "\\";
            //if (!Directory.Exists(path))
            Directory.CreateDirectory(this.path);
        }

        public void savePlayer(List<Players.Player> players)
        {
            string playersPath = this.path + "players\\";
            string playersBasicInfo = playersPath + "basicInfo\\";
            string playersMessages = playersPath + "messages\\";
            string playersTechnologies = playersPath + "technologies\\";
            string playersVillages = playersPath + "villages\\";

            Directory.CreateDirectory(playersPath);
            Directory.CreateDirectory(playersBasicInfo);
            Directory.CreateDirectory(playersMessages);
            Directory.CreateDirectory(playersTechnologies);
            Directory.CreateDirectory(playersVillages);
            int playerCounter = 1;

            foreach (Players.Player player in players)
            {
                StreamWriter basicInfoFile = new StreamWriter(playersBasicInfo + "player" + playerCounter.ToString() + ".xml");
                basicInfoFile.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-2\" standalone=\"no\" ?>");
                basicInfoFile.WriteLine("<name>" + player.name + "</name>");
                basicInfoFile.WriteLine("<color>(" + player.color.ToString() + ")</color>");
                basicInfoFile.Close();

                this.messageCounter = 0;
                StreamWriter messagesFile = new StreamWriter(playersMessages + "player" + playerCounter.ToString() + ".xml");
                messagesFile.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-2\" standalone=\"no\" ?>");
                messagesFile.WriteLine("<messages>");
                foreach (Messages.Message message in player.messages)
                {
                    this.messageCounter++;
                    messagesFile.WriteLine("\t<message" + messageCounter + ">");
                    messagesFile.WriteLine("\t\t<date>" + message.Date + "</date>");
                    messagesFile.WriteLine("\t\t<sender>" + message.Sender + "</sender>");
                    messagesFile.WriteLine("\t\t<topic>" + message.Topic + "</topic>");
                    messagesFile.WriteLine("\t\t<text>" + message.message + "</text>");
                    messagesFile.WriteLine("\t</message" + messageCounter + ">");
                }
                messagesFile.WriteLine("</messages>");
                messagesFile.Close();

                this.technologyCounter = 0;
                StreamWriter techFile = new StreamWriter(playersTechnologies + "player" + playerCounter.ToString() + ".xml");
                techFile.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-2\" standalone=\"no\" ?>");
                techFile.WriteLine("<technologies>");
                foreach(TechnologY.Technology tech in player.technologies.technologies)
                {
                    this.technologyCounter++;
                    techFile.WriteLine("\t<technology" + technologyCounter + ">");

                    techFile.WriteLine("\t\t<name>" + tech.Name + "</name>");

                    techFile.WriteLine("\t\t<price>");
                    techFile.WriteLine("\t\t\t<food>" + tech.Price.food + "</food>");
                    techFile.WriteLine("\t\t\t<iron>" + tech.Price.iron + "</iron>");
                    techFile.WriteLine("\t\t\t<stone>" + tech.Price.stone + "</stone>");
                    techFile.WriteLine("\t\t<wood>" + tech.Price.wood + "</wood>");
                    techFile.WriteLine("\t\t</price>");

                    techFile.WriteLine("\t\t<requirements>");
                    foreach (RequirementS.Requirement requirement in tech.Requirements.RequirementsList)
                    {
                        techFile.WriteLine("\t\t\t" + requirement.ToString());
                    }
                    techFile.WriteLine("\t\t</requirements>");

                    techFile.WriteLine("\t\t<research>");
                    techFile.WriteLine("\t\t\t<researched>" + tech.researched.ToString() + "</researched>");
                    techFile.WriteLine("\t\t\t<time>" + tech.ResearchTime.time + "</time>");
                    techFile.WriteLine("\t\t<research>");

                    techFile.WriteLine("\t</technology" + technologyCounter + ">");
                }
                techFile.WriteLine("</technologies>");
                techFile.Close();

                this.villageCounter = 0;
                StreamWriter villagesFile = new StreamWriter(playersVillages + "player" + playerCounter.ToString() + ".xml");
                villagesFile.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-2\" standalone=\"no\" ?>");
                villagesFile.WriteLine("<villages>");
                foreach (Villages.Village village in player.villages)
                {
                    this.villageCounter++;
                    villagesFile.WriteLine("\t<village" + villageCounter + ">");

                    if (player.Capital.name == village.name)
                        villagesFile.WriteLine("\t\t<capital>true</capital>");
                    else
                        villagesFile.WriteLine("\t\t<capital>false</capital>");

                    villagesFile.WriteLine("\t\t<name>" + village.name + "</name>");
                    villagesFile.WriteLine("\t\t<type>" + village.type + "</type>");

                    villagesFile.WriteLine("\t\t<location>");
                    villagesFile.WriteLine("\t\t\t<x>" + village.location.x + "</x>");
                    villagesFile.WriteLine("\t\t\t<y>" + village.location.y + "</y>");
                    villagesFile.WriteLine("\t\t</location>");

                    villagesFile.WriteLine("\t\t<owner>" + village.Owner.name + "</owner>");

                    villagesFile.WriteLine("\t\t<resources>");
                    villagesFile.WriteLine("\t\t\t<food>" + village.resources.food + "</food>");
                    villagesFile.WriteLine("\t\t\t<iron>" + village.resources.iron + "</iron>");
                    villagesFile.WriteLine("\t\t\t<stone>" + village.resources.stone + "</stone>");
                    villagesFile.WriteLine("\t\t\t<wood>" + village.resources.wood + "</wood>");
                    villagesFile.WriteLine("\t\t</resources>");

                    this.buildingCounter = 0;
                    villagesFile.WriteLine("\t\t<buildings>");
                    foreach (KeyValuePair<Buildings.BuildingType, Buildings.Building> building in village.buildings)
                    {
                        this.buildingCounter++;
                        villagesFile.WriteLine("\t\t\t<building" + buildingCounter + ">");
                        villagesFile.WriteLine("\t\t\t\t<key>" + building.Key + "</key>");
                        villagesFile.WriteLine("\t\t\t\t<level>" + building.Value.level + "</level>");

                        /*villagesFile.WriteLine("\t\t\t\t<requirements>");
                        foreach (RequirementS.Requirement requirement in building.Value.requirements)
                        {
                            villagesFile.WriteLine("\t\t\t\t\t<requirement>" + requirement.ToString() + "</requirement>");
                        }
                        villagesFile.WriteLine("\t\t\t\t</requirements>");*/

                        villagesFile.WriteLine("\t\t\t\t<type>" + building.Value.type.ToString() + "</type>");
                        villagesFile.WriteLine("\t\t\t</building" + buildingCounter + ">");
                    }
                    villagesFile.WriteLine("\t\t</buildings>");

                    villagesFile.WriteLine("\t\t<builtTimeEnd>" + village.buildTimeEnd.time + "</builtTimeEnd>");

                    this.armyCounter = 0;
                    villagesFile.WriteLine("\t\t<armies>");
                    var unitType = Enum.GetValues(typeof(Units.UnitType));
                    foreach (Units.UnitType unit in unitType)
                    {
                        this.armyCounter++;
                        villagesFile.WriteLine("\t\t\t<army" + armyCounter + ">");
                        villagesFile.WriteLine("\t\t\t\t<attackStrength>" + village.army[unit].attackStrength + "</attackStrength>");
                        villagesFile.WriteLine("\t\t\t\t<defenseArchers>" + village.army[unit].defenseArchers + "</defenseArchers>");
                        villagesFile.WriteLine("\t\t\t\t<defenseCavalry>" + village.army[unit].defenseCavalry + "</defenseCavalry>");
                        villagesFile.WriteLine("\t\t\t\t<defenseInfantry>" + village.army[unit].defenseInfantry + "</defenseInfantry>");

                        /*villagesFile.WriteLine("\t\t\t\t<DependencyObjectType>");
                        villagesFile.WriteLine("\t\t\t\t\t<BaseType>" + village.army[unit].DependencyObjectType.BaseType + "</BaseType>");
                        villagesFile.WriteLine("\t\t\t\t\t<Id>" + village.army[unit].DependencyObjectType.Id + "</Id>");
                        villagesFile.WriteLine("\t\t\t\t\t<Name>" + village.army[unit].DependencyObjectType.Name + "</Name>");
                        villagesFile.WriteLine("\t\t\t\t\t<SystemType>" + village.army[unit].DependencyObjectType.SystemType + "</SystemType>");
                        villagesFile.WriteLine("\t\t\t\t</DependencyObjectType>");*/

                        villagesFile.WriteLine("\t\t\t\t<IsSealed>" + village.army[unit].IsSealed + "</IsSealed>");
                        villagesFile.WriteLine("\t\t\t\t<lootCapacity>" + village.army[unit].lootCapacity + "</lootCapacity>");
                        villagesFile.WriteLine("\t\t\t\t<movementSpeed>" + village.army[unit].movementSpeed + "</movementSpeed>");
                        villagesFile.WriteLine("\t\t\t\t<name>" + village.army[unit].name + "</name>");
                        villagesFile.WriteLine("\t\t\t\t<quantity>" + village.army[unit].quantity + "</quantity>");

                        villagesFile.WriteLine("\t\t\t\t<recruitCost>");
                        villagesFile.WriteLine("\t\t\t\t\t<food>" + village.army[unit].recruitCost.food + "</food>");
                        villagesFile.WriteLine("\t\t\t\t\t<iron>" + village.army[unit].recruitCost.iron + "</iron>");
                        villagesFile.WriteLine("\t\t\t\t\t<stone>" + village.army[unit].recruitCost.stone + "</stone>");
                        villagesFile.WriteLine("\t\t\t\t\t<wood>" + village.army[unit].recruitCost.wood + "</wood>");
                        villagesFile.WriteLine("\t\t\t\t</recruitCost>");

                        villagesFile.WriteLine("\t\t\t\t<recruitTime>" + village.army[unit].recruitTime + "</recruitTime>");

                        /*villagesFile.WriteLine("\t\t\t\t<requirements>");
                        for (int i = 0; i < village.army[unit].requirements.Count; i++)
                        {
                            villagesFile.WriteLine("\t\t\t\t\t<requirement>" + village.army[unit].requirements[i] + "</requirement>");
                        }
                        villagesFile.WriteLine("\t\t\t\t<requirements>");*/

                        villagesFile.WriteLine("\t\t\t\t<unitClass>" + village.army[unit].unitClass.ToString() + "</unitClass>");
                        villagesFile.WriteLine("\t\t\t\t<unitType>" + village.army[unit].unitType.ToString() + "</unitType>");

                        villagesFile.WriteLine("\t\t\t\t<upkeepCost>");
                        villagesFile.WriteLine("\t\t\t\t\t<food>" + village.army[unit].upkeepCost.food + "</food>");
                        villagesFile.WriteLine("\t\t\t\t\t<iron>" + village.army[unit].upkeepCost.iron + "</iron>");
                        villagesFile.WriteLine("\t\t\t\t\t<stone>" + village.army[unit].upkeepCost.stone + "</stone>");
                        villagesFile.WriteLine("\t\t\t\t\t<wood>" + village.army[unit].upkeepCost.wood + "</wood>");
                        villagesFile.WriteLine("\t\t\t\t</upkeepCost>");
                        villagesFile.WriteLine("\t\t\t</army" + armyCounter + ">");
                    }
                    villagesFile.WriteLine("\t\t</armies>");

                    this.buildingQueueCounter = 0;
                    villagesFile.WriteLine("\t\t<queues>");
                    foreach (Buildings.BuildingQueueItem queue in village.queues.buildingQueue)
                    {
                        this.buildingQueueCounter++;
                        villagesFile.WriteLine("\t\t\t<buildingQueue" + buildingQueueCounter + ">");
                        villagesFile.WriteLine("\t\t\t\t<name>" + queue.Name + "</name>");
                        villagesFile.WriteLine("\t\t\t\t<Start>" + queue.Start.time + "</Start>");
                        villagesFile.WriteLine("\t\t\t\t<End>" + queue.End.time + "</End>");
                        villagesFile.WriteLine("\t\t\t\t<Extra>" + queue.Extra + "</Extra>");
                        villagesFile.WriteLine("\t\t\t\t<level>" + queue.level + "</level>");

                        villagesFile.WriteLine("\t\t\t\t<toBuild>");
                        villagesFile.WriteLine("\t\t\t\t\t<level>" + queue.toBuild.level + "</level>");
                        villagesFile.WriteLine("\t\t\t\t\t<type>" + queue.toBuild.type.ToString() + "</type>");

                        /*villagesFile.WriteLine("\t\t\t\t\t<requirements>");
                        for (int i = 0; i < queue.toBuild.requirements.Count; i++)
                        {
                            villagesFile.WriteLine("\t\t\t\t\t\t<requirement>" + queue.toBuild.requirements[i].ToString() + "</requirement>");
                        }
                        villagesFile.WriteLine("\t\t\t\t\t</requirements>");*/
                        villagesFile.WriteLine("\t\t\t\t</toBuild>");
                        villagesFile.WriteLine("\t\t\t</buildingQueue" + buildingQueueCounter + ">");
                    }

                    this.queueCounter = 0;
                    foreach (Villages.Queues.QueueItem queue in village.queues.queue)
                    {
                        this.queueCounter++;
                        villagesFile.WriteLine("\t\t\t<queue" + queueCounter + ">");
                        villagesFile.WriteLine("\t\t\t\t<name>" + queue.Name + "</name>");
                        villagesFile.WriteLine("\t\t\t\t<Start>" + queue.Start.time + "</Start>");
                        villagesFile.WriteLine("\t\t\t\t<End>" + queue.End.time + "</End>");
                        villagesFile.WriteLine("\t\t\t\t<Extra>" + queue.Extra + "</Extra>");
                        villagesFile.WriteLine("\t\t\t</queue" + queueCounter + ">");
                    }
                    villagesFile.WriteLine("\t\t</queues>");

                    villagesFile.WriteLine("\t\t<recruitTimeEnd>" + village.recruitTimeEnd.time + "</recruitTimeEnd>");
                    //villagesFile.WriteLine("\t\t<researchTimeEnd>" + village.researchTimeEnd.time + "</researchTimeEnd>"); ERROR: Object reference not set to an instance of an object.

                    villagesFile.WriteLine("\t</village" + villageCounter + ">");
                }
                villagesFile.WriteLine("</villages>");
                villagesFile.Close();

                playerCounter++;
            }
        }

        public void saveMap(Maps.Map map)
        {
            string mapPath = this.path + "map\\";
            Directory.CreateDirectory(mapPath);

            StreamWriter mapFile = new StreamWriter(mapPath + "map.xml");
            Plevian.Maps.Tile[,] tiles = map.getMap();

            mapFile.WriteLine("<map>");

            mapFile.WriteLine("\t<size>");
            mapFile.WriteLine("\t\t<x>" + map.sizeX + "</x>");
            mapFile.WriteLine("\t\t<y>" + map.sizeY + "</y>");
            mapFile.WriteLine("\t</size>");

            mapFile.WriteLine("\t<tiles>");
            this.tilesCounter = 0;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    this.tilesCounter++;
                    mapFile.WriteLine("\t\t<tile" + tilesCounter + ">");
                    mapFile.WriteLine("\t\t\t<index>" + i + " " + j + "</index>");
                    mapFile.WriteLine("\t\t\t<location>");
                    mapFile.WriteLine("\t\t\t\t<x>" + tiles[i, j].location.x + "</x>");
                    mapFile.WriteLine("\t\t\t\t<y>" + tiles[i, j].location.y + "</y>");
                    mapFile.WriteLine("\t\t\t</location>");
                    mapFile.WriteLine("\t\t\t<type>" + tiles[i, j].type + "</type>");
                    mapFile.WriteLine("\t\t</tile" + tilesCounter + ">");
                }
            }
            mapFile.WriteLine("\t</tiles>");

            mapFile.WriteLine("</map>");
            mapFile.Close();
        }

        public String getMapFile()
        {
            return path + "map.txt";
        }

        private String getGameTimeFile()
        {
            return path + "time.txt";
        }

        public GameTime getGameTime()
        {
            FileStream fs = new FileStream(getGameTimeFile(), FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4]; // ulong is 64 bits = 8 bytes
            fs.Read(buffer, 0, 4);
            int gameTime = BitConverter.ToInt32(buffer, 0);
            fs.Close();
            return new Seconds(gameTime);
        }
    }
}
