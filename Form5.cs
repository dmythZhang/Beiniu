using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beiniu
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            Text = "参数设置";
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Program.FtpEnable;
            textBox1.Text = Program.FtpHost;
            textBox2.Text = Program.FtpUser;
            textBox3.Text = Program.FtpPass;
            textBox4.Text = Program.DataPath;
            textBox5.Text = Program.HtmlPath;
            textBox6.Text = Program.ImagePath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.FtpEnable = checkBox1.Checked;
            Program.FtpHost = textBox1.Text;
            Program.FtpUser = textBox2.Text;
            Program.FtpPass = textBox3.Text;
            Program.DataPath = textBox4.Text;
            Program.HtmlPath = textBox5.Text;
            Program.ImagePath = textBox6.Text;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
