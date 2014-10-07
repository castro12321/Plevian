using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Plevian.Save
{
    class SaveReader
    {
        private string path;
        public SaveReader(string name)
        {
            this.path = "Save\\" + name + "\\";
        }

        public void getPlayer(int numberOfPlayer)
        {
            string playersPath = this.path + "players\\";
            string playersBasicInfo = playersPath + "basicInfo\\";
            string playersMessages = playersPath + "messages\\";
            string playersTechnologies = playersPath + "technologies\\";
            string playersVillages = playersPath + "villages\\";

            string[] basicInfoPaths = Directory.GetFiles(playersBasicInfo);
            string[] messagesPaths = Directory.GetFiles(playersMessages);
            string[] technologiesPaths = Directory.GetFiles(playersTechnologies);
            string[] villagesPaths = Directory.GetFiles(playersVillages);

            System.Collections.Generic.List<string> playerName = new List<string>();

            //XmlDocument xml = new XmlDocument();

            // < ------- basic info read
            /*xml.Load(basicInfoPaths[numberOfPlayer-1]);
            XmlNodeList name = xml.GetElementsByTagName("name");*/
            
            // < ------- messages read

            XDocument xml = XDocument.Load(messagesPaths[0]);
           // XElement root = xml.Element("village1"); 
        }

        public void playerRestore()
        {

        }

        public void mapRestore()
        {
            
        }
    }
}
