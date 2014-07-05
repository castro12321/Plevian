using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian
{
    public class Entry
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Your initialization code
            App.Main();
            Plevian.Debugging.Logger.c("hello entry");
        }
    }
}
