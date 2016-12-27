using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyCAE_GUI_prototype
{
    public partial class CrossSection : Form
    {
        public CrossSection()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new                  System.ComponentModel.ComponentResourceManager(typeof(CrossSection));

            if (this.comboBox1.SelectedIndex == 0 )
            {
                this.panel1.BackgroundImage = global::MyCAE_GUI_prototype.Properties.Resources.circle_1;
            }
            else if( this.comboBox1.SelectedIndex == 1 )
            {
                this.panel1.BackgroundImage = global::MyCAE_GUI_prototype.Properties.Resources.tube_2;
            }
            else if (this.comboBox1.SelectedIndex == 2)
            {
                this.panel1.BackgroundImage = global::MyCAE_GUI_prototype.Properties.Resources.rect_3;
            }
            else if (this.comboBox1.SelectedIndex == 3)
            {
                this.panel1.BackgroundImage = global::MyCAE_GUI_prototype.Properties.Resources.O_4;
            }
            else if (this.comboBox1.SelectedIndex == 4)
            {
                this.panel1.BackgroundImage = global::MyCAE_GUI_prototype.Properties.Resources.L_5;
            }
            else if (this.comboBox1.SelectedIndex == 5)
            {
                this.panel1.BackgroundImage = global::MyCAE_GUI_prototype.Properties.Resources.T_6;
            }
            else if (this.comboBox1.SelectedIndex == 6)
            {
                this.panel1.BackgroundImage = global::MyCAE_GUI_prototype.Properties.Resources.I_7;
            }
            else if (this.comboBox1.SelectedIndex == 7)
            {
                this.panel1.BackgroundImage = global::MyCAE_GUI_prototype.Properties.Resources.Z_8;
            }
            else if (this.comboBox1.SelectedIndex == 8)
            {
                this.panel1.BackgroundImage = global::MyCAE_GUI_prototype.Properties.Resources.U_9;
            }
            else if (this.comboBox1.SelectedIndex == 9)
            {
                this.panel1.BackgroundImage = global::MyCAE_GUI_prototype.Properties.Resources.X_10;
            }
        }
    }
}
