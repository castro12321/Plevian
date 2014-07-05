﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Plevian.Debugging;
using SFML.Graphics;
using SFML.Window;
using Plevian.Utils;

namespace Plevian
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Alloc debug console
            NativeMethods.AllocConsole();
            Console.WriteLine("Debug Console");
            
            // Load the game
            //initializeSFML();

            Game game = new Game();
            //while (true)
                game.tick();
        }

        public void addSFML()
        {
            DrawingSurface surface = new DrawingSurface();
            surface.Size = new System.Drawing.Size(400, 400);
            surface.Location = new System.Drawing.Point(100, 100);

            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
            host.Child = surface;

            sfml_map.Children.Add(host);

            // initialize sfml
            SFML.Graphics.RenderWindow renderwindow = new SFML.Graphics.RenderWindow(surface.Handle); // creates our SFML RenderWindow on our surface control

            Random random = new Random();

            byte oldR = 128;
            byte oldG = 128;
            byte oldB = 128;

            // drawing loop
            while (true) // loop while the window is open
            {
                System.Windows.Forms.Application.DoEvents(); // handle form events
                renderwindow.DispatchEvents(); // handle SFML events - NOTE this is still required when SFML is hosted in another window

                byte newR = (byte)random.Next(oldR - 2, oldR + 3);
                byte newG = (byte)random.Next(oldG - 2, oldG + 3);
                byte newB = (byte)random.Next(oldB - 2, oldB + 3);
                oldR = newR;
                oldG = newG;
                oldB = newB;
                Logger.c("new color: " + newR + " " + newG + " " + newB);
                Color newColor = new Color(newR, newG, newB);
                for (int i = 0; i < 1000 * 1000; ++i)
                    ;

                renderwindow.Clear(newColor); // clear our SFML RenderWindow
                renderwindow.Display(); // display what SFML has drawn to the screen
            }
        }

        private static void initializeSFML()
        {
            // Create the main window
            RenderWindow app = new RenderWindow(new VideoMode(1024, 768), "SFML Works!");
            app.Closed += new EventHandler(OnClose);

            Color windowColor = new Color(0, 192, 255);

            RectangleShape shape = new RectangleShape(new Vector2f(250, 50));
            shape.Position = new Vector2f(100, 100);
            Color outColor = Color.White;
            Color inColor = Color.Green;

            // Start the game loop
            while (app.IsOpen())
            {
                // Process events
                app.DispatchEvents();
                // Clear screen
                app.Clear(windowColor);

                int mouseX = Mouse.GetPosition(app).X;
                int mouseY = Mouse.GetPosition(app).Y;

                Console.WriteLine(mouseX + ", " + mouseY);

                shape.FillColor = outColor;
                if (Functions.Collision(mouseX, 100, mouseY, 100, 2, 250, 2, 50))
                {
                    shape.FillColor = inColor;
                }

                app.Draw(shape);

                
                

                // Update the window
                app.Display();
            } //End game loop
        }

        static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        private class DrawingSurface : System.Windows.Forms.Control
        {
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
}
