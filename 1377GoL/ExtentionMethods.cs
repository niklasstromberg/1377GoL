using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            for (int x = 0; x <= 4; x++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int y = 0; y <= 4; y++)
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
                r.Fill = Brushes.Black;
                r.Stroke = Brushes.Black;
                r.DataContext = cell;
                r.MouseLeftButtonDown += new MouseButtonEventHandler(RectangleOnClick);
            }
        }

        // If the iteration isn't running, leftclicking a rectangle toggles the isAlive property of the corresponding cell
        public static void RectangleOnClick(object sender, MouseButtonEventArgs e)
        {
            if (!running)
            {
                Cell cell = ((Rectangle)sender).DataContext as Cell;
                cell.isAlive = !cell.isAlive;
                //MessageBox.Show(CountLivingNeighbors(cell).ToString());
            }
        }

        // Counting alive neighbors using the virtual list in the object
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
    }
}
