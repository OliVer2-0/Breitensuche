using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

namespace Breitensuche
{
    class Breitensuche : Form
    {
        char[,] mazeArray;
        int playerPosX = 0, playerPosY = 0;

        public Breitensuche()
        {
            Width = 600;
            Height = 600;
            Text = "Breitensuche";
            ResizeRedraw = true;
            mazeArray = GetInput();
        }

        static void Main()
        {
            Application.Run(new Breitensuche());
        }

        override protected void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            RectangleF bounds = e.Graphics.VisibleClipBounds; // um die Groesse des sichtbaren Bereichs zu ermitteln
            
            Font mazeFont = new Font("Arial", 12);
            //Lies Datei ein und lege schreibe auf Array 
            //char[,] mazeArray = GetInput();

            // Berechne Spacing
            float spaceX = bounds.Width / (float)(mazeArray.GetUpperBound(1) + 1);
            float spaceY = bounds.Height / (float)(mazeArray.GetUpperBound(0) + 1);
            // Zeichne Labyrinth - Übergabe mazeArray nicht sinnvoll ?! 
            DrawMaze(e.Graphics, mazeArray, mazeFont, spaceX, spaceY);
        }

        private char[,] GetInput()
        {

            string line = "";
            int counter = 0, i = 0;


            int columns = Convert.ToInt16(Console.ReadLine());
            int lines = Convert.ToInt16(Console.ReadLine());
            char[,] charArray = new char[lines, columns];

            while (counter <= charArray.GetUpperBound(0))
            {
                //1. String einlesen 
                line = Console.ReadLine();
                //2. Chars aus String kopieren
                foreach (char c in line)
                {
                    // Char auf Array Zeile = Counter, Spalte = i legen
                    charArray[counter, i] = c;
                    // Nächster Char muss in nächste Spalte
                    i++;
                }

                //3. Bei nächstem String beginne in nächster Zeile und wieder Spalte 0 
                counter++;
                i = 0;
            } 

            return charArray;
        }

        //Übergebe Array, Startkoordinaten, Schriftart, Abstand zwischen den Zeichen in X und Y 
        private void DrawMaze(Graphics g, char[,] array, Font font, float spacingX, float spacingY)
        {
            // Baue zwei Schleifen die das Labyrinth zeichen und frage ab, welches Zeichen 
            // Setze anhand der Zeichen unterschiedliche Brushes ein
            float x = 0, y = 0;

            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                for (int n = 0; n <= array.GetUpperBound(1); n++)
                {
                    string s = Convert.ToString(array[i, n]);

                    switch (s)
                    {
                        case "#":
                            SolidBrush brushGreen = new SolidBrush(Color.Green);
                            g.DrawString(s, font, brushGreen, x, y);
                            break;
                        case ".":
                            SolidBrush brushBlue = new SolidBrush(Color.Blue);
                            g.DrawString("°", font, brushBlue, x, y);
                            break;
                        case "@":
                            SolidBrush brushRed = new SolidBrush(Color.Red);
                            g.DrawString(s, font, brushRed, x, y);
                            //Speichere Start-Spielerposi für späteres navigieren im Laby
                            playerPosX = n;
                            playerPosY = i;
                            break;

                    }
                    x = x + spacingX;
                }
                y = y + spacingY;
                x = 0;
            }

        }
        override protected void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (mazeArray[playerPosY - 1, playerPosX] == '#')
                        break;
                    else if(mazeArray[playerPosY -1, playerPosX] == '.')
                    {
                        mazeArray[playerPosY - 1, playerPosX] = '@';
                        mazeArray[playerPosY, playerPosX] = ' ';
                    }
                    else
                    {
                        mazeArray[playerPosY - 1, playerPosX] = '@';
                        mazeArray[playerPosY, playerPosX] = ' ';
                    }
                    Refresh();
                    break;
                case Keys.Down:
                    if (mazeArray[playerPosY + 1, playerPosX] == '#')
                        break;
                    else if (mazeArray[playerPosY + 1, playerPosX] == '.')
                    {
                        mazeArray[playerPosY + 1, playerPosX] = '@';
                        mazeArray[playerPosY, playerPosX] = ' ';
                    }
                    else
                    {
                        mazeArray[playerPosY + 1, playerPosX] = '@';
                        mazeArray[playerPosY, playerPosX] = ' ';
                    }
                    Refresh();
                    break;
                case Keys.Right:
                    if (mazeArray[playerPosY, playerPosX + 1] == '#')
                        break;
                    else if (mazeArray[playerPosY, playerPosX + 1] == '.')
                    {
                        mazeArray[playerPosY, playerPosX + 1] = '@';
                        mazeArray[playerPosY, playerPosX] = ' ';
                    }
                    else
                    {
                        mazeArray[playerPosY, playerPosX + 1] = '@';
                        mazeArray[playerPosY, playerPosX] = ' ';
                    }
                    Refresh();
                    break;
                case Keys.Left:
                    if (mazeArray[playerPosY, playerPosX - 1] == '#')
                        break;
                    else if (mazeArray[playerPosY, playerPosX - 1] == '.')
                    {
                        mazeArray[playerPosY, playerPosX - 1] = '@';
                        mazeArray[playerPosY, playerPosX] = ' ';
                    }
                    else
                    {
                        mazeArray[playerPosY, playerPosX - 1] = '@';
                        mazeArray[playerPosY, playerPosX] = ' ';
                    }
                    Refresh();
                    break;
                case Keys.S:
                    if (Computerplayer.BFSfindItem(mazeArray, playerPosX, playerPosY))
                    {
                        if (Computerplayer.BFSfindWay(playerPosX, playerPosY))
                        {
                            // Item gefunden und Weg gefunden - arbeite Stack ab
                          
                        }
                    }
                    break;
                default:
                    break;
                    
            }
        }
    }

    class Computerplayer
    {
        static Queue<Point> queue = new Queue<Point>();
        static Dictionary<Point, Point> dictionary = new Dictionary<Point, Point>();
        public static Stack<Point> stack = new Stack<Point>();


        public static bool BFSfindItem(char[,] array,int xPos, int yPos)
        {
            //Item gefunden
            bool foundItem = false;
            // Füge Spielerposi in leere Queue ein
            Point playerPos = new Point(xPos, yPos);
            queue.Enqueue(playerPos);
            // Solange Schlange nicht leer ist 
            while(queue.Count > 0)
            {
                // hole erstes Paar aus der Queue
                Point firstPoint = queue.Dequeue();
                // falls hier Item, brich ab und weiter mit Phase 2 
                if(array[firstPoint.Y,firstPoint.X] == '.')
                {
                    foundItem = true;
                    // Lege Zielkoordinaten auf Stack - Schritt 2.1
                    stack.Push(firstPoint);
                    break;
                }
                else
                {
                    // oberer Nachbar - entscheide mit Hilfe von CheckNeighbour
                    // if(CheckNeighbour(array, point, direction) -> füge hinzu
                    if(CheckNeighbour(array,firstPoint,'u') )
                    {
                        // Füge Nachbarn in die Queue ein
                        Point pointAbove = new Point(firstPoint.X, firstPoint.Y - 1);
                        queue.Enqueue(pointAbove);
                        //Nachbarn als Key hinzufügen und aktuelle Koordinaten als Value
                        dictionary.Add(pointAbove, firstPoint);
                    
                    }
                    if (CheckNeighbour(array, firstPoint, 'd'))
                    {
                        Point pointBelow = new Point(firstPoint.X, firstPoint.Y + 1);
                        queue.Enqueue(pointBelow);

                        dictionary.Add(pointBelow, firstPoint);
                       
                    }
                    if (CheckNeighbour(array, firstPoint, 'l'))
                    {
                        Point pointLeft = new Point(firstPoint.X - 1, firstPoint.Y);
                        queue.Enqueue(pointLeft);

                        dictionary.Add(pointLeft, firstPoint);
                       
                    }
                    if (CheckNeighbour(array, firstPoint, 'r'))
                    {
                        Point pointRight = new Point(firstPoint.X + 1, firstPoint.Y);
                        queue.Enqueue(pointRight);

                        dictionary.Add(pointRight, firstPoint);
                   
                    }

                }
            }
            // Falls in Phase 1 Item gefunden, weiter mit Phase 2 
            return foundItem;

        }

        // Phase 2 beginnend bei Schritt 2.2, Schritt 2.1 siehe Zeile 213
        // Spielerposi muss übergeben werden
       public static bool BFSfindWay (int xPos, int yPos)
        {
            bool foundWay = false;
            Point playerPos = new Point(xPos, yPos);
            Point from = new Point();
            ICollection keys = dictionary.Keys;

            // Zielposi liegt oben auf Stack, hole Posi 
            Point targetPosi = stack.Pop();
            // Frage ob ZielPosi in Hashtable
            if (dictionary.ContainsKey(targetPosi))
            {
                //ICollection keys = dictionary.Keys;

                foreach(Point key in keys)
                {
                    // lese zugehörigen Value (vorherige Posi) aus Hashtable
                    if(targetPosi.GetHashCode() == key.GetHashCode())
                    {
                        from = dictionary[targetPosi];
                    }
                }
            }
            // Solange from nicht ausgangsposi entspricht
            while (!playerPos.Equals(from))
            {
                // lege from auf Stack
                stack.Push(from);
                foreach(Point key in keys)
                {
                    if(from.GetHashCode() == key.GetHashCode())
                    {
                        //bestimme vorherige Posi und lege diese auf Stapel
                        from = dictionary[from];
                    }
                }

            }
            foundWay = true;
            return foundWay;
        }

        static bool CheckNeighbour(char[,] array, Point point, char direction)
        {
            bool itemAvailable = false;

            switch (direction)
            {
                case 'u':
                 
                    // Setze Point auf Nachbarkoordinaten
                    point.Y = point.Y - 1;
                    // if nicht geblockt und nicht in hashtable
                    if (array[point.Y, point.X] != '#' && !dictionary.ContainsKey(point))
                    {
                        itemAvailable = true;
                    }
                    break;
                case 'd':
                    point.Y = point.Y + 1;
                    if(array[point.Y, point.X] != '#' && !dictionary.ContainsKey(point))
                    {
                        itemAvailable = true;
                    }
                    break;
                case 'l':
                    point.X = point.X - 1;
                    if (array[point.Y, point.X] != '#' && !dictionary.ContainsKey(point))
                    {
                        itemAvailable = true;
                    }

                    break;
                case 'r':
                    point.X = point.X + 1;
                    if(array[point.Y, point.X] != '#' && !dictionary.ContainsKey(point))
                    {
                        itemAvailable = true;
                    }
                    break;
            }

            return itemAvailable;
        }
    }

}