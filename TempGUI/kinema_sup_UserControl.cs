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
    public partial class kinema_sup_UserControl : UserControl
    {
        public kinema_sup_UserControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            set_support obj = new set_support();
            obj.Show();
        }
    }
}
