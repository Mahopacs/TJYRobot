using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Robot.Classes
{
    public static class Routines
    {
        static int robotStartingRow;
        static int robotStartingCol;
        static string robotStartingDirection;
        static string robotMovementCommands;

        static int gMainGridNumberOfRows;
        static int gMainGridNumberOfColumns;
        public static object[,] gMainGrid;
        public static Robot gRobot = new Robot();
        public static string gRobotTraversalPath;
        public static string gGridObstacles;

        public enum RobotDirection { Forward, Back, Left, Right }
        public enum MovementDirection { Left, Right, Forward }

        public static void LoadAppConfigSettings()
        {
            robotStartingRow = Convert.ToInt32(ConfigurationManager.AppSettings["robot_starting_row"]);
            robotStartingCol = Convert.ToInt32(ConfigurationManager.AppSettings["robot_starting_col"]);
            robotStartingDirection = ConfigurationManager.AppSettings["robot_starting_direction"].ToUpper();
            robotMovementCommands = ConfigurationManager.AppSettings["robot_movement_commands"].ToString();

            gMainGridNumberOfRows = Convert.ToInt32(ConfigurationManager.AppSettings["grid_number_of_rows"]);
            gMainGridNumberOfColumns = Convert.ToInt32(ConfigurationManager.AppSettings["grid_number_of_cols"]);
        }

        public static void Initialize_Grid()
        {
            gMainGrid = new object[gMainGridNumberOfRows, gMainGridNumberOfColumns];
        }

        public static void Initialize_Robot()
        {
            gRobot.RowLocation = robotStartingRow;
            gRobot.ColumnLocation = robotStartingCol;
            gRobot.MovementCommands = robotMovementCommands;

            switch (robotStartingDirection)
            {
                case "FORWARD":
                    gRobot.Direction = RobotDirection.Forward;
                    break;
                case "BACK":
                    gRobot.Direction = RobotDirection.Back;
                    break;
                case "LEFT":
                    gRobot.Direction = RobotDirection.Left;
                    break;
                case "RIGHT":
                    gRobot.Direction = RobotDirection.Right;
                    break;
                default:
                    gRobot.Direction = RobotDirection.Forward;
                    break;
            }

            gRobotTraversalPath = "ROBOT START LOCATION: row/col " + robotStartingRow + "/" + robotStartingCol +
            " (direction= " + gRobot.Direction + ")" + Environment.NewLine +
            "ROBOT SERIES OF COMMANDS: '" + gRobot.MovementCommands + "'" + Environment.NewLine;

            gRobotTraversalPath = gRobotTraversalPath + Environment.NewLine + "GRID OBSTACLES: " + Environment.NewLine + gGridObstacles + Environment.NewLine;
            gRobotTraversalPath = gRobotTraversalPath + "ROBOT MOVEMENTS:" + Environment.NewLine;          
        }
        
        public static void ProcessRobotMovements()
        {
            Char[] letters = gRobot.MovementCommands.ToUpper().ToCharArray();

            foreach (char item in letters)
            {
                switch (item)
                {
                    case 'L':
                        MoveLeft();
                        break;
                    case 'R':
                        MoveRight();
                        break;
                    case 'F':
                        MoveForward();
                        break;
                    default:
                        break;
                }
            }

            gRobotTraversalPath = gRobotTraversalPath + Environment.NewLine + "ROBOT END LOCATION: col/row " +
                gRobot.RowLocation + "/" + gRobot.ColumnLocation + " (direction= " + gRobot.Direction + ")" +
                Environment.NewLine;
        }

        public static void MoveLeft()
        {
            int newRow = 0;
            int newCol = 0;

            switch (gRobot.Direction)
            {
                case RobotDirection.Forward:
                    newRow = gRobot.RowLocation;
                    newCol = gRobot.ColumnLocation - 1;
                    break;
                case RobotDirection.Back:
                    newRow = gRobot.RowLocation;
                    newCol = gRobot.ColumnLocation + 1;
                    break;
                case RobotDirection.Left:
                    newRow = gRobot.RowLocation + 1;
                    newCol = gRobot.ColumnLocation;
                    break;
                case RobotDirection.Right:
                    newRow = gRobot.RowLocation - 1;
                    newCol = gRobot.ColumnLocation;
                    break;
            }

            ProcessRobotTryingToMoveToNewGridLocation(MovementDirection.Left, newRow, newCol);
        }

        public static void MoveRight()
        {
            int newRow = 0;
            int newCol = 0;

            switch (gRobot.Direction)
            {
                case RobotDirection.Forward:
                    newRow = gRobot.RowLocation;
                    newCol = gRobot.ColumnLocation + 1;
                    break;
                case RobotDirection.Back:
                    newRow = gRobot.RowLocation;
                    newCol = gRobot.ColumnLocation - 1;
                    break;
                case RobotDirection.Left:
                    newRow = gRobot.RowLocation - 1;
                    newCol = gRobot.ColumnLocation;
                    break;
                case RobotDirection.Right:
                    newRow = gRobot.RowLocation + 1;
                    newCol = gRobot.ColumnLocation;
                    break;
            }

            ProcessRobotTryingToMoveToNewGridLocation(MovementDirection.Right, newRow, newCol);
        }

        public static void MoveForward()
        {
            int newRow = 0;
            int newCol = 0;

            switch (gRobot.Direction)
            {
                case RobotDirection.Forward:
                    newRow = gRobot.RowLocation - 1;
                    newCol = gRobot.ColumnLocation;
                    break;
                case RobotDirection.Back:
                    newRow = gRobot.RowLocation + 1;
                    newCol = gRobot.ColumnLocation;
                    break;
                case RobotDirection.Left:
                    newRow = gRobot.RowLocation;
                    newCol = gRobot.ColumnLocation - 1;
                    break;
                case RobotDirection.Right:
                    newRow = gRobot.RowLocation;
                    newCol = gRobot.ColumnLocation + 1;
                    break;
            }

            ProcessRobotTryingToMoveToNewGridLocation(MovementDirection.Right, newRow, newCol);
        }

        //This routine is used to set grids obstructions
        public static void Initialize_Obstructions()
        {
            //Initialize grid obsctructions to spaces
            for (int row = 0; row < gMainGridNumberOfRows; row++)
            {
                for (int col = 0; col < gMainGridNumberOfColumns; col++)
                {
                    gMainGrid[row, col] = " ";
                }
            }

            //Set the Rocks
            gMainGrid[0, 2] = "R";
            gMainGrid[0, 1] = "R";
            gMainGrid[1, 3] = "R";
            gMainGrid[2, 1] = "R";

            //Set the Spinners
            gMainGrid[1, 0] = 1;
            gMainGrid[1, 4] = 1;

            //Set the Holes
            Hole objHole = new Hole(3, 4);
            Hole objHole2 = new Hole(2, 3);

            gMainGrid[0, 0] = objHole;
            gMainGrid[3, 3] = objHole2;

            //Create an unknown obstacle
            gMainGrid[4, 1] = "Z";
        }

        public static void ProcessRobotTryingToMoveToNewGridLocation(MovementDirection movementDirection, int row, int col)
        {
            string movementString = string.Empty;
            RobotDirection originalDirection = gRobot.Direction;

            if ((row < 0) || (col < 0))
            {
                movementString = "Robot (facing direction " + gRobot.Direction + ") attempted to move " + movementDirection +
                          " from row/col " + gRobot.RowLocation + "/" + gRobot.ColumnLocation +
                          " to row/col " + row + "/" + col + " but did not because this would have put the robot of the grid. " + Environment.NewLine;

                gRobot.RowLocation = gRobot.RowLocation;
                gRobot.ColumnLocation = gRobot.ColumnLocation;
            }
            else
            {
                Type type = gMainGrid[row, col].GetType();

                if (type == typeof(Hole))
                {
                    Hole objHole = new Hole(0, 0);
                    objHole = (Hole)gMainGrid[row, col];

                    movementString = "Robot (facing direction " + gRobot.Direction + ") moved from row/col " + gRobot.RowLocation + "/" + gRobot.ColumnLocation +
                      " to row/col " + row + "/" + col + " (by moving " + movementDirection + ") where it encountered a HOLE that brought it " +
                      " to row/col " + objHole.RowLocation + "/" + objHole.ColumnLocation + "." + Environment.NewLine;

                    gRobot.RowLocation = objHole.RowLocation;
                    gRobot.ColumnLocation = objHole.ColumnLocation;
                }
                else if (type == typeof(Int32))
                {
                    //This is a Spinner, so move rotate the robots direction and move to the new space        
                    SpinTheRobotAndRotateItsDirection((int)gMainGrid[row, col]);

                    movementString = "Robot (facing direction " + originalDirection + ") moved from row/col " + gRobot.RowLocation + "/" + gRobot.ColumnLocation +
                           " to row/col " + row + "/" + col + " (by moving " + movementDirection + "). " +
                           "The robot also changed rotation to direction " + gRobot.Direction + " because the location it moved to was a Spinner obstalce with paramter value " + (int)gMainGrid[row, col] + "." +
                           Environment.NewLine;

                    gRobot.RowLocation = row;
                    gRobot.ColumnLocation = col;
                }
                else if (type == typeof(string))
                {
                    //This is a rock so can not move there
                    if (gMainGrid[row, col].ToString() == "R")
                    {
                        movementString = "Robot (facing direction " + gRobot.Direction + ") attempted to move " + movementDirection +
                        " from row/col " + gRobot.RowLocation + "/" + gRobot.ColumnLocation +
                        " to row/col " + row + "/" + col + " but did not because this grid location contained a 'Rock' obstacle. " + Environment.NewLine;

                        gRobot.RowLocation = gRobot.RowLocation;
                        gRobot.ColumnLocation = gRobot.ColumnLocation;
                    }
                    else if (gMainGrid[row, col].ToString() == " ")
                    {
                        //There is no obstacle so robot can move to this new location
                        movementString = "Robot (facing direction " + gRobot.Direction + ") moved from row/col " + gRobot.RowLocation + "/" + gRobot.ColumnLocation +
                            " to row/col " + row + "/" + col + " (by moving " + movementDirection + ")." + Environment.NewLine;

                        gRobot.RowLocation = row;
                        gRobot.ColumnLocation = col;
                    }
                    else
                    {
                        //This is an unknown obstacle so can not move there
                        movementString = "Robot (facing direction " + gRobot.Direction + ") attempted to move " + movementDirection +
                           " from row/col " + gRobot.RowLocation + "/" + gRobot.ColumnLocation +
                           " to row/col " + row + "/" + col + " but did not because this grid location contained an unknown obstacle " +
                           "(" + gMainGrid[row, col].ToString() + ")." + Environment.NewLine;

                        gRobot.RowLocation = gRobot.RowLocation;
                        gRobot.ColumnLocation = gRobot.ColumnLocation;
                    }
                }
            }

            gRobotTraversalPath = gRobotTraversalPath + Environment.NewLine + movementString;
        }

        //Rotate Robots direction 90 degress for each value (i.e. 1 = 90 degrees, 2 = 180 degrees, 3 = 270 degrees, 4 = 360 degrees)
        //Assumption is rotation is always clockwise direction
        public static void SpinTheRobotAndRotateItsDirection(int rotationParameter)
        {
            switch (rotationParameter)
            {
                case 1:
                    if (gRobot.Direction == RobotDirection.Forward) { gRobot.Direction = RobotDirection.Right; break; }
                    if (gRobot.Direction == RobotDirection.Left) { gRobot.Direction = RobotDirection.Forward; break; }
                    if (gRobot.Direction == RobotDirection.Right) { gRobot.Direction = RobotDirection.Back; break; }
                    if (gRobot.Direction == RobotDirection.Back) { gRobot.Direction = RobotDirection.Left; break; }
                    break;
                case 2:
                    if (gRobot.Direction == RobotDirection.Forward) { gRobot.Direction = RobotDirection.Back; break; }
                    if (gRobot.Direction == RobotDirection.Left) { gRobot.Direction = RobotDirection.Right; break; }
                    if (gRobot.Direction == RobotDirection.Right) { gRobot.Direction = RobotDirection.Left; break; }
                    if (gRobot.Direction == RobotDirection.Back) { gRobot.Direction = RobotDirection.Forward; break; }
                    break;
                case 3:
                    if (gRobot.Direction == RobotDirection.Forward) { gRobot.Direction = RobotDirection.Left; break; }
                    if (gRobot.Direction == RobotDirection.Left) { gRobot.Direction = RobotDirection.Back; break; }
                    if (gRobot.Direction == RobotDirection.Right) { gRobot.Direction = RobotDirection.Forward; break; }
                    if (gRobot.Direction == RobotDirection.Back) { gRobot.Direction = RobotDirection.Right; break; }
                    break;
                case 4:
                    if (gRobot.Direction == RobotDirection.Forward) { gRobot.Direction = RobotDirection.Forward; break; }
                    if (gRobot.Direction == RobotDirection.Left) { gRobot.Direction = RobotDirection.Left; break; }
                    if (gRobot.Direction == RobotDirection.Right) { gRobot.Direction = RobotDirection.Right; break; }
                    if (gRobot.Direction == RobotDirection.Back) { gRobot.Direction = RobotDirection.Back; break; }
                    break;
                default:
                    break;
            }
        }

        public static void GetGridsObstructions()
        {
            for (int row = 0; row < gMainGridNumberOfRows; row++)
            {
                for (int col = 0; col < gMainGridNumberOfColumns; col++)
                {
                    Type type = gMainGrid[row, col].GetType();

                    if (type == typeof(Hole))
                    {
                        Hole objHole = new Hole(0, 0);
                        objHole = (Hole)gMainGrid[row, col];

                        gGridObstacles = gGridObstacles + "row/col " + row + "/" + col + ">> HOLE to row/col " + objHole.RowLocation + "/" + objHole.ColumnLocation + Environment.NewLine;
                    }
                    else if (type == typeof(Int32))
                    {
                        gGridObstacles = gGridObstacles + "row/col " + row + "/" + col + ">> SPINNER (" + (int)gMainGrid[row, col] + ")" + Environment.NewLine;
                    }
                    else if (type == typeof(string))
                    {
                        //This is a rock so can not move there
                        if (gMainGrid[row, col].ToString() == "R")
                        {

                            gGridObstacles = gGridObstacles + "row/col " + row + "/" + col + ">> ROCK" + Environment.NewLine;
                        }
                        else if (gMainGrid[row, col].ToString() == " ")
                        {

                        }
                        else
                        {
                            //This is an unknown obstacle so can not move there
                            gGridObstacles = gGridObstacles + "row/col " + row + "/" + col + ">> UNKNOWN (" + gMainGrid[row, col].ToString() + ")" + Environment.NewLine;
                        }
                    }
                }
            }
        }
    }
}
