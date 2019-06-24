using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Robot.Classes.Routines;

namespace Robot.Classes
{
    public class Robot
    {
        public int RowLocation { get; set; }
        public int ColumnLocation { get; set; }
        public RobotDirection Direction { get; set; }
        public string MovementCommands { get; set; }
    }
}
