using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian;

namespace Tests
{
    class FakeGameTime : GameTime
    {
        public FakeGameTime(int time)
            : base(time)
        {
        }

        public void addFakeTime(int seconds)
        {
            now += new Seconds(seconds);
        }
    }
}
