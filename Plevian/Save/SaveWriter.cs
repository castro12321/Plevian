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
            string filePath = this.path;
            int playerCounter = 0;

            foreach(Players.Player player in players)
            {
                ++playerCounter;
                int messageCounter = 0;
                int villageCounter = 0;
                StreamWriter basicInfo = new StreamWriter(filePath + "BasicInfoPlayer" + playerCounter.ToString() + ".txt");
                basicInfo.WriteLine(player.Capital.ToString());
                basicInfo.WriteLine(player.color.ToString());
                basicInfo.WriteLine(player.name);
                basicInfo.Close();
                foreach (Messages.Message message in player.messages)
                {
                    ++messageCounter;
                    StreamWriter messages = new StreamWriter(filePath + messageCounter.ToString() + "MessagesOfPlayer" + playerCounter.ToString() + ".txt");
                    messages.WriteLine(message.ToString());
                    messages.Close();
                }

                foreach (Villages.Village village in player.villages)
                {
                    ++villageCounter;
                    StreamWriter villages = new StreamWriter(filePath + villageCounter.ToString() + "VillageOfPlayer" + playerCounter.ToString() + ".txt");
                    villages.WriteLine(village.name);
                    villages.Close();
                }

               //TODO: download from player list of technologies
            }
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
