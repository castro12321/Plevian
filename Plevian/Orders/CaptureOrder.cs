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
    class CaptureOrder : AttackOrder
    {
        public CaptureOrder(Tile origin, Tile destination, Army army)
            : base(origin, destination, army)
        {
            this.type = OrderType.CAPTURE;
        }

        protected override void onFightWin(Battles.Report report)
        {
            Village village = destination as Village;
            Village attVillage = destination as Village;
            Player attacker = attVillage.Owner;
            attacker.CaptureVillage(village);
            completed = true;
            village.addArmy(army);
        }
    }
}
