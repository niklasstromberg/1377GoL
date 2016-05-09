using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static _1377GoL.MainWindow;

namespace _1377GoL
{
    public static class ExtentionMethods
    {
        // initializes the XAML grid, defining 50x50 size
        public static void InitializeGrid(this Grid grid)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            for (int x = 0; x <= size; x++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int y = 0; y <= size; y++)
                grid.RowDefinitions.Add(new RowDefinition());
        }

        // Populates the grid with rectangles, one for each cell
        public static void PopulateGrid(this Grid grid, List<Cell> Cells)
        {
            foreach (Cell cell in Cells)
            {
                Rectangle r = new Rectangle();
                Grid.SetColumn(r, cell.xCoord);
                Grid.SetRow(r, cell.yCoord);
                grid.Children.Add(r);
                r.Stretch = Stretch.UniformToFill;
                r.DataContext = cell;
                r.Stroke = Brushes.Black;
                r.MouseLeftButtonDown += new MouseButtonEventHandler(RectangleOnClick);
                //r.ToolTip = "x: " + cell.xCoord + " y: " + cell.yCoord;                           // uncomment this line to see a rectangles coordinates in the tooltip
            }
        }

        // If the iteration isn't running, leftclicking a rectangle toggles the isAlive property of the corresponding cell
        public static void RectangleOnClick(object sender, MouseButtonEventArgs e)
        {
            if (!running)
            {
                Cell cell = ((Rectangle)sender).DataContext as Cell;
                cell.isAlive = !cell.isAlive;
            }
        }

        // Counting alive neighbors using the virtual list in the object being passed in
        public static short CountLivingNeighbors(this Cell cell)
        {
            var query = from c in cell.neighbors
                        where c.isAlive == true
                        select c;
            if (query == null)
                return 0;
            else
                return Convert.ToInt16(query.Count());
        }

        // Counts the current number of living cells and returns the value
        public static int CountLivingCells(this List<Cell> list)
        {
            var query = from c in list
                        where c.isAlive == true
                        select c;
            return query.Count();
        }
    }
}
