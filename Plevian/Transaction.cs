using Plevian.Resource;
using Plevian.Units;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian
{
    // Ideas:
    // fee --> Cost of the transaction that goes to the admins (yay)
    public class Transaction
    {
        public readonly Village sender;
        public readonly Village receiver;

        public readonly Resources sentResources;
        public readonly Army sentArmy;

        public Transaction(Village sender, Village receiver, Resources sentResources, Army sentArmy)
        {
            this.sender = sender;
            this.receiver = receiver;
        }
    }
}
