using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

namespace Breitensuche
{
    /// <summary>
    /// Class Breitensuche allows you to walk through a maze, while gathering items.
    /// </summary>
    class Breitensuche : Form
    {
        char[,] mazeArray;
        int playerPosX = 0, playerPosY = 0;
        Timer timerBFS = new Timer();
        bool readyToGather = false; 
    

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Breitensuche.Breitensuche"/> class.
        /// </summary>
        public Breitensuche()
        {
            Width = 600;
            Height = 600;
            Text = "Breitensuche";
            ResizeRedraw = true;
            mazeArray = GetInput();
            timerBFS.Interval = 400;
            timerBFS.Tick += new EventHandler(TimerTick);
        }

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        static void Main()
        {
            Application.Run(new Breitensuche());
        }

        /// <summary>
        /// Fires the Paint-Event and allows using the graphics-object. 
        /// Calculates the spacing between the wallbricks and items. 
        /// Calls a method to draw the maze.
        /// </summary>
        /// <param name="e">E.</param>
        override protected void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // To identify the visible area.
            RectangleF bounds = e.Graphics.VisibleClipBounds; 
            
            Font mazeFont = new Font("Arial", 12);

            // Calculating the spacing between wallbricks and items.
            float spaceX = bounds.Width / (float)(mazeArray.GetUpperBound(1) + 1);
            float spaceY = bounds.Height / (float)(mazeArray.GetUpperBound(0) + 1);
            // Draws the maze. 
            DrawMaze(e.Graphics, mazeArray, mazeFont, spaceX, spaceY);
        }


        /// <summary>
        /// Reads input from a file.
        /// Converts the input into a 2d-chararray.
        /// </summary>
        /// <returns>The input as a 2d-chararray.</returns>
        private char[,] GetInput()
        {

            string line = "";
            int counter = 0, i = 0;

            // Creating the array, depending on the first two lines, which describe 
            // how many lines and columns the array will have. 
            int columns = Convert.ToInt16(Console.ReadLine());
            int lines = Convert.ToInt16(Console.ReadLine());
            char[,] charArray = new char[lines, columns];

            while (counter <= charArray.GetUpperBound(0))
            {
                // Read the string 
                line = Console.ReadLine();
                // Copy each char into the 2d-chararray
                foreach (char c in line)
                {
                    //line = counter, column = i 
                    charArray[counter, i] = c;
                    i++;
                }
                // Set the startindex for the next string - next line at column 0
                counter++;
                i = 0;
            } 

            return charArray;
        }

        /// <summary>
        /// Draws the maze.
        /// </summary>
        /// <param name="g">The green component.</param>
        /// <param name="array">Array.</param>
        /// <param name="font">Font.</param>
        /// <param name="spacingX">Spacing x.</param>
        /// <param name="spacingY">Spacing y.</param>
        private void DrawMaze(Graphics g, char[,] array, Font font, float spacingX, float spacingY)
        {
            // Create two loops to draw the maze
            // Use different color based on the char
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
                            // Saving the actual player position to navigate in the maze
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

        /// <summary>
        /// Uses the KeyDown-Event for checking whether ArrowUp, ArrowDown, ArrowLeft or ArrowRight is pressed.
        /// Starts the automatically BFS by detecting key 'S' was pressed. 
        /// </summary>
        /// <param name="e">E.</param>
        override protected void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                // At first set the timer for the automatic BFS to false, so the player can interrupt 
                // the computerplayer at any time. 
                // Check if there is a wall (break here - nothing more to do), if not there might be a item - set the player (@) to the new
                // position and delete (writing 'SPACE') the last position.
                case Keys.Up:
                    timerBFS.Enabled = false;
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
                    timerBFS.Enabled = false;
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
                    timerBFS.Enabled = false;
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
                    timerBFS.Enabled = false;
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
                    // Starts the Computerplayer and enables the timer, so the timer can fire its tick event.
                case Keys.S:
                    StartBFS();
                    timerBFS.Enabled = true;
                    break;
                default:
                    break;
                    
            }
        }
        /// <summary>
        /// If the Computerplayer shall walk through the maze this method works with the class Computerplayer,
        /// that implements a BFS to navigate and gather all items automatically. 
        /// <seealso cref="T:Computerplayer.BFSfindItem(char[,], int, int)"/>
        /// <seealso cref="T:Computerplayer.BFSfindWay(int, int)"/>
        /// </summary>
        void StartBFS()
        {
            readyToGather = false;
            // If the Breadth-first search finds an item ...
            if (Computerplayer.BFSfindItem(mazeArray, playerPosX, playerPosY))
            {
                // ... and if the BFS finds a way to this item ...
                if (Computerplayer.BFSfindWay(playerPosX, playerPosY))
                {
                    // ... the Computerplayer is ready to gather the next item.
                    readyToGather = true;
                }
            }
        }

        /// <summary>
        /// Registered method for the TimerBFS.Tick-Event. 
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void TimerTick(object sender, EventArgs e)
        {
            Point routePoint = new Point();
                
            if (readyToGather)
            {
                if(Computerplayer.stack.Count > 0)
                {   
                    // Every time the timer fires the Tick-Event the computerplayer moves one step closer 
                    // to his targetposition, which is the last one on the stack. 
                    // Pop the next coordinates from the stack.
                    // Edit the maze and set the new playerposition
                    // Draw the maze.
                    routePoint = Computerplayer.stack.Pop();
                    mazeArray[routePoint.Y, routePoint.X] = '@';
                    mazeArray[playerPosY, playerPosX] = ' ';
                    Refresh();
                }
                else
                {
                   // If the stack is empty, the computerplayer should have reached his targetposition
                   // and the Breadth-first search is going to find the next item.
                    StartBFS();
                }
            }

        }
    }

    /// <summary>
    /// Implements a Breadth-first search for gathering all items in the maze automatically by a computerplayer.
    /// </summary>
    class Computerplayer
    {
        static Queue<Point> queue = new Queue<Point>();
        static Dictionary<Point, Point> dictionary = new Dictionary<Point, Point>();
        public static Stack<Point> stack = new Stack<Point>();

        /// <summary>
        /// Step 1 of the BFS - finds a Item
        /// </summary>
        /// <returns><c>true</c>, if find item was BFSed, <c>false</c> otherwise.</returns>
        /// <param name="array">Array.</param>
        /// <param name="xPos">X position.</param>
        /// <param name="yPos">Y position.</param>
        public static bool BFSfindItem(char[,] array,int xPos, int yPos)
        {

            bool foundItem = false;
            // Add playerposition to the empty queue
            queue.Clear(); 
            dictionary.Clear();
            Point playerPos = new Point(xPos, yPos);
            queue.Enqueue(playerPos);

            // While queue is not empty... 
            while(queue.Count > 0)
            {
                // ... get first pair of coordinates from the queue
                Point firstPoint = queue.Dequeue();
                // ... if there is an item: break - and go on with phase 2.
                if(array[firstPoint.Y,firstPoint.X] == '.')
                {
                    foundItem = true;
                    // Push the targetposition onto the stack. 
                    stack.Push(firstPoint);
                    break;
                }
                else
                {
                    // If there is a possible item at a neighbourpositon ...
                    if(CheckNeighbour(array,firstPoint,'u') ) // u = upper neighbour
                    {
                        // ... add neighbourposition to the queue ... 
                        Point pointAbove = new Point(firstPoint.X, firstPoint.Y - 1);
                        queue.Enqueue(pointAbove);
                        // ... and add the neighbourposition as a key to the dictionary, add the coordinates of 
                        // the step before as a value to the dictionary.
                        // So in further steps of the algorithm the computerplayer knows, from which coordinates (value)
                        // the checked neighbourcoordinates can be reached (key).
                        dictionary.Add(pointAbove, firstPoint);
                    
                    }
                    if (CheckNeighbour(array, firstPoint, 'd')) // d = down below
                    {
                        Point pointBelow = new Point(firstPoint.X, firstPoint.Y + 1);
                        queue.Enqueue(pointBelow);

                        dictionary.Add(pointBelow, firstPoint);
                       
                    }
                    if (CheckNeighbour(array, firstPoint, 'l')) // l = left neighbour
                    {
                        Point pointLeft = new Point(firstPoint.X - 1, firstPoint.Y);
                        queue.Enqueue(pointLeft);

                        dictionary.Add(pointLeft, firstPoint);
                       
                    }
                    if (CheckNeighbour(array, firstPoint, 'r')) // r = right neighbour
                    {
                        Point pointRight = new Point(firstPoint.X + 1, firstPoint.Y);
                        queue.Enqueue(pointRight);

                        dictionary.Add(pointRight, firstPoint);
                   
                    }

                }
            }
            // If the BFS found an item in this phase, the BFS will go ahead with phase 2. 
            return foundItem;

        }

      /// <summary>
      /// Phase 2 of the BFS-Algorithm - if an item was found in phase 1, now the algorithm
      /// finds the way to this item. 
      /// </summary>
      /// <returns><c>true</c>, if find way was BFSed, <c>false</c> otherwise.</returns>
      /// <param name="xPos">X position.</param>
      /// <param name="yPos">Y position.</param>
       public static bool BFSfindWay (int xPos, int yPos)
        {
            bool foundWay = false;
            Point playerPos = new Point(xPos, yPos);
            Point from = new Point();
            ICollection keys = dictionary.Keys;

            // Get targetposition - which is already on the stack. 
            Point targetPosi = stack.Pop();
            stack.Push(targetPosi);

            // If dictionary contains the targetposition, read the value of the key to set a 
            // Point 'from' - these coordinates are representing the coordinates, where the computerplayer
            // can reach the targetposition from.
            if (dictionary.ContainsKey(targetPosi))
            {
                foreach(Point key in keys)
                {
                    if(targetPosi.GetHashCode() == key.GetHashCode())
                    {
                        from = dictionary[targetPosi];
                    }
                }
            }
            // While the point where the BFS came from, isnt the origin playerposition
            // read more values from the dictionary, which represent waypoints. 
            // So a route of waypoints can be created and pushed onto the stack.
            while (!playerPos.Equals(from))
            {

                stack.Push(from);
                foreach(Point key in keys)
                {
                    if(from.GetHashCode() == key.GetHashCode())
                    {
                        // Add a new waypoint to the stack, to create a way through the maze.
                        from = dictionary[from];
                        break;
                    }
                }

            }
            foundWay = true;
            return foundWay;
        }

        /// <summary>
        /// Checks the neighbourcoordinates for possible avaiable items.
        /// </summary>
        /// <returns><c>true</c>, if neighbour was checked and the coordinates 
        /// are neither blocked nor already in the dictionary </returns>
        /// <param name="array">Array.</param>
        /// <param name="point">Point.</param>
        /// <param name="direction">Direction.</param>
        static bool CheckNeighbour(char[,] array, Point point, char direction)
        {
            bool itemAvailable = false;

           
            switch (direction)
            {
                case 'u':

                    // Set the point to the coordinates to check
                    point.Y = point.Y - 1;
                    // If the point is not blocked by a wall AND is not already in the dictionary ...
                    if (array[point.Y, point.X] != '#' && !dictionary.ContainsKey(point))
                    {
                        // ... there could possibly be an item.
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