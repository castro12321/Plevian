using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
namespace Plevian.UI
{
    public class Control
    {
        public UiPosition position { protected set; get; }
        public UISize size { protected set; get; }
        protected List<Control> childrens = new List<Control>();
        protected Control father = null;
        protected ControlState state;


        public Control(UiPosition position)
        {
            this.position = position;
        }

        public void tick()
        {
            Vector2i mousePos = Mouse.GetPosition();
            
        }

        public readonly UiPosition getRelativePosition()
        {
            return position;
        }

        public readonly UiPosition getPosition()
        {

            if(father == null)
            return position;
            return father.getRelativePosition() + position;
        }

        public readonly UISize getSize()
        {
            return size;
        }

        public void addChildren(Control child)
        {
            childrens.Add(child);
        }
    }
}
