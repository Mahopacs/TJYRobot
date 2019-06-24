using Robot.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Robot
{
    public partial class Form1 : Form
    {
      
        public Form1()
        {
            InitializeComponent();
            Routines.LoadAppConfigSettings();
            Routines.Initialize_Grid();
            Routines.Initialize_Obstructions();
            Routines.GetGridsObstructions();
            Routines.Initialize_Robot();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Routines.ProcessRobotMovements();
            this.txtRobotTraversalPath.Text = Routines.gRobotTraversalPath;
        }            
    }
}
