using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian;

namespace Tests.Unit
{
    public abstract class TestWithTime
    {
        private FakeGameTime time;
        
        protected void fakeTime(int time)
        {
            this.time = new FakeGameTime(time);
            GameTime.init(this.time);
        }

        protected void addFakeTime(int time)
        {
            this.time.addFakeTime(time);
        }
    }
}
