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

            //Lies Datei ein und lege schreibe auf Array 
            char[,] mazeArray = GetInput();
        }

        private char[,] GetInput ()
        {

            string line;
            int counter = 0, i = 0;


            int columns = Convert.ToInt16(Console.ReadLine());
            int lines = Convert.ToInt16(Console.ReadLine());
            char[,] charArray = new char[columns, lines];

            do
            {
                //1. String einlesen 
                line = Console.ReadLine();
                //2. Chars aus String kopieren
                foreach(char c in line)
                {
                    // Char auf Array Zeile = Counter, Spalte = i legen
                    charArray[counter, i] = c;
                    // Nächster Char muss in nächste Spalte
                    i++;
                }

                //3. Bei nächstem String beginne in nächster Zeile und wieder Spalte 0 
                counter++;
                i = 0;

                
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
