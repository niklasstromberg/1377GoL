using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace _1377GoL
{
    public partial class MainWindow : Window
    {
        // Properties
        public int iterationCount = 0;
        public List<Cell> theCells = new List<Cell>();
        public static bool running = false;
        public static int size = 49;

        public MainWindow()
        {
            CreateTheCells();
            InitializeComponent();
            Area.InitializeGrid();
            Area.PopulateGrid(theCells);
        }

        // Finds and populates the passed cells neighbors to its list
        public List<Cell> FindNeighbors(Cell cell)
        {
            var query = from c in theCells
                        where c.yCoord == (cell.yCoord + 1) && ((c.xCoord > (cell.xCoord - 2) && c.xCoord < (cell.xCoord + 2))) ||  // bottom row
                        c.yCoord == cell.yCoord && ((c.xCoord == (cell.xCoord - 1) || c.xCoord == (cell.xCoord + 1))) ||            // same row
                        c.yCoord == (cell.yCoord - 1) && ((c.xCoord > (cell.xCoord - 2) && c.xCoord < (cell.xCoord + 2)))           // top row
                        select c;
            List<Cell> neighbors = query.ToList<Cell>();
            return neighbors;
        }

        // Calls DrawIteration indefinately with a 0.1 second delay
        public void Iteration(bool start)
        {
            if (start)
            {
                Task.Run(() =>
                {
                    while (running)
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            IterationLogic();
                        }));
                        Thread.Sleep(100);
                    }
                });
            }
            else
            {
                // Do nothing
            }
        }

        // The logic performed per iteration, using a temporary list to work with
        public void IterationLogic()
        {
            List<NewCell> nextGen = new List<NewCell>();

            foreach (Cell c in theCells)
            {
                c.aliveNeighbors = c.CountLivingNeighbors();
                bool result = false;

                if (c.isAlive && c.aliveNeighbors < 2)                                      // cell is lonely
                    result = false;
                else if (c.isAlive && (c.aliveNeighbors == 2 || c.aliveNeighbors == 3))     // cell is happy
                    result = true;
                else if (c.isAlive && c.aliveNeighbors > 3)                                 // cell is crowded
                    result = false;
                else if (!c.isAlive && c.aliveNeighbors == 3)                               // cell is resurrected
                    result = true;

                NewCell newCell = new NewCell();
                newCell.x = c.xCoord;
                newCell.y = c.yCoord;
                newCell.lives = result;
                nextGen.Add(newCell);                                                       // store data about the next generation
            }

            foreach (Cell c in theCells)                                                   // Move data from next generation to current generation
            {                                                                               // (the binding does the rest)
                var query = from cell in nextGen
                            where cell.x == c.xCoord && cell.y == c.yCoord
                            select cell.lives;
                c.isAlive = query.FirstOrDefault();
            }
            iterationCount++;
            TBCounter.Text = "Iteration: " + iterationCount;
        }

        // Creates the cells and sets the properties
        public void CreateTheCells()
        {
            theCells.Clear();
            for (int y = 0; y <= size; y++)
            {
                for (int x = 0; x <= size; x++)
                {
                    Cell cell = new Cell(x, y) { xCoord = x, yCoord = y, isAlive = false };
                    theCells.Add(cell);
                }
            }
            foreach (Cell cell in theCells)
            {
                cell.neighbors = FindNeighbors(cell);
            }
        }

        // Stops the iterations and resets the board
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            running = false;
            BtnStart.Content = "Start";
            iterationCount = 0;
            TBCounter.Text = "Iteration: " + iterationCount;
            CreateTheCells();
            Area.InitializeGrid();
            Area.PopulateGrid(theCells);
        }

        // Starts and pauses the iterations
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!running)
            {
                running = true;
                BtnStart.Content = "Pause";
                Iteration(running);
            }
            else
            {
                BtnStart.Content = "Start";
                running = false;
                Iteration(running);
            }
        }

        // holds next generation values until iteration is finished
        struct NewCell
        {
            public int x;
            public int y;
            public bool lives;
        }

        // Runs IterationLogic() and performs one iteration per click of the button
        private void BtnStep_Click(object sender, RoutedEventArgs e)
        {
            IterationLogic();
        }
    }
}

