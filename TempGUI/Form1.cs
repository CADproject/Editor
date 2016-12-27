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
    public partial class MyCAE : Form
    {
        public MyCAE()
        {
            InitializeComponent();

            treeView2.ExpandAll();
            
            aline = new a_line();
            propertyGrid1.SelectedObject = aline;
            /*
            aline.type = "Отрезок";
            aline.id = 15;
            aline.number = 2;
            aline.length = 3.75;
            aline.angle = 32;
            aline.color = "Черный";
            aline.thickness = 1;
            aline.x1 = 2.76;
            aline.y1 = -0.1;
            aline.z1 = 1.7;
            aline.x2 = -3.8;
            aline.y2 = 5.43;
            aline.z2 = -9.54;
            */
            
            /*
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();

            dataGridView1.Rows[0].Cells[0].Value = "Тип";
            dataGridView1.Rows[0].Cells[1].Value = "Отрезок";

            dataGridView1.Rows[1].Cells[0].Value = "id";
            dataGridView1.Rows[1].Cells[1].Value = 15;

            dataGridView1.Rows[2].Cells[0].Value = "Число узлов";
            dataGridView1.Rows[2].Cells[1].Value = 2;

            dataGridView1.Rows[3].Cells[0].Value = "Узел #1";
            dataGridView1.Rows[3].Cells[1].Value = "x:           12.35";
            dataGridView1.Rows[4].Cells[1].Value = "y:          -3.54";
            dataGridView1.Rows[5].Cells[1].Value = "z:           0.21";

            dataGridView1.Rows[6].Cells[0].Value = "Узел #2";
            dataGridView1.Rows[6].Cells[1].Value = "x:           17.11";
            dataGridView1.Rows[7].Cells[1].Value = "y:          -23.7";
            dataGridView1.Rows[8].Cells[1].Value = "z:           0.89";

            dataGridView1.Rows[9].Cells[0].Value = "Длина";
            dataGridView1.Rows[9].Cells[1].Value = 37.95;

            dataGridView1.Rows[10].Cells[0].Value = "Угол";
            dataGridView1.Rows[10].Cells[1].Value = "12'";

            dataGridView1.Rows[11].Cells[0].Value = "Цвет";
            dataGridView1.Rows[11].Cells[1].Value = "черный";

            dataGridView1.Rows[12].Cells[0].Value = "Толщина";
            dataGridView1.Rows[12].Cells[1].Value = 1;

            for (int i = 0; i <= 12; i++)
            {
                dataGridView1.Rows[i].Cells[0].Style.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold);

                dataGridView1.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            */
        }

        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserControl1 line_control = new UserControl1();
            this.panel3.Controls.Add(line_control);
        }

        private void точкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            point_UserControl point_control = new point_UserControl();
            this.panel3.Controls.Add(point_control);
        }

        private void ломанаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wire_UserControl wire_control = new wire_UserControl();
            this.panel3.Controls.Add(wire_control);
        }

        private void прямоугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rect_UserControl rect_control = new rect_UserControl();
            this.panel3.Controls.Add(rect_control);
        }

        private void окружностьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            circle_UserControl circle_control = new circle_UserControl();
            this.panel3.Controls.Add(circle_control);
        }

        private void дугаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            arc_UserControl arc_control = new arc_UserControl();
            this.panel3.Controls.Add(arc_control);
        }

        private void сплайнToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spline_UserControl spline_control = new spline_UserControl();
            this.panel3.Controls.Add(spline_control);
        }

        private void триммированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trim_UserControl trim_control = new trim_UserControl();
            this.panel3.Controls.Add(trim_control);
        }

        private void продлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long_UserControl obj = new long_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void пеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            move_UserControl obj = new move_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copy_UserControl obj = new copy_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void отобразитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mirrow_UserControl obj = new mirrow_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void повернутьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rotate_UserControl obj = new rotate_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void масштабироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            size_UserControl obj = new size_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void материалToolStripMenuItem_Click(object sender, EventArgs e)
        {
            material_UserControl obj = new material_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void свойстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            property_UserControl obj = new property_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void сеткаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mesh_UserControl obj = new mesh_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void закреплениеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            support_UserControl obj = new support_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void кинематическоеНагружениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kinema_sup_UserControl obj = new kinema_sup_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void нагрузкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            load_UserControl obj = new load_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void системыКоординатToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CS_UserControl obj = new CS_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void единицыИзмеренияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            units obj = new units();
            obj.Show();
        }

        private void линейноеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            linear_dist_UserControl obj = new linear_dist_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void угловоеToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            angle_UserControl obj = new angle_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CS_UserControl obj = new CS_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            point_UserControl obj = new point_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            UserControl1 obj = new UserControl1();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem9_Click(object sender, EventArgs e)
        {
            circle_UserControl obj = new circle_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem10_Click(object sender, EventArgs e)
        {
            arc_UserControl obj = new arc_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem11_Click(object sender, EventArgs e)
        {
            spline_UserControl obj = new spline_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            material_UserControl obj = new material_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            property_UserControl obj = new property_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            mesh_UserControl obj = new mesh_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem12_Click(object sender, EventArgs e)
        {
            kinema_sup_UserControl obj = new kinema_sup_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem13_Click(object sender, EventArgs e)
        {
            support_UserControl obj = new support_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            load_UserControl obj = new load_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void модельToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            model_modify_UserControl obj = new model_modify_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void центрВращенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            center_of_rotate obj = new center_of_rotate();
            obj.Show();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem9_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem10_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem11_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem12_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem13_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem14_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void параметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calculation_UserControl obj = new calculation_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void анализToolStripMenuItem_Click(object sender, EventArgs e)
        {
            analyse_UserControl obj = new analyse_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void редактироватьToolStripMenuItem14_Click(object sender, EventArgs e)
        {
            calculation_UserControl obj = new calculation_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void удалитьToolStripMenuItem15_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void результатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            solutions obj = new solutions();
            obj.Show();
        }

        private void показатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            show_UserControl obj = new show_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void общиеToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            settings obj = new settings();
            obj.Show();
        }

        private void освободитьСтепеньСвободыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            release_UserControl obj = new release_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            UserControl1 obj = new UserControl1();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            wire_UserControl obj = new wire_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            rect_UserControl obj = new rect_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            circle_UserControl obj = new circle_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            arc_UserControl obj = new arc_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            spline_UserControl obj = new spline_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            delete_UserControl obj = new delete_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            trim_UserControl obj = new trim_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            long_UserControl obj = new long_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            move_UserControl obj = new move_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            copy_UserControl obj = new copy_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            rotate_UserControl obj = new rotate_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            mirrow_UserControl obj = new mirrow_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            size_UserControl obj = new size_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton6_Click_2(object sender, EventArgs e)
        {
            point_UserControl obj = new point_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            mesh_UserControl obj = new mesh_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton50_Click(object sender, EventArgs e)
        {
            kinema_sup_UserControl obj = new kinema_sup_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton23_Click(object sender, EventArgs e)
        {
            support_UserControl obj = new support_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton24_Click(object sender, EventArgs e)
        {
            load_UserControl obj = new load_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton31_Click(object sender, EventArgs e)
        {
            analyse_UserControl obj = new analyse_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton32_Click(object sender, EventArgs e)
        {
            solutions obj = new solutions();
            obj.Show();
        }

        private void toolStripButton25_Click(object sender, EventArgs e)
        {
            CS_UserControl obj = new CS_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton52_Click(object sender, EventArgs e)
        {
            units obj = new units();
            obj.Show();
        }

        private void toolStripButton30_Click(object sender, EventArgs e)
        {
            linear_dist_UserControl obj = new linear_dist_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton51_Click(object sender, EventArgs e)
        {
            angle_UserControl obj = new angle_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton20_Click(object sender, EventArgs e)
        {
            model_modify_UserControl obj = new model_modify_UserControl();
            this.panel3.Controls.Add(obj);
        }

        private void toolStripButton54_Click(object sender, EventArgs e)
        {
            rotate_cs obj = new rotate_cs();
            obj.Show();
        }

        private void toolStripButton55_Click(object sender, EventArgs e)
        {
            rotate_cs obj = new rotate_cs();
            obj.Show();
        }

        private void toolStripButton56_Click(object sender, EventArgs e)
        {
            rotate_cs obj = new rotate_cs();
            obj.Show();
        }

        private void toolStripButton40_Click(object sender, EventArgs e)
        {
            center_of_rotate obj = new center_of_rotate();
            obj.Show();
        }

    }

}
