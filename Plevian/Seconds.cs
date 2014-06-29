using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian
{
    public class Seconds : GameTime
    {
        public int seconds { get { return time; } set { time = value; } }

        public Seconds(int seconds)
            : base(seconds)
        {
        }
    }
}
