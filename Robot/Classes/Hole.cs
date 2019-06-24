using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Classes
{
    class Hole
    {
        public int RowLocation { get; set; }
        public int ColumnLocation { get; set; }

        public Hole(int row, int col)
        {
            this.RowLocation = row;
            this.ColumnLocation = col;
        }
    }
}
