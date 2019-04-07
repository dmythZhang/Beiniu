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
    public partial class Form3 : Form
    {
        public Form3(string text, string value = "", bool canEnter = true)
        {
            InitializeComponent();
            Text = text;
            textBox1.Text = value;
            textBox1.AcceptsReturn = canEnter;
        }

        public string Value { get { return textBox1.Text; } }
    }
}
