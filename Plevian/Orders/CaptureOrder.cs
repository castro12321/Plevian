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
    public class CaptureOrder : AttackOrder
    {
        public CaptureOrder(Tile origin, Tile destination, Army army)
            : base(origin, destination, army)
        {
            if (!army.contain(UnitType.DUKE))
                throw new Exception("CaptureOrder without duke!");
            this.Type = OrderType.CAPTURE;
        }

        protected override void onFightWin(Battles.Report report)
        {
            if (army.contain(UnitType.DUKE))
            {
                Village village = Destination as Village;
                Village attVillage = origin as Village;
                Player attacker = attVillage.Owner;
                attacker.CaptureVillage(village);
                completed = true;
                village.addArmy(army);
                report.villageCaptured = true;
            }
            else
            {
                base.onFightWin(report);
            }
        }
    }
}
