using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Units;

namespace Plevian.Battles
{
    class Battle
    {
        Army attacker, defender;
        private float attackerLuck, defenderDefense;

        public Battle(Army attacker, Army defender, float luck, float defense)
        {
            this.attacker = attacker;
            this.defender = defender;
            attackerLuck = luck;
            defenderDefense = defense;
        }

        public Report makeBattle()
        {
            int attackPower = 0;
            int defenseInf = 0, defenseCav = 0, defenseArch = 0;

            attackPower = attacker.getAttackStrength();
            defenseInf = defender.getDefenseInfantry();
            defenseCav = defender.getDefenseCavalry();
            defenseArch = defender.getDefenseArchers();


            throw new NotImplementedException("makeBattle() not implemented yet");
            return new Report();
        }
        

    }
}
