using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Plevian.Players;
using Plevian.TechnologY;
using Plevian.Units;
using Plevian.Maps;

// TODO: Save writer/reader
// - Orders handling

namespace Plevian.Save
{
    public class SaveWriter
    {
        private readonly String path;

        public SaveWriter(String path)
        {
            //Directory.CreateDirectory("Save\\");
            //this.path = "Save\\" + path + "\\";
            //if (!Directory.Exists(path))
            this.path = path + "\\";
            Directory.CreateDirectory(this.path);
        }

        private void savePlayers(List<Player> players)
        {
            int messageCounter;
            int technologyCounter;
            int villageCounter;
            int buildingCounter;
            int armyCounter;
            int buildingQueueCounter;
            int researchQueueCounter;
            int queueCounter;
            int recruitQueueCounter;

            string playersPath = this.path + "players\\";
            string playersBasicInfo = playersPath + "basicInfo\\";
            string playersMessages = playersPath + "messages\\";
            string playersTechnologies = playersPath + "technologies\\";
            string playersVillages = playersPath + "villages\\";
            string playersCounters = playersPath + "counters\\";

            Directory.CreateDirectory(playersPath);
            Directory.CreateDirectory(playersBasicInfo);
            Directory.CreateDirectory(playersMessages);
            Directory.CreateDirectory(playersTechnologies);
            Directory.CreateDirectory(playersVillages);
            Directory.CreateDirectory(playersCounters);

            int playerCounter = 1;
            foreach (Player player in players)
            {
                StreamWriter counterFile = new StreamWriter(playersCounters + "counter" + playerCounter.ToString() + ".xml");
                counterFile.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-2\" standalone=\"no\" ?>");
                counterFile.WriteLine("<counters>");

                StreamWriter basicInfoFile = new StreamWriter(playersBasicInfo + "player" + playerCounter.ToString() + ".xml");
                basicInfoFile.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-2\" standalone=\"no\" ?>");

                basicInfoFile.WriteLine("<basicInfo>");
                basicInfoFile.WriteLine("\t<name>" + player.name + "</name>");

                if (player.GetType().ToString() == "Plevian.Players.ComputerPlayer")
                    basicInfoFile.WriteLine("\t<computer>true</computer>");
                else
                    basicInfoFile.WriteLine("\t<computer>false</computer>");

                basicInfoFile.WriteLine("\t<color>");
                basicInfoFile.WriteLine("\t\t<A>" + player.color.A + "</A>");
                basicInfoFile.WriteLine("\t\t<R>" + player.color.R + "</R>");
                basicInfoFile.WriteLine("\t\t<G>" + player.color.G + "</G>");
                basicInfoFile.WriteLine("\t\t<B>" + player.color.B + "</B>");
                basicInfoFile.WriteLine("\t</color>");

                basicInfoFile.WriteLine("</basicInfo>");
                basicInfoFile.Close();

                messageCounter = 0;
                StreamWriter messagesFile = new StreamWriter(playersMessages + "player" + playerCounter.ToString() + ".xml");
                messagesFile.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-2\" standalone=\"no\" ?>");
                messagesFile.WriteLine("<messages>");
                foreach (Messages.Message message in player.messages)
                {
                    messageCounter++;
                    messagesFile.WriteLine("\t<message" + messageCounter + ">");
                    messagesFile.WriteLine("\t\t<date>" + message.Date.Content + "</date>");
                    messagesFile.WriteLine("\t\t<sender>" + message.Sender.Content + "</sender>");
                    messagesFile.WriteLine("\t\t<topic>" + message.Topic.Content + "</topic>");
                    messagesFile.WriteLine("\t\t<text>" + message.message + "</text>");
                    messagesFile.WriteLine("\t</message" + messageCounter + ">");
                }
                messagesFile.WriteLine("</messages>");
                messagesFile.Close();
                counterFile.WriteLine("\t<messageCounter>" + messageCounter + "</messageCounter>");

                technologyCounter = 0;
                StreamWriter techFile = new StreamWriter(playersTechnologies + "player" + playerCounter.ToString() + ".xml");
                techFile.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-2\" standalone=\"no\" ?>");
                techFile.WriteLine("<technologies>");
                foreach(Technology tech in player.technologies.technologies)
                {
                    technologyCounter++;
                    techFile.WriteLine("\t<technology" + technologyCounter + ">");

                    techFile.WriteLine("\t\t<name>" + tech.Name + "</name>");

                    techFile.WriteLine("\t\t<researched>" + tech.researched + "</researched>");

                    techFile.WriteLine("\t</technology" + technologyCounter + ">");
                }
                techFile.WriteLine("</technologies>");
                techFile.Close();
                counterFile.WriteLine("\t<technologyCounter>" + technologyCounter + "</technologyCounter>");

                villageCounter = 0;
                StreamWriter villagesFile = new StreamWriter(playersVillages + "player" + playerCounter.ToString() + ".xml");
                villagesFile.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-2\" standalone=\"no\" ?>");
                villagesFile.WriteLine("<villages>");
                foreach (Villages.Village village in player.villages)
                {
                    villageCounter++;

                    counterFile.WriteLine("\t<village" + villageCounter + ">");

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

                    buildingCounter = 0;
                    villagesFile.WriteLine("\t\t<buildings>");
                    foreach (KeyValuePair<Buildings.BuildingType, Buildings.Building> building in village.buildings)
                    {
                        buildingCounter++;

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
                    counterFile.WriteLine("\t\t<buildingCounter>" + buildingCounter + "</buildingCounter>");
                    villagesFile.WriteLine("\t\t</buildings>");

                    villagesFile.WriteLine("\t\t<builtTimeEnd>" + village.buildTimeEnd.time + "</builtTimeEnd>");

                    armyCounter = 0;
                    villagesFile.WriteLine("\t\t<armies>");
                    var unitType = Enum.GetValues(typeof(UnitType));
                    foreach (UnitType unit in unitType)
                    {
                        armyCounter++;
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
                    counterFile.WriteLine("\t\t<armyCounter>" + armyCounter + "</armyCounter>");
                    villagesFile.WriteLine("\t\t</armies>");
                    
                    buildingQueueCounter = 0;
                    villagesFile.WriteLine("\t\t<queues>");
                    foreach (Buildings.BuildingQueueItem buildingQueue in village.queues.buildingQueue)
                    {
                        buildingQueueCounter++;
                        villagesFile.WriteLine("\t\t\t<buildingQueue" + buildingQueueCounter + ">");
                        villagesFile.WriteLine("\t\t\t\t<name>" + buildingQueue.Name + "</name>");
                        villagesFile.WriteLine("\t\t\t\t<Start>" + buildingQueue.Start.time + "</Start>");
                        villagesFile.WriteLine("\t\t\t\t<End>" + buildingQueue.End.time + "</End>");
                        villagesFile.WriteLine("\t\t\t\t<Extra>" + buildingQueue.Extra + "</Extra>");
                        villagesFile.WriteLine("\t\t\t\t<level>" + buildingQueue.level + "</level>");

                        villagesFile.WriteLine("\t\t\t\t<toBuild>");
                        villagesFile.WriteLine("\t\t\t\t\t<level>" + buildingQueue.toBuild.level + "</level>");
                        villagesFile.WriteLine("\t\t\t\t\t<type>" + buildingQueue.toBuild.type.ToString() + "</type>");

                        /*villagesFile.WriteLine("\t\t\t\t\t<requirements>");
                        for (int i = 0; i < queue.toBuild.requirements.Count; i++)
                        {
                            villagesFile.WriteLine("\t\t\t\t\t\t<requirement>" + queue.toBuild.requirements[i].ToString() + "</requirement>");
                        }
                        villagesFile.WriteLine("\t\t\t\t\t</requirements>");*/
                        villagesFile.WriteLine("\t\t\t\t</toBuild>");
                        villagesFile.WriteLine("\t\t\t</buildingQueue" + buildingQueueCounter + ">");
                    }
                    counterFile.WriteLine("\t\t<buildingQueueCounter>" + buildingQueueCounter + "</buildingQueueCounter>");

                    researchQueueCounter = 0;
                    foreach (ResearchQueueItem researchQueue in village.queues.researchQueue)
                    {
                        researchQueueCounter++;
                        villagesFile.WriteLine("\t\t\t<researchQueue" + researchQueueCounter + ">");
                        villagesFile.WriteLine("\t\t\t\t<name>" + researchQueue.Name + "</name>");
                        villagesFile.WriteLine("\t\t\t\t<Start>" + researchQueue.Start.time + "</Start>");
                        villagesFile.WriteLine("\t\t\t\t<End>" + researchQueue.End.time + "</End>");
                        villagesFile.WriteLine("\t\t\t\t<Extra>" + researchQueue.Extra + "</Extra>");
                        villagesFile.WriteLine("\t\t\t\t<researched>" + researchQueue.researched.Name + "</researched>");
                        villagesFile.WriteLine("\t\t\t</researchQueue" + researchQueueCounter + ">");
                    }
                    counterFile.WriteLine("\t\t<researchQueueCounter>" + researchQueueCounter + "</researchQueueCounter>");

                    recruitQueueCounter = 0;
                    foreach (RecruitQueueItem recruitQueue in village.queues.recruitQueue)
                    {
                        recruitQueueCounter++;
                        villagesFile.WriteLine("\t\t\t<recruitQueue" + recruitQueueCounter + ">");
                        villagesFile.WriteLine("\t\t\t\t<name>" + recruitQueue.Name + "</name>");
                        villagesFile.WriteLine("\t\t\t\t<Start>" + recruitQueue.Start.time + "</Start>");
                        villagesFile.WriteLine("\t\t\t\t<End>" + recruitQueue.End.time + "</End>");
                        villagesFile.WriteLine("\t\t\t\t<Extra>" + recruitQueue.Extra + "</Extra>");
                        villagesFile.WriteLine("\t\t\t\t<toRecruit>" + recruitQueue.toRecruit.name + "</toRecruit>");
                        villagesFile.WriteLine("\t\t\t</recruitQueue" + recruitQueueCounter + ">");
                    }
                    counterFile.WriteLine("\t\t<recruitQueueCounter>" + recruitQueueCounter + "</recruitQueueCounter>");

                    queueCounter = 0;
                    foreach (Villages.Queues.QueueItem queue in village.queues.queue)
                    {
                        queueCounter++;
                        villagesFile.WriteLine("\t\t\t<queue" + queueCounter + ">");
                        villagesFile.WriteLine("\t\t\t\t<name>" + queue.Name + "</name>");
                        villagesFile.WriteLine("\t\t\t\t<Start>" + queue.Start.time + "</Start>");
                        villagesFile.WriteLine("\t\t\t\t<End>" + queue.End.time + "</End>");
                        villagesFile.WriteLine("\t\t\t\t<Extra>" + queue.Extra + "</Extra>");
                        villagesFile.WriteLine("\t\t\t</queue" + queueCounter + ">");
                    }
                    counterFile.WriteLine("\t\t<queueCounter>" + queueCounter + "</queueCounter>");
                    villagesFile.WriteLine("\t\t</queues>");

                    villagesFile.WriteLine("\t\t<recruitTimeEnd>" + village.recruitTimeEnd.time + "</recruitTimeEnd>");

                    villagesFile.WriteLine("\t\t<researchTimeEnd>" + village.researchTimeEnd.time + "</researchTimeEnd>");

                    villagesFile.WriteLine("\t</village" + villageCounter + ">");

                    counterFile.WriteLine("\t</village" + villageCounter + ">"); 
                }
                villagesFile.WriteLine("</villages>");
                villagesFile.Close();
                
                counterFile.WriteLine("\t<villageCounter>" + villageCounter + "</villageCounter>");
                counterFile.WriteLine("</counters>");
                counterFile.Close();

                playerCounter++;
            }
        }

        private void saveMap(Map map)
        {
            int tilesCounter;

            string mapPath = this.path + "map\\";
            Directory.CreateDirectory(mapPath);

            StreamWriter mapFile = new StreamWriter(mapPath + "map.xml");
            Tile[,] tiles = map.getMap();

            mapFile.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-2\" standalone=\"no\" ?>");
            mapFile.WriteLine("<map>");

            mapFile.WriteLine("\t<size>");
            mapFile.WriteLine("\t\t<x>" + map.sizeX + "</x>");
            mapFile.WriteLine("\t\t<y>" + map.sizeY + "</y>");
            mapFile.WriteLine("\t</size>");

            mapFile.WriteLine("\t<tiles>");
            tilesCounter = 0;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tilesCounter++;
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

            mapFile.WriteLine("\t<time>" + GameTime.now + "</time>");
            mapFile.WriteLine("</map>");
            mapFile.Close();
        }

        public void writeSave(Map map, List<Player> players)
        {
            this.saveMap(map);
            this.savePlayers(players);
        }
    }
}
