using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Orders;
using Plevian.Villages;

namespace Plevian.Players
{
    public class AiModuleRelations : AiModule
    {
        public Dictionary<Player, float> relations = new Dictionary<Player, float>();

        public AiModuleRelations(AiPlayer ai)
            : base(ai)
        {
        }

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

        public override void tick()
        {
            // Time heals all wounds
            foreach (Player player in Game.game.players)
                set(player, get(player) * 0.999f);

            // Set -10 relations for those who attack us
            foreach(Village village in ai.villages)
            {
                foreach(Order order in village.orders)
                {
                    if (order.owner == village)
                        continue;

                    if (order.Type == OrderType.ATTACK)
                        set(order.owner.Owner, -10f);
                }
            }
        }
    }
}
