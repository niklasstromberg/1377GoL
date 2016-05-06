using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace _1377GoL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Properties
        public int iterationCount = 0;
        public List<Cell> theCells = new List<Cell>();
        public static bool running = false;

        public MainWindow()
        {
            CreateTheCells();
            InitializeComponent();
            Area.InitializeGrid();
            Area.PopulateGrid(theCells);
        }

        public List<Cell> FindNeighbors2(Cell cell)
        {
            var query = from c in theCells
                        where c.yCoord == (cell.yCoord + 1) && (c.xCoord > (cell.xCoord - 2) && c.xCoord < (cell.xCoord + 2)) ||
                        c.yCoord == cell.yCoord && (c.xCoord == (cell.xCoord - 1) || c.xCoord == (cell.xCoord + 1)) ||
                        c.yCoord == (cell.yCoord - 1) && (c.xCoord > (cell.xCoord - 2) && c.xCoord < (cell.xCoord + 2))
                        select c;
            List<Cell> neighbors = query.ToList<Cell>();
            return neighbors;
        }


        // Finds the neighbors of the cell that is passed into the method
        public List<Cell> FindNeighbors(Cell cell)
        {
            List<Cell> neighbors = new List<Cell>();

            foreach (Cell c in theCells)
            {
                if (c.yCoord == (cell.yCoord + 1) && (c.xCoord > (cell.xCoord - 2) && c.xCoord < (cell.xCoord + 2)))
                {
                    if (!neighbors.Contains(c))
                        neighbors.Add(c);
                }
                if (c.yCoord == cell.yCoord && (c.xCoord == (cell.xCoord - 1) || c.xCoord == (cell.xCoord + 1)))
                {
                    if (!neighbors.Contains(c))
                        neighbors.Add(c);
                }
                if (c.yCoord == (cell.yCoord - 1) && (c.xCoord > (cell.xCoord - 2) && c.xCoord < (cell.xCoord + 2)))
                {
                    if (!neighbors.Contains(c))
                        neighbors.Add(c);
                }
            }
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
                            IterationLogic(theCells);
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

        // The logic performed per iteration
        public void IterationLogic(List<Cell> nextGen)
        {
            foreach (Cell c in nextGen)
            {
                c.aliveNeighbors = c.CountLivingNeighbors();

                if (!c.isAlive)
                {
                    if (c.aliveNeighbors == 3)
                        c.isAlive = !c.isAlive;
                }
                else
                {
                    if (c.aliveNeighbors < 2 || c.aliveNeighbors > 3)
                        c.isAlive = !c.isAlive;
                    //else if (c.aliveNeighbors > 3)                                       // TA BORT!!!!
                    //    c.isAlive = false;
                }
            }
            iterationCount++;
            TBCounter.Text = "Iteration: " + iterationCount;

            theCells = nextGen;
        }

        // Creates the cells and sets the properties
        public void CreateTheCells()
        {
            theCells.Clear();
            for (int x = 0; x <= 4; x++)
            {
                for (int y = 0; y <= 4; y++)
                {
                    Cell cell = new Cell(x, y) { xCoord = x, yCoord = y, isAlive = false };
                    theCells.Add(cell);
                }
            }
            foreach (Cell cell in theCells)
            {
                cell.neighbors = FindNeighbors2(cell);
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

        //// Changes the value of the corresponding cells isAlive property
        //public void RectangleOnClick(object sender, MouseButtonEventArgs e)                 // TA BORT!!!!
        //{
        //    if (!running)
        //    {
        //        Cell cell = ((Rectangle)sender).DataContext as Cell;                        // TA BORT!!!!
        //        cell.isAlive = !cell.isAlive;
        //    }
        //}

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
    }
}
