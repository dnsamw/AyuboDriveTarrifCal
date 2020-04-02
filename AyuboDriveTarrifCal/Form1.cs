using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AyuboDriveTarrifCal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            userControls.UC_HireRent ucHireRent = new userControls.UC_HireRent();
            AddControlsToPanel(ucHireRent);
        }


        private void AddControlsToPanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            panelControls.Controls.Clear();
            panelControls.Controls.Add(c);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            userControls.UC_HireRent ucHireRent = new userControls.UC_HireRent();
            AddControlsToPanel(ucHireRent);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            userControls.UC_TarrifCal ucHireRent = new userControls.UC_TarrifCal();
            AddControlsToPanel(ucHireRent);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            userControls.UC_CompletedTrips ucHireRent = new userControls.UC_CompletedTrips();
            AddControlsToPanel(ucHireRent);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            userControls.UC_Manage ucHireRent = new userControls.UC_Manage();
            AddControlsToPanel(ucHireRent);
        }


    }
}
