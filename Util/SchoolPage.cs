using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beiniu
{
    class SchoolPage : UserControl
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
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(375, 325);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.label1.Location = new System.Drawing.Point(15, 340);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(345, 50);
            this.label1.TabIndex = 1;
            this.label1.Text = "主题";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label2.Location = new System.Drawing.Point(17, 400);
            this.label2.MaximumSize = new System.Drawing.Size(345, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "内容";
            // 
            // SchoolPage
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "SchoolPage";
            this.Size = new System.Drawing.Size(375, 600);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public SchoolPage()
        {
            InitializeComponent();
            label1.ForeColor = Color.FromArgb(0x11, 0x18, 0x20);
            label2.ForeColor = Color.FromArgb(0xa4, 0x83, 0x5f);
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
    }
}
