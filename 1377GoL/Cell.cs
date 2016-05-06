using System.Collections.Generic;
using System.ComponentModel;

namespace _1377GoL
{
    public class Cell : INotifyPropertyChanged
    {
        private bool _isAlive = false;
        public bool isAlive
        {
            get
            {
                return _isAlive;
            }
            set
            {
                _isAlive = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("isAlive"));
            }
        }
        public int xCoord { get; set; }
        public int yCoord { get; set; }

        public short aliveNeighbors { get; set; }

        public virtual List<Cell> neighbors { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public Cell(int x, int y)
        {
            xCoord = x;
            yCoord = y;
        }
    }
}
