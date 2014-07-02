using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.UI
{
    public class Control
    {
        UiPosition position;
        UISize size;


        public Control(UiPosition position)
        {
            this.position = position;
        }

        public readonly UiPosition getPosition()
        {
            return position;
        }

        public readonly UiPosition getSize()
        {
            return size;
        }
    }
}
