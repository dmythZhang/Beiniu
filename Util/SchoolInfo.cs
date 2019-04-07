using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beiniu
{
    class SchoolInfo : Control
    {
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(120, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 50);
            this.label1.TabIndex = 1;
            this.label1.Text = "学校名称";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.label1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.label2.Location = new System.Drawing.Point(122, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 50);
            this.label2.TabIndex = 2;
            this.label2.Text = "学校简介";
            this.label2.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // SchoolInfo
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.Size = new System.Drawing.Size(375, 120);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        public SchoolInfo()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public string Image
        {
            get { return pictureBox1.ImageLocation; }
            set { pictureBox1.ImageLocation = value; }
        }

        public string Title
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public string Content
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }

        public BorderStyle BorderStyle
        {
            get { return pictureBox1.BorderStyle; }
            set { pictureBox1.BorderStyle = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (BorderStyle == BorderStyle.None)
            { g.DrawLine(Pens.Gray, 120, 110, 360, 110); }
            else
            { g.DrawRectangle(Pens.Gray, 1, 1, 373, 118); }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }
    }
}
