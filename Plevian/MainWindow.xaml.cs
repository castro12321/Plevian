using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

            // Load GUI
            
            
            // Link SFML to GUI and game
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


            return;
            
            // initialize the form
            System.Windows.Forms.Form form = new System.Windows.Forms.Form(); // create our form
            form.Size = new System.Drawing.Size(600, 600); // set form size to 600 width & 600 height
            form.Show(); // show our form
            DrawingSurface rendersurface = new DrawingSurface(); // our control for SFML to draw on
            rendersurface.Size = new System.Drawing.Size(400, 400); // set our SFML surface control size to be 500 width & 500 height
            form.Controls.Add(rendersurface); // add the SFML surface control to our form
            rendersurface.Location = new System.Drawing.Point(100, 100); // center our control on the form

            // initialize sfml
            SFML.Graphics.RenderWindow renderwindow = new SFML.Graphics.RenderWindow(rendersurface.Handle); // creates our SFML RenderWindow on our surface control

            // drawing loop
            while (form.Visible) // loop while the window is open
            {
                System.Windows.Forms.Application.DoEvents(); // handle form events
                renderwindow.DispatchEvents(); // handle SFML events - NOTE this is still required when SFML is hosted in another window
                renderwindow.Clear(SFML.Graphics.Color.Yellow); // clear our SFML RenderWindow
                renderwindow.Display(); // display what SFML has drawn to the screen
            }
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
