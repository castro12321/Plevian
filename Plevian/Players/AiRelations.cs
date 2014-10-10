using Plevian.Buildings;
using Plevian.Debugging;
using Plevian.Resource;
using Plevian.TechnologY;
using Plevian.Units;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Players
{
    public class AiRelations
    {
        public Dictionary<Player, float> relations = new Dictionary<Player, float>();

        public float get(Player player)
        {
            if (!contains(player))
                add(player);
            return relations[player];
        }

        private void add(Player player)
        {
            relations.Add(player, 0f);
        }

        private bool contains(Player player)
        {
            return relations.ContainsKey(player);
        }

        public void set(Player player, float relations)
        {
            if (!contains(player))
                add(player);
            if (relations < -10f)
                relations = -10f;
            if (relations > 10f)
                relations = 10f;
            this.relations[player] = relations;
        }

        public void reduce(Player player, float reduce)
        {
            set(player, get(player) - reduce);
        }

        public void increase(Player player, float increase)
        {
            set(player, get(player) + increase);
        }

        public void step()
        {
            foreach(Player player in Game.game.players)
                set(player, get(player)*0.999f);
        }
    }
}
