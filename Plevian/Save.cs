using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian
{
    class Save
    {
        private readonly String path;

        public Save(String path)
        {
            this.path = path + "\\";
        }

        public String getMapFile()
        {
            return path + "map.txt";
        }
    }
}
