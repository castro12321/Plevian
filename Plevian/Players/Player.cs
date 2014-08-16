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

        public Village Capital { get { return villages[0]; } }

        public Player(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }

        public void addVillage(Village village)
        {
            villages.Add(village);
        }

        public void removeVillage(Village village)
        {
           // if (!villages.Contains(village))
           //     throw new Exception("Removing not existing village");

            villages.Remove(village);
        }
    }
}
