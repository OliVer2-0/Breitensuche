using System;
using System.Windows.Forms;
using System.Drawing;

namespace Breitensuche
{
    class Breitensuche : Form
    {
        public Breitensuche()
        {
            Width = 600;
            Height = 600;
            Text = "Breitensuche";
        }

        static void Main()
        {
            Application.Run(new Breitensuche());
        }

        override protected void OnPaint(PaintEventArgs e)
        {
            RectangleF bounds = e.Graphics.VisibleClipBounds; // um die Groesse des sichtbaren Bereichs zu ermitteln
        }

        private char[,] GetInput ()
        {
            char[,] charArray[z;
            string line;
            int counter = 0, z, s;
            do
            {
                counter++;
                if(counter == 1)
                {
                    line = Console.ReadLine();
                    s = line;
                }
                line = Console.ReadLine();
            } while (line != null);
            return charArray;
        }

        private void SierpinskiTeppich(Graphics g, Brush brush, Single x, Single y, Single width, Single height, int iteration)
        {

        }
        override protected void OnKeyDown(KeyEventArgs e)
        {

        }
    }

}
