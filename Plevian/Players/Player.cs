using Plevian.Messages;
using Plevian.Villages;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Players
{
    public class Player
    {
        public readonly string name;
        public readonly Color color;

        private List<Village> villages = new List<Village>();
        public List<Message> messages = new List<Message>();

        public Village Capital
        {
            get
            {
                return villages[0];
            }
        }

        public IReadOnlyCollection<Village> Villages
        {
            get
            {
                return villages.AsReadOnly();
            }
        }

        public Player(string name, Color color)
        {
            this.name = name;
            this.color = color;

            SendMessage(new Message("System", "Welcome", "Welcome to the game!", DateTime.Parse("2014-08-13")));
            SendMessage(new Message("God", "Meaning of the life", "Win the game", DateTime.Parse("2014-08-14")));
            SendMessage(new Message("Hitler", "Message to you", "I'll kill you", DateTime.Now));
        }

        public void addVillage(Village village)
        {
            villages.Add(village);
        }

        public void removeVillage(Village village)
        {
            if (!villages.Contains(village))
               throw new Exception("Removing not existing village");

            villages.Remove(village);
        }

        public void SendMessage(Message message)
        {
            messages.Add(message);
        }
    }
}
