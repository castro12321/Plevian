using Plevian.Buildings;
using Plevian.Debugging;
using Plevian.Maps;
using Plevian.Orders;
using Plevian.Resource;
using Plevian.TechnologY;
using Plevian.Units;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Players
{
    public class AiPlayer : Player
    {
        private static bool disabled = false;
        private static Random random = new Random();

        public List<AiModule> modules = new List<AiModule>();
        public AiModuleRelations relations;

        public AiPlayer(String name, SFML.Graphics.Color color)
            : base(name, color)
        {
            modules.Add(relations = new AiModuleRelations(this));
            modules.Add(new AiModuleBuilding(this));
            modules.Add(new AiModuleRecruiting(this));
            modules.Add(new AiModuleResearch(this));
            modules.Add(new AiModuleInternalCooperation(this));
            modules.Add(new AiModuleExpansion(this));
            modules.Add(new AiModuleAttacks(this));
        }

        public override void tick(ulong timediff)
        {
            base.tick(timediff);
            if (disabled)
                return;

            Logger.AI("AI tick " + name);
            foreach (AiModule module in modules)
                module.tick();
        }

    }
}
