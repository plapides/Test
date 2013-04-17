using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SpinningBoxesWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Position {
            public float radius;
            public float angle;
            public float speed;
            public bool selected;
        }

        public Rectangle[] rects;
        public Position[] positions;
        public DispatcherTimer timer;

        double centerX;
        double centerY;

        int boxSize = 30;
        public MainWindow()
        {
            InitializeComponent();

            Random r = new Random();
            int numBoxes = 2000;
            int radius = 700;
            rects = new Rectangle[numBoxes];
            positions = new Position[numBoxes];

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 16);

            for (int i = 0; i < numBoxes; i++)
            {
                Color newColor = new Color();
                newColor.R = (byte)(r.NextDouble() * 255);
                newColor.G = (byte)(r.NextDouble() * 255);
                newColor.B = (byte)(r.NextDouble() * 255);
                newColor.A = 255;

                Rectangle newRect = new Rectangle();
                newRect.Width = boxSize;
                newRect.Height = boxSize;
                newRect.Fill = new SolidColorBrush(newColor);
                newRect.Margin = new Thickness((float)i / numBoxes);

                // give this box a random position
                //Vector3.Transform(new Vector3((float)r.NextDouble() * radius, 0, 0), Matrix.CreateRotationZ((float)(r.NextDouble() * 2 * Math.PI)));
                positions[i] = new Position();
                positions[i].radius = (float)Math.Sqrt(r.NextDouble()) * radius;
                positions[i].angle = (float)(r.NextDouble() * 2 * Math.PI);
                positions[i].speed = 5 * (float)r.NextDouble() / (float)Math.Sqrt(positions[i].radius);
                positions[i].selected = false;

                rects[i] = newRect;
                newRect.TouchEnter += new EventHandler<TouchEventArgs>(newRect_TouchEnter);

                canvas.Children.Add(newRect);
                //Canvas.SetLeft(newRect, r.NextDouble() * radius);
                //Canvas.SetTop(newRect, r.NextDouble() * radius);
                //this.AddChild(newRect);
            }

            centerX = 2560 / 2;
            centerY = 1440 / 2;

            timer.Start();
        }

        void newRect_TouchEnter(object sender, TouchEventArgs e)
        {
            //int i = 5;
            Rectangle rect = (Rectangle)sender;
            int i = (int)(rect.Margin.Left * rects.Length);
            positions[i].selected = true;
            rects[i].Width = boxSize * 2;
            rects[i].Height = boxSize * 2;
            //throw new NotImplementedException();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < rects.Length; i++)
            {
                positions[i].angle += positions[i].speed * 0.06f;
                Canvas.SetLeft(rects[i], Math.Cos(positions[i].angle) * positions[i].radius + centerX);
                Canvas.SetTop(rects[i], Math.Sin(positions[i].angle) * positions[i].radius + centerY);
            }
        }
    }
}
