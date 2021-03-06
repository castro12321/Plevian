﻿using System;
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
using System.Xml;
using Plevian.Villages;
using Plevian.Orders;

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
            int armyCounter;
            int buildingQueueCounter;
            int researchQueueCounter;
            int queueCounter;
            int recruitQueueCounter;
            int orderCounter;

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
                XmlDocument countersXml = new XmlDocument();
                XmlDeclaration countersDeclaration = countersXml.CreateXmlDeclaration("1.0", "UTF-8", null);
                countersXml.AppendChild(countersDeclaration);

                XmlNode countersRoot = countersXml.CreateElement("counters");
                countersXml.AppendChild(countersRoot);

                #region basicInfoXml

                XmlDocument basicInfoXml = new XmlDocument();
                XmlDeclaration basicInfoDeclaration = basicInfoXml.CreateXmlDeclaration("1.0", "UTF-8", null);
                basicInfoXml.AppendChild(basicInfoDeclaration);

                XmlNode basicInfoRoot = basicInfoXml.CreateElement("basicInfo");
                basicInfoXml.AppendChild(basicInfoRoot);

                XmlNode playerName = basicInfoXml.CreateElement("name");
                playerName.AppendChild(basicInfoXml.CreateTextNode(player.name));
                basicInfoRoot.AppendChild(playerName);

                XmlNode computer = basicInfoXml.CreateElement("computer");
                if (player.GetType().ToString() == "Plevian.Players.ComputerPlayer")
                    computer.AppendChild(basicInfoXml.CreateTextNode("true"));
                else
                    computer.AppendChild(basicInfoXml.CreateTextNode("false"));
                basicInfoRoot.AppendChild(computer);

                XmlNode playerColor = basicInfoXml.CreateElement("color");
                XmlNode A = basicInfoXml.CreateElement("A");
                A.AppendChild(basicInfoXml.CreateTextNode(player.color.A.ToString()));
                playerColor.AppendChild(A);

                XmlNode R = basicInfoXml.CreateElement("R");
                R.AppendChild(basicInfoXml.CreateTextNode(player.color.R.ToString()));
                playerColor.AppendChild(R);

                XmlNode G = basicInfoXml.CreateElement("G");
                G.AppendChild(basicInfoXml.CreateTextNode(player.color.G.ToString()));
                playerColor.AppendChild(G);

                XmlNode B = basicInfoXml.CreateElement("B");
                B.AppendChild(basicInfoXml.CreateTextNode(player.color.B.ToString()));
                playerColor.AppendChild(B);
                basicInfoRoot.AppendChild(playerColor);

                basicInfoXml.Save(playersBasicInfo + "player" + playerCounter.ToString() + ".xml");

                #endregion
                
                #region message
                
                XmlDocument messagesXml = new XmlDocument();
                XmlDeclaration messagesDeclaration = messagesXml.CreateXmlDeclaration("1.0", "UTF-8", null);
                messagesXml.AppendChild(messagesDeclaration);

                XmlNode messagesRoot = messagesXml.CreateElement("messages");
                messagesXml.AppendChild(messagesRoot);

                messageCounter = 0;
                foreach (Messages.Message message in player.messages)
                {
                    messageCounter++;
                    XmlNode messageNode = messagesXml.CreateElement("message" + messageCounter);
                    XmlNode messageDate = messagesXml.CreateElement("date");
                    messageDate.AppendChild(messagesXml.CreateTextNode(message.Date.Content.ToString()));
                    messageNode.AppendChild(messageDate);

                    XmlNode messageSender = messagesXml.CreateElement("sender");
                    messageSender.AppendChild(messagesXml.CreateTextNode(message.Sender.Content.ToString()));
                    messageNode.AppendChild(messageSender);

                    XmlNode messageTopic = messagesXml.CreateElement("topic");
                    messageTopic.AppendChild(messagesXml.CreateTextNode(message.Topic.Content.ToString()));
                    messageNode.AppendChild(messageTopic);

                    XmlNode messageText = messagesXml.CreateElement("text");
                    messageText.AppendChild(messagesXml.CreateTextNode(message.message));
                    messageNode.AppendChild(messageText);
                    messagesRoot.AppendChild(messageNode);
                }

                messagesXml.Save(playersMessages + "player" + playerCounter.ToString() + ".xml");

                #endregion

                XmlNode messageCount = countersXml.CreateElement("messageCounter");
                messageCount.AppendChild(countersXml.CreateTextNode(messageCounter.ToString()));
                countersRoot.AppendChild(messageCount);

                #region technology

                XmlDocument technologiesXml = new XmlDocument();
                XmlDeclaration technologiesDeclaration = technologiesXml.CreateXmlDeclaration("1.0", "UTF-8", null);
                technologiesXml.AppendChild(technologiesDeclaration);

                XmlNode technologiesRoot = technologiesXml.CreateElement("technologies");
                technologiesXml.AppendChild(technologiesRoot);

                technologyCounter = 0;
                foreach (Technology tech in player.technologies.technologies)
                {
                    technologyCounter++;
                    XmlNode technologyNode = technologiesXml.CreateElement("technology" + technologyCounter);
                    XmlNode technologyName = technologiesXml.CreateElement("name");
                    technologyName.AppendChild(technologiesXml.CreateTextNode(tech.Name));
                    technologyNode.AppendChild(technologyName);

                    XmlNode researched = technologiesXml.CreateElement("researched");
                    researched.AppendChild(technologiesXml.CreateTextNode(tech.researched.ToString()));
                    technologyNode.AppendChild(researched);
                    technologiesRoot.AppendChild(technologyNode);
                }

                technologiesXml.Save(playersTechnologies + "player" + playerCounter.ToString() + ".xml");

                #endregion

                XmlNode technologyCount = countersXml.CreateElement("technologyCounter");
                technologyCount.AppendChild(countersXml.CreateTextNode(technologyCounter.ToString()));
                countersRoot.AppendChild(technologyCount);

                #region village

                XmlDocument villagesXml = new XmlDocument();
                XmlDeclaration villagesDeclaration = villagesXml.CreateXmlDeclaration("1.0", "UTF-8", null);
                villagesXml.AppendChild(villagesDeclaration);

                XmlNode villagesRoot = villagesXml.CreateElement("villages");
                villagesXml.AppendChild(villagesRoot);

                villageCounter = 0;
                foreach (Village village in player.villages)
                {
                    villageCounter++;

                    XmlNode villageCounters = countersXml.CreateElement("village" + villageCounter);
                    countersRoot.AppendChild(villageCounters);

                    XmlNode villageNode = villagesXml.CreateElement("village" + villageCounter);
                    XmlNode villageName = villagesXml.CreateElement("name");
                    villageName.AppendChild(villagesXml.CreateTextNode(village.name));
                    villageNode.AppendChild(villageName);

                    XmlNode villageType = villagesXml.CreateElement("type");
                    villageType.AppendChild(villagesXml.CreateTextNode(village.type.ToString()));
                    villageNode.AppendChild(villageType);

                    XmlNode capital = villagesXml.CreateElement("capital");
                    if (player.Capital.name == village.name)
                        capital.AppendChild(villagesXml.CreateTextNode("true"));
                    else
                        capital.AppendChild(villagesXml.CreateTextNode("false"));
                    villageNode.AppendChild(capital);

                    XmlNode VilageLocation = villagesXml.CreateElement("location");
                    XmlNode x = villagesXml.CreateElement("x");
                    x.AppendChild(villagesXml.CreateTextNode(village.location.x.ToString()));
                    XmlNode y = villagesXml.CreateElement("y");
                    y.AppendChild(villagesXml.CreateTextNode(village.location.y.ToString()));
                    VilageLocation.AppendChild(x);
                    VilageLocation.AppendChild(y);
                    villageNode.AppendChild(VilageLocation);

                    XmlNode villageOwner = villagesXml.CreateElement("owner");
                    villageOwner.AppendChild(villagesXml.CreateTextNode(village.Owner.name));
                    villageNode.AppendChild(villageOwner);

                    XmlNode villageResources = villagesXml.CreateElement("resources");
                    XmlNode food = villagesXml.CreateElement("food");
                    food.AppendChild(villagesXml.CreateTextNode(village.resources.food.ToString()));
                    villageResources.AppendChild(food);

                    XmlNode iron = villagesXml.CreateElement("iron");
                    iron.AppendChild(villagesXml.CreateTextNode(village.resources.iron.ToString()));
                    villageResources.AppendChild(iron);

                    XmlNode stone = villagesXml.CreateElement("stone");
                    stone.AppendChild(villagesXml.CreateTextNode(village.resources.stone.ToString()));
                    villageResources.AppendChild(stone);

                    XmlNode wood = villagesXml.CreateElement("wood");
                    wood.AppendChild(villagesXml.CreateTextNode(village.resources.wood.ToString()));
                    villageResources.AppendChild(wood);
                    villageNode.AppendChild(villageResources);

                    XmlNode buildings = villagesXml.CreateElement("buildings");
                    foreach (KeyValuePair<Buildings.BuildingType, Buildings.Building> build in village.buildings)
                    {
                        XmlNode building = villagesXml.CreateElement(build.Key.ToString());

                        XmlNode level = villagesXml.CreateElement("level");
                        level.AppendChild(villagesXml.CreateTextNode(build.Value.level.ToString()));
                        building.AppendChild(level);

                        buildings.AppendChild(building);
                    }
                    villageNode.AppendChild(buildings);

                    XmlNode buildTimeEnd = villagesXml.CreateElement("buildTimeEnd");
                    buildTimeEnd.AppendChild(villagesXml.CreateTextNode(village.buildTimeEnd.time.ToString()));
                    villageNode.AppendChild(buildTimeEnd);

                    XmlNode armies = villagesXml.CreateElement("armies");
                    var unitType = Enum.GetValues(typeof(UnitType));
                    armyCounter = 0;
                    foreach (UnitType unit in unitType)
                    {
                        armyCounter++;

                        XmlNode army = villagesXml.CreateElement(unit.ToString());

                        XmlNode armyName = villagesXml.CreateElement("name");
                        armyName.AppendChild(villagesXml.CreateTextNode(village.army[unit].name));
                        army.AppendChild(armyName);

                        XmlNode armyQuantity = villagesXml.CreateElement("quantity");
                        armyQuantity.AppendChild(villagesXml.CreateTextNode(village.army[unit].quantity.ToString()));
                        army.AppendChild(armyQuantity);

                        armies.AppendChild(army);
                    }
                    villageNode.AppendChild(armies);

                    XmlNode recruitTimeEnd = villagesXml.CreateElement("recruitTimeEnd");
                    recruitTimeEnd.AppendChild(villagesXml.CreateTextNode(village.recruitTimeEnd.time.ToString()));
                    villageNode.AppendChild(recruitTimeEnd);


                    XmlNode orders = villagesXml.CreateElement("orders");
                    orderCounter = 0;
                    foreach (Order order in village.orders)
                    {
                        orderCounter++;

                        TradeOrder tradeOrder;

                        XmlNode orderNode = villagesXml.CreateElement("order" + orderCounter);
                        XmlNode orderType = villagesXml.CreateElement("type");
                        orderType.AppendChild(villagesXml.CreateTextNode(order.Type.ToString()));
                        orderNode.AppendChild(orderType);

                        if(order.Type.ToString() == "TRADE") // <--------Start trade
                        {
                            tradeOrder = order as TradeOrder;

                            XmlNode sentResources = villagesXml.CreateElement("sentResources");
                            XmlNode resourcesFood = villagesXml.CreateElement("food");
                            resourcesFood.AppendChild(villagesXml.CreateTextNode(tradeOrder.sentResources.food.ToString()));
                            sentResources.AppendChild(resourcesFood);

                            XmlNode resourcesStone = villagesXml.CreateElement("stone");
                            resourcesStone.AppendChild(villagesXml.CreateTextNode(tradeOrder.sentResources.stone.ToString()));
                            sentResources.AppendChild(resourcesStone);

                            XmlNode resourcesWood = villagesXml.CreateElement("wood");
                            resourcesWood.AppendChild(villagesXml.CreateTextNode(tradeOrder.sentResources.wood.ToString()));
                            sentResources.AppendChild(resourcesWood);

                            XmlNode resourcesIron = villagesXml.CreateElement("iron");
                            resourcesIron.AppendChild(villagesXml.CreateTextNode(tradeOrder.sentResources.iron.ToString()));
                            sentResources.AppendChild(resourcesIron);
                            orderNode.AppendChild(sentResources);

                            XmlNode sentArmy = villagesXml.CreateElement("sentArmy");
                            foreach (UnitType unit in unitType)
                            {
                                XmlNode army = villagesXml.CreateElement(unit.ToString());

                                XmlNode armyName = villagesXml.CreateElement("name");
                                armyName.AppendChild(villagesXml.CreateTextNode(tradeOrder.sentArmy[unit].name));
                                army.AppendChild(armyName);

                                XmlNode armyQuantity = villagesXml.CreateElement("quantity");
                                armyQuantity.AppendChild(villagesXml.CreateTextNode(tradeOrder.sentArmy[unit].quantity.ToString()));
                                army.AppendChild(armyQuantity);

                                sentArmy.AppendChild(army);
                            }
                            orderNode.AppendChild(sentArmy);
                        } // <--------Stop trade

                        XmlNode orderArmies = villagesXml.CreateElement("armies");
                        foreach (UnitType unit in unitType)
                        {
                            XmlNode army = villagesXml.CreateElement(unit.ToString());

                            XmlNode armyName = villagesXml.CreateElement("name");
                            armyName.AppendChild(villagesXml.CreateTextNode(order.army[unit].name));
                            army.AppendChild(armyName);

                            XmlNode armyQuantity = villagesXml.CreateElement("quantity");
                            armyQuantity.AppendChild(villagesXml.CreateTextNode(order.army[unit].quantity.ToString()));
                            army.AppendChild(armyQuantity);

                            orderArmies.AppendChild(army);
                        }
                        orderNode.AppendChild(orderArmies);

                        XmlNode orderDestination = villagesXml.CreateElement("destination");
                        XmlNode destinationLocation = villagesXml.CreateElement("location");
                        XmlNode desLocationX = villagesXml.CreateElement("x");
                        desLocationX.AppendChild(villagesXml.CreateTextNode(order.Destination.location.x.ToString()));
                        destinationLocation.AppendChild(desLocationX);

                        XmlNode desLocationY = villagesXml.CreateElement("y");
                        desLocationY.AppendChild(villagesXml.CreateTextNode(order.Destination.location.y.ToString()));
                        destinationLocation.AppendChild(desLocationY);
                        orderDestination.AppendChild(destinationLocation);

                        XmlNode destinationType = villagesXml.CreateElement("type");
                        destinationType.AppendChild(villagesXml.CreateTextNode(order.Destination.type.ToString()));
                        orderDestination.AppendChild(destinationType);
                        orderNode.AppendChild(orderDestination);

                        //origin order

                        XmlNode orderOrigin = villagesXml.CreateElement("origin");
                        XmlNode originLocation = villagesXml.CreateElement("location");
                        XmlNode originLocationX = villagesXml.CreateElement("x");
                        originLocationX.AppendChild(villagesXml.CreateTextNode(order.origin.location.x.ToString()));
                        originLocation.AppendChild(originLocationX);

                        XmlNode originLocationY = villagesXml.CreateElement("y");
                        originLocationY.AppendChild(villagesXml.CreateTextNode(order.origin.location.y.ToString()));
                        originLocation.AppendChild(originLocationY);
                        orderOrigin.AppendChild(originLocation);

                        XmlNode originType = villagesXml.CreateElement("type");
                        originType.AppendChild(villagesXml.CreateTextNode(order.origin.type.ToString()));
                        orderOrigin.AppendChild(originType);
                        orderNode.AppendChild(orderOrigin);

                        XmlNode orderDuration = villagesXml.CreateElement("duration");
                        orderDuration.AppendChild(villagesXml.CreateTextNode(order.Duration.time.ToString()));
                        orderNode.AppendChild(orderDuration);

                        XmlNode orderOverallTime = villagesXml.CreateElement("overallTime");
                        orderOverallTime.AppendChild(villagesXml.CreateTextNode(order.OverallTime.time.ToString()));
                        orderNode.AppendChild(orderOverallTime);
                        
                        orders.AppendChild(orderNode);
                    }
                    villageNode.AppendChild(orders);

                    // orderCounter write
                    XmlNode orderCount = countersXml.CreateElement("orderCounter");
                    orderCount.AppendChild(countersXml.CreateTextNode(orderCounter.ToString()));
                    villageCounters.AppendChild(orderCount);
                    // -----------

                    XmlNode queues = villagesXml.CreateElement("queues");
                    buildingQueueCounter = 0;
                    foreach (Buildings.BuildingQueueItem queue in village.queues.buildingQueue)
                    {
                        buildingQueueCounter++;

                        XmlNode buildingQueue = villagesXml.CreateElement("buildingQueue" + buildingQueueCounter);
                        XmlNode name = villagesXml.CreateElement("name");
                        name.AppendChild(villagesXml.CreateTextNode(queue.Name));
                        buildingQueue.AppendChild(name);

                        XmlNode start = villagesXml.CreateElement("start");
                        start.AppendChild(villagesXml.CreateTextNode(queue.Start.time.ToString()));
                        buildingQueue.AppendChild(start);

                        XmlNode end = villagesXml.CreateElement("end");
                        end.AppendChild(villagesXml.CreateTextNode(queue.End.time.ToString()));
                        buildingQueue.AppendChild(end);

                        XmlNode level = villagesXml.CreateElement("level");
                        level.AppendChild(villagesXml.CreateTextNode(queue.level.ToString()));
                        buildingQueue.AppendChild(level);

                        XmlNode toBuild = villagesXml.CreateElement("toBuild");
                        XmlNode toBuildLevel = villagesXml.CreateElement("level");
                        toBuildLevel.AppendChild(villagesXml.CreateTextNode(queue.toBuild.level.ToString()));
                        toBuild.AppendChild(toBuildLevel);

                        XmlNode toBuildType = villagesXml.CreateElement("type");
                        toBuildType.AppendChild(villagesXml.CreateTextNode(queue.toBuild.type.ToString()));
                        toBuild.AppendChild(toBuildType);
                        buildingQueue.AppendChild(toBuild);

                        queues.AppendChild(buildingQueue);
                    }

                    //buildingQueueCounter write
                    XmlNode buildingQueueCount = countersXml.CreateElement("buildingQueueCounter");
                    buildingQueueCount.AppendChild(countersXml.CreateTextNode(buildingQueueCounter.ToString()));
                    villageCounters.AppendChild(buildingQueueCount);
                    //-----------

                    researchQueueCounter = 0;
                    foreach (ResearchQueueItem queue in village.queues.researchQueue)
                    {
                        researchQueueCounter++;

                        XmlNode researchQueue = villagesXml.CreateElement("researchQueue" + researchQueueCounter);
                        XmlNode name = villagesXml.CreateElement("name");
                        name.AppendChild(villagesXml.CreateTextNode(queue.Name));
                        researchQueue.AppendChild(name);

                        XmlNode start = villagesXml.CreateElement("start");
                        start.AppendChild(villagesXml.CreateTextNode(queue.Start.time.ToString()));
                        researchQueue.AppendChild(start);

                        XmlNode end = villagesXml.CreateElement("end");
                        end.AppendChild(villagesXml.CreateTextNode(queue.End.time.ToString()));
                        researchQueue.AppendChild(end);

                        queues.AppendChild(researchQueue);
                    }

                    //recruitQueueCounter write
                    XmlNode researchQueueCount = countersXml.CreateElement("researchQueueCounter");
                    researchQueueCount.AppendChild(countersXml.CreateTextNode(researchQueueCounter.ToString()));
                    villageCounters.AppendChild(researchQueueCount);
                    //-----------

                    recruitQueueCounter = 0;
                    foreach (RecruitQueueItem queue in village.queues.recruitQueue)
                    {
                        recruitQueueCounter++;

                        XmlNode recruitQueue = villagesXml.CreateElement("recruitQueue" + recruitQueueCounter);
                        XmlNode name = villagesXml.CreateElement("name");
                        name.AppendChild(villagesXml.CreateTextNode(queue.Name));
                        recruitQueue.AppendChild(name);

                        XmlNode start = villagesXml.CreateElement("start");
                        start.AppendChild(villagesXml.CreateTextNode(queue.Start.time.ToString()));
                        recruitQueue.AppendChild(start);

                        XmlNode end = villagesXml.CreateElement("end");
                        end.AppendChild(villagesXml.CreateTextNode(queue.End.time.ToString()));
                        recruitQueue.AppendChild(end);

                        queues.AppendChild(recruitQueue);
                    }

                    //recruitQueueCounter write
                    XmlNode recruitQueueCount = countersXml.CreateElement("recruitQueueCounter");
                    recruitQueueCount.AppendChild(countersXml.CreateTextNode(recruitQueueCounter.ToString()));
                    villageCounters.AppendChild(recruitQueueCount);
                    //-----------

                    queueCounter = 0;
                    foreach (Villages.VillageQueues.QueueItem queue in village.queues.queue)
                    {
                        queueCounter++;
                    }
                    villageNode.AppendChild(queues);

                    //queueCounter write
                    XmlNode queueCount = countersXml.CreateElement("queueCounter");
                    queueCount.AppendChild(countersXml.CreateTextNode(queueCounter.ToString()));
                    villageCounters.AppendChild(queueCount);
                    //-----------

                    XmlNode researchTimeEnd = villagesXml.CreateElement("researchTimeEnd");
                    researchTimeEnd.AppendChild(villagesXml.CreateTextNode(village.researchTimeEnd.time.ToString()));
                    villageNode.AppendChild(researchTimeEnd);

                    villagesRoot.AppendChild(villageNode);
                    villagesXml.Save(playersVillages + "player" + playerCounter.ToString() + ".xml");
                }

                #endregion

                XmlNode villageCount = countersXml.CreateElement("villageCounter");
                villageCount.AppendChild(countersXml.CreateTextNode(villageCounter.ToString()));
                countersRoot.AppendChild(villageCount);

                countersXml.Save(playersCounters + "counter" + playerCounter + ".xml");

                playerCounter++;
            }

        }

        private void saveMap(Map map)
        {
            int tilesCounter;
            Tile[,] tiles = map.getMap();

            string mapPath = this.path + "map\\";
            Directory.CreateDirectory(mapPath);

            XmlDocument mapXml = new XmlDocument();
            XmlDeclaration mapDeclaration = mapXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            mapXml.AppendChild(mapDeclaration);

            XmlNode mapRoot = mapXml.CreateElement("map");
            mapXml.AppendChild(mapRoot);

            XmlNode mapSize = mapXml.CreateElement("size");
            XmlNode sizeX = mapXml.CreateElement("x");
            sizeX.AppendChild(mapXml.CreateTextNode(map.sizeX.ToString()));
            mapSize.AppendChild(sizeX);

            XmlNode sizeY = mapXml.CreateElement("y");
            sizeY.AppendChild(mapXml.CreateTextNode(map.sizeY.ToString()));
            mapSize.AppendChild(sizeY);
            mapRoot.AppendChild(mapSize);

            XmlNode tilesNode = mapXml.CreateElement("tiles");
            tilesCounter = 0;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tilesCounter++;
                    XmlNode tile = mapXml.CreateElement("tile" + tilesCounter);
                    XmlNode type = mapXml.CreateElement("type");
                    type.AppendChild(mapXml.CreateTextNode(tiles[i, j].type.ToString()));
                    tile.AppendChild(type);

                    XmlNode tileLocation = mapXml.CreateElement("location");
                    XmlNode x = mapXml.CreateElement("x");
                    x.AppendChild(mapXml.CreateTextNode(tiles[i, j].location.x.ToString()));
                    tileLocation.AppendChild(x);

                    XmlNode y = mapXml.CreateElement("y");
                    y.AppendChild(mapXml.CreateTextNode(tiles[i, j].location.y.ToString()));
                    tileLocation.AppendChild(y);
                    tile.AppendChild(tileLocation);
                    tilesNode.AppendChild(tile);
                }
            }
            mapRoot.AppendChild(tilesNode);

            XmlNode time = mapXml.CreateElement("time");
            time.AppendChild(mapXml.CreateTextNode(GameTime.now.time.ToString()));
            mapRoot.AppendChild(time);

            mapXml.Save(mapPath + "map.xml");
        }

        public void writeSave(Map map, List<Player> players)
        {
            this.saveMap(map);
            this.savePlayers(players);
        }
    }
}
