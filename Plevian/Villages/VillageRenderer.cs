﻿using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;

namespace Plevian.Maps
{
    class VillageView : System.Windows.Forms.Control
    {
        private readonly RenderWindow renderer;
        private readonly Game game;

        public VillageView(Game game)
        {
            this.game = game;
            Size = new System.Drawing.Size(400, 400);
            Location = new System.Drawing.Point(100, 100);
            renderer = new RenderWindow(Handle);
        }

        public void handleEvents()
        {
            renderer.DispatchEvents(); // handle SFML events - NOTE this is still required when SFML is hosted in another window
        }

        public void render()
        {
            renderer.Clear(randomColor());
            renderer.Display();
        }

        private static Random random = new Random();
        byte oldR = 192;
        byte oldG = 192;
        byte oldB = 192;
        private Color randomColor()
        {
            oldR = (byte)random.Next(oldR - 2, oldR + 3);
            oldG = (byte)random.Next(oldG - 2, oldG + 3);
            oldB = (byte)random.Next(oldB - 2, oldB + 3);
            return new Color(oldR, oldG, oldB);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            // don't call base.OnPaint(e) to prevent forground painting
            // base.OnPaint(e);
        }
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
        {
            // don't call base.OnPaintBackground(e) to prevent background painting
            //base.OnPaintBackground(pevent);
        }
    }
}