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

        private Dictionary<string, Dictionary<string, int>> getCounters(string path)
        {
            Dictionary<string, Dictionary<string, int>> counters = new Dictionary<string, Dictionary<string, int>>();

            XDocument countersXml = XDocument.Load(path);
            XElement counterRoot = countersXml.Element("counters");

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
                    test.Add("queueCounter", int.Parse(counterRoot.Element("village" + i).Element("queueCounter").Value));
                }

                if (i == 0)
                    counters.Add("basicCounters", test);
                else
                    counters.Add("village" + i, test);
            }

            return counters;
        }

        private ObservableCollection<Villages.Village> getVillages(string path, Dictionary<string, Dictionary<string, int>> counters)
        {
            ObservableCollection<Villages.Village> villages = new ObservableCollection<Villages.Village>();
            Dictionary<string, int> basicCounters = counters["basicCounters"];

            for (int i = 1; i < basicCounters["villageCounter"]; i++)
            {
                Dictionary<string, int> village = counters["village" + i];
            }

            return null;
        }

        private ObservableCollection<Messages.Message> getMessages(string path, int messagesCounter)
        {
            ObservableCollection<Messages.Message> messages = new ObservableCollection<Messages.Message>();
            XDocument messagesXml = XDocument.Load(path);
            XElement messagesRoot = messagesXml.Element("messages");

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
            XElement techRoot = techXml.Element("technologies");

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

        public SaveReader(string name)
        {
            this.path = "Save\\" + name + "\\";
        }

        public Players.Player getPlayer(int numberOfPlayer)
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

            Dictionary<string, Dictionary<string, int>> counters = this.getCounters(playersCounters + "counter" + numberOfPlayer + ".xml");
            Dictionary<string, int> basicCounters = counters["basicCounters"];

            //basic info ------------
            XDocument basicInfoXml = XDocument.Load(playersBasicInfo + "player" + numberOfPlayer + ".xml");
            XElement basicInfoRoot = basicInfoXml.Element("basicInfo");
            
            SFML.Graphics.Color color = new SFML.Graphics.Color(System.Byte.Parse(basicInfoRoot.Element("color").Element("R").Value),
                                                                System.Byte.Parse(basicInfoRoot.Element("color").Element("R").Value),
                                                                System.Byte.Parse(basicInfoRoot.Element("color").Element("R").Value),
                                                                System.Byte.Parse(basicInfoRoot.Element("color").Element("R").Value));
            string name = basicInfoRoot.Element("name").Value;

            Players.Player player = new Players.Player(name, color);
            player.messages = this.getMessages(playersMessages + "player" + numberOfPlayer + ".xml", basicCounters["messageCounter"]);
            player.technologies.technologies = this.getTechnologies(playersTechnologies + "player" + numberOfPlayer + ".xml", basicCounters, player);
            return null;
        }

        public void playerRestore()
        {

        }

        public void mapRestore()
        {
            
        }
    }
}
