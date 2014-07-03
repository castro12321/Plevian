using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.UI
{
    public class UISize
    {
        int width, height;

        public UISize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public UISize(UISize other)
        {
            this.width = other.width;
            this.height = other.height;
        }
    }
}
