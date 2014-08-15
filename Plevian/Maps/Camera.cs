using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Maps
{
    public class Camera
    {
        public float x { get; private set; } 
        public float y { get; private set; }
        public float centerX
        {
            get
            {
                return x + width / 2;
            }
            private set
            {
                x = value - width / 2;
            }
        }
        public float centerY
        {
            get
            {
                return y + height / 2;
            }
            private set
            {
                y = value - height / 2;
            }
        }
        protected int height, width;
        protected int maxX, maxY;

        public Camera( int width, int height, int maxX, int maxY )
        {
            this.height = width;
            this.width = height;
            this.maxX = maxX;
            this.maxY = maxY;
            centerCameraOn(maxX / 2, maxY / 2);
        }

        public void centerCameraOn(int x, int y)
        {
            centerX = x;
            centerY = y;
            checkCameraBonds();
        }

        public void moveCameraBy(int x, int y)
        {
            this.x += x;
            this.y += y;
            checkCameraBonds();
        }

        public void moveCameraBy(Location loc)
        {
            moveCameraBy(loc.x, loc.y);
        }

        public void checkCameraBonds()
        {
            if (centerX < 0) centerX = 0;
            if (centerY < 0) centerY = 0;
            if (centerX > maxX) centerX = maxX;
            if (centerY > maxY) centerY = maxY;
        }

        public bool isInCamera(Location location)
        {
            return (location.x >= x && location.x <= x + width) &&
                   (location.y >= y && location.x <= y + height);
        }

        public void changeSize(int newWidth, int newHeight)
        {
            this.width = newWidth;
            this.height = newHeight;
            checkCameraBonds();
        }

        public int TranslateX(int x)
        {
            return x - (int)this.x;
        }

        public int translateY(int y)
        {
            return y - (int)this.y;
        }

        public float TranslateX(float x)
        {
            return x - this.x;
        }

        public float translateY(float y)
        {
            return y - this.y;
        }

        public Location translate(Location location)
        {
            return new Location(TranslateX(location.x), translateY(location.y));
        }

        public Vector2f translate( Vector2f vector )
        {
            return new Vector2f(TranslateX(vector.X), translateY(vector.Y));
        }

    }
}
