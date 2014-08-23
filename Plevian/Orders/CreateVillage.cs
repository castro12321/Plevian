using Plevian.Maps;
using Plevian.Players;
using Plevian.Units;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Orders
{
    class CreateVillage : Order
    {
        public CreateVillage(Tile origin, Tile destination, Army army)
            : base(origin, destination, army, OrderType.CAPTURE)
        {
            if (destination.type != TerrainType.PLAINS)
                throw new Exception("You can create villages only on PLAINS tiles");
            if (!army.contain(UnitType.SETTLER))
                throw new KeyNotFoundException("You need a settler to create village");
        }

        override protected void onEnd()
        {
            // Somebody taken over this tile? abort
            if (Destination.type != TerrainType.PLAINS)
            {
                Game.player.SendMessage("System", "Failed to create village", "The village has been created by someone else");
                turnBack();
                return;
            }

            Player player = Game.player;
            Map map = Game.game.map;

            Village newVillage = new Village(Destination.location, player, "New village");
            map.place(newVillage);
            player.addVillage(newVillage);

            /* TODO: doesn't work
            Army settler = new Army();
            settler += UnitFactory.createUnit(UnitType.SETTLER, 1);

            army -= settler;
            newVillage.addArmy(army);
            */

            Game.player.SendMessage("System", "Created village", newVillage.name + " created at " + newVillage.location);

            completed = true;
        }
    }
}
