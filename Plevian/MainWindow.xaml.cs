using System;
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
using System.Windows.Forms.Integration;

namespace Plevian
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private static bool running = true;
        private static Random random = new Random();
        private RenderWindow mapRender;
        private RenderWindow villageRender;
        private Game game;

        public MainWindow()
        {
            // Create window
            InitializeComponent();

            // Initialize game
            game = new Game();

            // Add SFML (map)
            DrawingSurface mapSurface = new DrawingSurface();
            mapSurface.Size = new System.Drawing.Size(400, 400);
            mapSurface.Location = new System.Drawing.Point(100, 100);
            mapRender = new RenderWindow(mapSurface.Handle);
            sfml_map.Child = mapSurface;
            
            // Add SFML (village)
            DrawingSurface villageSurface = new DrawingSurface();
            villageSurface.Size = new System.Drawing.Size(400, 400);
            villageSurface.Location = new System.Drawing.Point(100, 100);
            villageRender = new RenderWindow(villageSurface.Handle);
            sfml_village.Child = villageSurface;
             
            // Listen to some events
            Closed += new EventHandler(OnClose);
        }

        byte oldR = 128;
        byte oldG = 128;
        byte oldB = 128;
        private Color randomColor()
        {
            oldR = (byte)random.Next(oldR - 2, oldR + 3);
            oldG = (byte)random.Next(oldG - 2, oldG + 3);
            oldB = (byte)random.Next(oldB - 2, oldB + 3);
            return new Color(oldR, oldG, oldB);
        }

        
        public void run()
        {
            while (running)
            {
                System.Threading.Thread.Sleep(10); // Some fake lag (if needed)

                // Process events
                System.Windows.Forms.Application.DoEvents(); // handle form events

                // handle SFML events - NOTE this is still required when SFML is hosted in another window
                //mapRender.DispatchEvents();
                //villageRender.DispatchEvents();

                // Do the logic
                game.tick();

                // Render
                mapRender.Clear(randomColor());
                villageRender.Clear(randomColor()); // clear our SFML RenderWindow
                mapRender.Display();
                villageRender.Display(); // display what SFML has drawn to the screen
            }
        }

        /*
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
                    shape.FillColor = inColor;

                app.Draw(shape);

                app.Display();
            } //End game loop
        }
        */

        static void OnClose(object sender, EventArgs e)
        {
            running = false;
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
