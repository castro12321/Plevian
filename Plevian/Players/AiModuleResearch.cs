using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Debugging;
using Plevian.TechnologY;
using Plevian.Villages;

namespace Plevian.Players
{
    public class AiModuleResearch : AiModule
    {
        private static Random random = new Random();

        public AiModuleResearch(AiPlayer ai)
            : base(ai)
        {
        }

        public override void tick()
        {
            foreach(Village village in ai.villages)
                DoResearching(village);
        }

        /* Main idea:
         * Just randomly research whatever you can...
         * Reasearch only if you have free resources
         */

        private Technology RandomTechnology(Village village)
        {
            return ai.technologies.technologies[random.Next(ai.technologies.technologies.Count)];
        }

        private void DoResearching(Village village)
        {
            if (village.queues.researchQueue.Count == 0)
            {
                Technology toResearch = RandomTechnology(village);
                if (village.canResearch(toResearch)
                && village.resources.canAfford(toResearch.Price * toResearch.getAiResourceModifier()))
                {
                    Logger.AI("Researching " + toResearch);
                    village.research(toResearch);
                }
            }
        }
    }
}
