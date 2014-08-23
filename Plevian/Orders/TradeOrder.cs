﻿using Plevian.Maps;
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
    class TradeOrder : Order
    {
        public readonly Resources sentResources;
        public readonly Army sentArmy;

        public TradeOrder(Tile origin, Tile destination, Army traders, Resources sentResources, Army sentArmy)
            : base(origin, destination, traders, OrderType.SUPPORT)
        {
            this.sentResources = sentResources;
            this.sentArmy = sentArmy;

            if (!traders.contain(UnitType.TRADER))
                throw new KeyNotFoundException("You need a trader to... trade");
            if(destination.type != TerrainType.VILLAGE)
                throw new Exception("You can trade with other villages only");

            Village sourceVillage = origin as Village;
            if (sentResources != null)
            {
                /* Idea for traders capacity limit
                int summedResources = sentResources.sumResources();
                int tradersCount = traders.get(UnitType.TRADER).quanity;
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
                if (!(destination is Village))
                    throw new Exception("Army returned to tile which is not Village!");
                Village village = destination as Village;
                village.addArmy(army);
                completed = true;
                Game.player.SendMessage("System", "Trader returned", "Trader returned to the village " + destination);
            }
            else
            {
                Village destinationVillage = destination as Village;
                if (sentResources != null)
                    destinationVillage.addResources(sentResources);
                if (sentArmy != null)
                    destinationVillage.addArmy(sentArmy);

                turnBack();
                Game.player.SendMessage("System", "Trade completed", sentResources + " and " + army + " have beed transferred to " + destinationVillage);
            }
        }
    }
}
