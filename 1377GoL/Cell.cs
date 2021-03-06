﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

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
        public int age { get; set; }
        private Brush _background = Brushes.Black;
        public Brush background
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("background"));
            }
        }
        public virtual List<Cell> neighbors { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public Cell(int x, int y)
        {
            xCoord = x;
            yCoord = y;
        }
    }
}
