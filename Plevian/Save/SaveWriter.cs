using System;
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
                StreamWriter basicInfo = new StreamWriter(playersBasicInfo + "player" + playerCounter.ToString() + ".txt");
                basicInfo.WriteLine("<name>" + player.name + "</name>");
                basicInfo.WriteLine("<color>" + player.color.ToString() + "</color>");
                basicInfo.Close();

                StreamWriter messagesFile = new StreamWriter(playersMessages + "player" + playerCounter.ToString() + ".txt");
                foreach (Messages.Message message in player.messages)
                {
                    messagesFile.WriteLine("<---message-start--->");
                    messagesFile.WriteLine("\t<date>" + message.Date + "</date>");
                    messagesFile.WriteLine("\t<sender>" + message.Sender + "</sender>");
                    messagesFile.WriteLine("\t<topic>" + message.Topic + "</topic>");
                    messagesFile.WriteLine("\t<message>" + message.message + "</message>");
                    messagesFile.WriteLine("<---message-stop--->");
                }
                messagesFile.Close();

                StreamWriter techFile = new StreamWriter(playersTechnologies + "player" + playerCounter.ToString() + ".txt");
                foreach(TechnologY.Technology tech in player.technologies.technologies)
                {
                    techFile.WriteLine("<---technology-start--->");

                    techFile.WriteLine("\t<name>" + tech.Name + "</name>");

                    techFile.WriteLine("\t<price>");
                    techFile.WriteLine("\t\t<food>" + tech.Price.food + "</food>");
                    techFile.WriteLine("\t\t<iron>" + tech.Price.iron + "</iron>");
                    techFile.WriteLine("\t\t<stone>" + tech.Price.stone + "</stone>");
                    techFile.WriteLine("\t\t<wood>" + tech.Price.wood + "</wood>");
                    techFile.WriteLine("\t</price>");

                    techFile.WriteLine("\t<requirements>");
                    foreach (RequirementS.Requirement requirement in tech.Requirements.RequirementsList)
                    {
                        techFile.WriteLine("\t\t" + requirement.ToString());
                    }
                    techFile.WriteLine("\t</requirements>");

                    techFile.WriteLine("\t<research>");
                    techFile.WriteLine("\t\t<researched>" + tech.researched.ToString() + "</researched>");
                    techFile.WriteLine("\t\t<time>" + tech.ResearchTime.time + "</time>");
                    techFile.WriteLine("\t<research>");

                    techFile.WriteLine("<---technology-stop--->");
                }
                techFile.Close();

                StreamWriter villagesFile = new StreamWriter(playersVillages + "player" + playerCounter.ToString() + ".txt");
                foreach (Villages.Village village in player.villages)
                {
                    villagesFile.WriteLine("<---village-start--->");

                    if (player.Capital.name == village.name)
                        villagesFile.WriteLine("<!--capital--!>");

                    villagesFile.WriteLine("\t<name>" + village.name + "</name>");
                    villagesFile.WriteLine("\t<type>" + village.type + "</type>");

                    villagesFile.WriteLine("\t<location>");
                    villagesFile.WriteLine("\t\t<x>" + village.location.x + "</x>");
                    villagesFile.WriteLine("\t\t<y>" + village.location.y + "</y>");
                    villagesFile.WriteLine("\t</location>");

                    villagesFile.WriteLine("\t<owner>" + village.Owner.name + "</owner>");

                    villagesFile.WriteLine("\t<resources>");
                    villagesFile.WriteLine("\t\t<food>" + village.resources.food + "</food>");
                    villagesFile.WriteLine("\t\t<iron>" + village.resources.iron + "</iron>");
                    villagesFile.WriteLine("\t\t<stone>" + village.resources.stone + "</stone>");
                    villagesFile.WriteLine("\t\t<wood>" + village.resources.wood + "</wood>");
                    villagesFile.WriteLine("\t</resources>");

                    villagesFile.WriteLine("\t<buildings>");
                    foreach (KeyValuePair<Buildings.BuildingType, Buildings.Building> building in village.buildings)
                    {
                        villagesFile.WriteLine("\t\t<building>");
                        villagesFile.WriteLine("\t\t\t<key>" + building.Key + "</key>");
                        villagesFile.WriteLine("\t\t\t<level>" + building.Value.level + "</level>");
                        
                        villagesFile.WriteLine("\t\t\t<requirements>");
                        foreach (RequirementS.Requirement requirement in building.Value.requirements)
                        {
                            villagesFile.WriteLine("\t\t\t\t<requirement>" + requirement.ToString() + "</requirement>");
                        }
                        villagesFile.WriteLine("\t\t\t</requirements>");

                        villagesFile.WriteLine("\t\t\t<type>" + building.Value.type.ToString() + "</type>");
                        villagesFile.WriteLine("\t\t</building>");
                    }
                    villagesFile.WriteLine("\t</buildings>");

                    villagesFile.WriteLine("\t<builtTimeEnd>" + village.buildTimeEnd.time + "</builtTimeEnd>");

                    villagesFile.WriteLine("\t<armies>");
                    var unitType = Enum.GetValues(typeof(Units.UnitType));
                    foreach (Units.UnitType unit in unitType)
                    {
                        villagesFile.WriteLine("\t\t<army>");
                        villagesFile.WriteLine("\t\t\t<attackStrength>" + village.army[unit].attackStrength + "</attackStrength>");
                        villagesFile.WriteLine("\t\t\t<defenseArchers>" + village.army[unit].defenseArchers + "</defenseArchers>");
                        villagesFile.WriteLine("\t\t\t<defenseCavalry>" + village.army[unit].defenseCavalry + "</defenseCavalry>");
                        villagesFile.WriteLine("\t\t\t<defenseInfantry>" + village.army[unit].defenseInfantry + "</defenseInfantry>");

                        /*villagesFile.WriteLine("\t\t\t<DependencyObjectType>");
                        villagesFile.WriteLine("\t\t\t\t<BaseType>" + village.army[unit].DependencyObjectType.BaseType + "</BaseType>");
                        villagesFile.WriteLine("\t\t\t\t<Id>" + village.army[unit].DependencyObjectType.Id + "</Id>");
                        villagesFile.WriteLine("\t\t\t\t<Name>" + village.army[unit].DependencyObjectType.Name + "</Name>");
                        villagesFile.WriteLine("\t\t\t\t<SystemType>" + village.army[unit].DependencyObjectType.SystemType + "</SystemType>");
                        villagesFile.WriteLine("\t\t\t</DependencyObjectType>");*/
                        
                        villagesFile.WriteLine("\t\t\t<IsSealed>" + village.army[unit].IsSealed + "</IsSealed>");
                        villagesFile.WriteLine("\t\t\t<lootCapacity>" + village.army[unit].lootCapacity + "</lootCapacity>");
                        villagesFile.WriteLine("\t\t\t<movementSpeed>" + village.army[unit].movementSpeed + "</movementSpeed>");
                        villagesFile.WriteLine("\t\t\t<name>" + village.army[unit].name + "</name>");
                        villagesFile.WriteLine("\t\t\t<quantity>" + village.army[unit].quantity + "</quantity>");

                        villagesFile.WriteLine("\t\t\t<recruitCost>");
                        villagesFile.WriteLine("\t\t\t\t<food>" + village.army[unit].recruitCost.food + "</food>");
                        villagesFile.WriteLine("\t\t\t\t<iron>" + village.army[unit].recruitCost.iron + "</iron>");
                        villagesFile.WriteLine("\t\t\t\t<stone>" + village.army[unit].recruitCost.stone + "</stone>");
                        villagesFile.WriteLine("\t\t\t\t<wood>" + village.army[unit].recruitCost.wood + "</wood>");
                        villagesFile.WriteLine("\t\t\t</recruitCost>");

                        villagesFile.WriteLine("\t\t\t<recruitTime>" + village.army[unit].recruitTime + "</recruitTime>");

                        villagesFile.WriteLine("\t\t\t<requirements>");
                        for (int i = 0; i < village.army[unit].requirements.Count; i++)
                        {
                            villagesFile.WriteLine("\t\t\t\t<requirement>" + village.army[unit].requirements[i] + "</requirement>");
                        }
                        villagesFile.WriteLine("\t\t\t<requirements>");

                        villagesFile.WriteLine("\t\t\t<unitClass>" + village.army[unit].unitClass.ToString() + "</unitClass>");
                        villagesFile.WriteLine("\t\t\t<unitType>" + village.army[unit].unitType.ToString() + "</unitType>");

                        villagesFile.WriteLine("\t\t\t<upkeepCost>");
                        villagesFile.WriteLine("\t\t\t\t<food>" + village.army[unit].upkeepCost.food + "</food>");
                        villagesFile.WriteLine("\t\t\t\t<iron>" + village.army[unit].upkeepCost.iron + "</iron>");
                        villagesFile.WriteLine("\t\t\t\t<stone>" + village.army[unit].upkeepCost.stone + "</stone>");
                        villagesFile.WriteLine("\t\t\t\t<wood>" + village.army[unit].upkeepCost.wood + "</wood>");
                        villagesFile.WriteLine("\t\t\t</upkeepCost>");
                        villagesFile.WriteLine("\t\t</army>");
                    }
                    villagesFile.WriteLine("\t</armies>");

                    villagesFile.WriteLine("\t<queues>");
                    villagesFile.WriteLine("\t\t<buildingQueue>");
                    foreach (Buildings.BuildingQueueItem queue in village.queues.buildingQueue)
                    {
                        villagesFile.WriteLine("\t\t\t<name>" + queue.Name + "</name>");
                        villagesFile.WriteLine("\t\t\t<Start>" + queue.Start.time + "</Start>");
                        villagesFile.WriteLine("\t\t\t<End>" + queue.End.time + "</End>");
                        villagesFile.WriteLine("\t\t\t<Extra>" + queue.Extra + "</Extra>");
                        villagesFile.WriteLine("\t\t\t<level>" + queue.level + "</level>");

                        villagesFile.WriteLine("\t\t\t<toBuild>");
                        villagesFile.WriteLine("\t\t\t\t<level>" + queue.toBuild.level + "</level>");
                        villagesFile.WriteLine("\t\t\t\t<type>" + queue.toBuild.type.ToString() + "</type>");

                        villagesFile.WriteLine("\t\t\t\t<requirements>");
                        for (int i = 0; i < queue.toBuild.requirements.Count; i++)
                        {
                            villagesFile.WriteLine("\t\t\t\t\t<requirement>" + queue.toBuild.requirements[i].ToString() + "</requirement>");
                        }
                        villagesFile.WriteLine("\t\t\t\t</requirements>");
                        villagesFile.WriteLine("\t\t\t</toBuild>");
                    }
                    villagesFile.WriteLine("\t\t</buildingQueue>");

                    villagesFile.WriteLine("\t\t<queue>");
                    foreach (Villages.Queues.QueueItem queue in village.queues.queue)
                    {
                        villagesFile.WriteLine("\t\t\t<name>" + queue.Name + "</name>");
                        villagesFile.WriteLine("\t\t\t<Start>" + queue.Start.time + "</Start>");
                        villagesFile.WriteLine("\t\t\t<End>" + queue.End.time + "</End>");
                        villagesFile.WriteLine("\t\t\t<Extra>" + queue.Extra + "</Extra>");
                    }
                    villagesFile.WriteLine("\t\t</queue>");
                    villagesFile.WriteLine("\t</queues>");

                    villagesFile.WriteLine("\t<recruitTimeEnd>" + village.recruitTimeEnd.time + "</recruitTimeEnd>");
                    //villagesFile.WriteLine("\t<researchTimeEnd>" + village.researchTimeEnd.time + "</researchTimeEnd>"); ERROR: Object reference not set to an instance of an object.

                    villagesFile.WriteLine("<---village-stop--->");
                }
                villagesFile.Close();

                playerCounter++;
            }
        }

        public void saveMap(Maps.Map map)
        {
            string mapPath = this.path + "map\\";
            Directory.CreateDirectory(mapPath);

            StreamWriter mapFile = new StreamWriter(mapPath + "map.txt");
            Plevian.Maps.Tile[,] tiles = map.getMap();

            mapFile.WriteLine("<---map-start--->");

            mapFile.WriteLine("\t<size>");
            mapFile.WriteLine("\t\t<x>" + map.sizeX + "</x>");
            mapFile.WriteLine("\t\t<y>" + map.sizeY + "</y>");
            mapFile.WriteLine("\t</size>");

            mapFile.WriteLine("\t<tiles>");
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    mapFile.WriteLine("\t\t<tile>");
                    mapFile.WriteLine("\t\t\t<index>" + i + " " + j + "</index>");
                    mapFile.WriteLine("\t\t\t<location>");
                    mapFile.WriteLine("\t\t\t\t<x>" + tiles[i, j].location.x + "</x>");
                    mapFile.WriteLine("\t\t\t\t<y>" + tiles[i, j].location.y + "</y>");
                    mapFile.WriteLine("\t\t\t</location>");
                    mapFile.WriteLine("\t\t\t<type>" + tiles[i, j].type + "</type>");
                    mapFile.WriteLine("\t\t</tile>");
                }
            }
            mapFile.WriteLine("\t</tiles>");

            mapFile.WriteLine("<---map-stop--->");
            mapFile.Close();
        }

        public SaveWriter(String path)
        {
            this.path = path + "\\";
            //if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
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
