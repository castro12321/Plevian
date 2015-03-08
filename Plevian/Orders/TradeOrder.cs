using Plevian.Maps;
using Plevian.Players;
using Plevian.Resource;
using Plevian.Units;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Orders
{
    public class TradeOrder : Order
    {
        public readonly Resources sentResources;
        public readonly Army sentArmy;

        // TODO: "Army traders" Is it really needed? The traders will be taken from the village anyway so no need for that parameter
        public TradeOrder(Village owner, Tile origin, Tile destination, Army traders, Resources sentResources, Army sentArmy)
            : base(owner, origin, destination, traders, OrderType.TRADE)
        {
            this.sentResources = sentResources;
            this.sentArmy = sentArmy;

            if (!traders.contains(UnitType.TRADER))
                throw new KeyNotFoundException("You need a trader to... trade");
            if(destination.type != TerrainType.VILLAGE)
                throw new ArgumentException("You can trade with other villages only");

            Village sourceVillage = origin as Village;
            if (sentResources != null)
            {
                /* Idea for traders capacity limit
                int summedResources = sentResources.sumResources();
                int tradersCount = traders.get(UnitType.TRADER).quantity;
                if (tradersCount * Trader.lootCapacity < summedResources)
                    throw new Exception("Traders don't have enough capacity");
                */
                sourceVillage.takeResources(sentResources);
            }
            if (sentArmy != null)
                sourceVillage.takeArmy(sentArmy);
        }

        override protected void onEnd()
        {
            if (isGoingBack == true)
            {
                if (!(Destination is Village))
                    throw new Exception("Army returned to tile which is not Village!");
                Village village = Destination as Village;
                village.addArmy(army);
                completed = true;
                owner.Owner.SendMessage("System", "Trader returned", "Trader returned to the village " + Destination);
            }
            else
            {
                Village destinationVillage = Destination as Village;
                if (sentResources != null)
                    destinationVillage.addResources(sentResources);
                if (sentArmy != null)
                    destinationVillage.addArmy(sentArmy);

                turnBack();
                owner.Owner.SendMessage("System", "Trade completed", sentResources + " and " + army + " have beed transferred to " + destinationVillage);
            }
        }
    }
}
