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

            Font mazeFont = new Font("Arial", 12);

            //Lies Datei ein und lege schreibe auf Array 
            char[,] mazeArray = GetInput();
            // Berechne Spacing
            float spaceX = bounds.Width / mazeArray.GetUpperBound(2) + 1;
            float spaceY = bounds.Height / mazeArray.GetUpperBound(1) + 1; 
            // Zeichne Labyrinth
            DrawMaze(e.Graphics, mazeArray, mazeFont, spaceX, spaceY);

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

        //Übergebe Array, Startkoordinaten, Schriftart, Abstand zwischen den Zeichen in X und Y 
        private void DrawMaze(Graphics g, Array array, Font font, float spacingX, float spacingY)
        {
            // Baue zwei Schleifen die das Labyrinth zeichen und frage ab, welches Zeichen 
            // Setze anhand der Zeichen unterschiedliche Brushes ein

        }
        override protected void OnKeyDown(KeyEventArgs e)
        {

        }
    }

}
