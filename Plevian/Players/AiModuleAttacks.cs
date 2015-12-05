using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Debugging;
using Plevian.Orders;
using Plevian.Units;
using Plevian.Villages;

namespace Plevian.Players
{
    public class AiModuleAttacks : AiModule
    {
        private static Random random = new Random();

        public AiModuleAttacks(AiPlayer ai)
            : base(ai)
        {
        }

        public override void tick()
        {
            foreach(Village village in ai.villages)
                DoAttacks(village);
        }

        /* Main ideas:
         * 
         * Deciding if we should attack
         * - Calculate average unit count out of the all villages on the map (once every 5 minute or so)
         * - If we have more than average; send 25%-75% of our army to steal cookies
         * 
         * Choosing target (priorities)
         * - Attacking enemy at war (~95% chance)
         * - Attacking neutral (~5% chance)
         * - Attacking ally (Less than 1% chance)
         * 
         * Relationship:
         * Add Map<Player, Relationship> for chances?
         * The worse the relationship, the more chance that we attack the player
         * -10 --> +10
         * When -10 --> ~95% chance of being attacked
         * When 0 --> Around 5% chance
         * When +10 --> Less than 1%
         * The relationship is going to be 0 over time.
         * Like relationship *= 0.999 per tick?
         */

        private double RelativeArmySizeToAverage(Village village)
        {
            double armySize = village.army.size();
            return armySize / GameStats.AverageUnitCountPerVillage;
        }

        private void DoAttacks(Village village)
        {
            Logger.AI("Doing attacks");
            double relativeArmySize = RelativeArmySizeToAverage(village);
            double chance = random.NextDouble(); // Chance of attacking

            // TODO: profile <relativeArmySize> and <chance> values
            if (relativeArmySize < 0.5d) // Do we have less than 50% of avg units per village?
            {
                if (chance < 0.99d) // 1% chance of attacking
                    return;
            }
            else if (relativeArmySize < 0.9d) // 90%?
            {
                if (chance < 0.95d) // 5% chance of attacking
                    return;
            }
            else if (relativeArmySize < 1.1d) // 110%?
            {
                if (chance < 0.85d) // 15% chance of attacking
                    return;
            }
            else // Do we have more than 110% of avg units per village?
            {
                if (chance < 0.75d) // 25% chance of attacking
                    return;
            }

            // Didn't quit earlier? attack!
            //ComputerPlayer owner = village.Owner as ComputerPlayer;
            int randomized = random.Next(Game.game.players.Count);
            Player toAttack = Game.game.players[randomized];
            if (toAttack == ai)
                return; // Don't attack ourselves xD
            if (toAttack.villages.Count == 0)
                return; // Well, shit happens. TODO: Remove players with no villages? (they actually lost the game, so no need to keep them)

            randomized = random.Next(-10, 10);
            if (randomized < ai.relations.get(toAttack)) // The better your relations
                return; // The less chance being attacked

            var villageArmy = village.army.getUnitsByType();
            Army attacking = new Army();

            foreach (var unit in villageArmy)
            {
                Unit toAdd = unit.Value.clone();
                toAdd.quantity = random.Next(toAdd.quantity / 2, toAdd.quantity);
                attacking.add(toAdd);
            }

            if (attacking.size() < 10)
                return; // Don't send (almost) empty armies lol, that's dumb. Hey, AI! Don't be dumb!

            Village targetVillage = toAttack.villages[random.Next(0, toAttack.villages.Count)];
            Order order = new AttackOrder(village, village, targetVillage, attacking);
            village.addOrder(order);
        }
    }
}
