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
        public CaptureOrder(Village owner, Tile origin, Tile destination, Army army)
            : base(owner, origin, destination, army, OrderType.CAPTURE)
        {
            if (!army.contains(UnitType.DUKE))
                throw new Exception("CaptureOrder without duke!");
        }

        protected override void onFightWin(Battles.Report report)
        {
            if (army.contains(UnitType.DUKE))
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
