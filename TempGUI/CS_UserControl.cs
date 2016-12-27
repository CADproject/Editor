using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyCAE_GUI_prototype
{
    public partial class CS_UserControl : UserControl
    {
        public CS_UserControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rotate_cs obj = new rotate_cs();
            obj.Show();
        }
    }
}
