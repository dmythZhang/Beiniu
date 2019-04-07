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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "资料上传";
            button3.Visible = Program.FtpEnable;
        }

        private void schoolMenu1_FocusedIndexChanged(object sender, EventArgs e)
        {
            if (schoolMenu1.FocusedSchool == null)
            {
                button2.Enabled = false;
                button4.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
            }
            else
            {
                button2.Enabled = true;
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 obj = new Form3("学校名称", "", false);
            if (obj.ShowDialog() == DialogResult.OK)
            {
                string folder = obj.Value.Trim();
                if (string.IsNullOrEmpty(folder))
                { MessageBox.Show(this, "学校名称不能为空！"); return; }
                if (FileHelper.CheckFolder(FileHelper.htmlPath, folder) || FileHelper.CheckFolder(FileHelper.imagePath, folder)
                    || folder.StartsWith(".") || folder.Contains("|") || folder.Contains("/") || folder.Contains("\\")
                    || folder.Contains("<") || folder.Contains(">") || folder.Contains("*") || folder.Contains("?")
                    || folder.Contains(":") || folder.Contains("\""))
                { MessageBox.Show(this, "学校已经存在或名称不可用！"); return; }
                FileHelper.WriteFile(FileHelper.htmlPath, folder, "0.html");
                schoolMenu1.Add(folder, true, true);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "是否确认删除学校：" + schoolMenu1.FocusedSchool.Title + "？", "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            FileHelper.DeleteFolder(FileHelper.htmlPath, schoolMenu1.FocusedSchool.Title);
            FileHelper.DeleteFolder(FileHelper.imagePath, schoolMenu1.FocusedSchool.Title);
            schoolMenu1.Remove();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 obj = new Form4();
            if (obj.ShowDialog() == DialogResult.OK)
            {
                GC.Collect();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog obj = new OpenFileDialog();
            obj.Filter = "图片文件|*.jpg|所有文件|*.*";
            if (obj.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap img = new Bitmap(obj.FileName);
                    if (img.Width != img.Height)
                        throw new Exception("图片格式必须为1:1，默认800*800");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                FileHelper.CopyFile(FileHelper.imagePath, schoolMenu1.FocusedSchool.Title, "0.jpg", obj.FileName);
                schoolMenu1.FocusedSchool.Image = FileHelper.GetFile(FileHelper.imagePath, schoolMenu1.FocusedSchool.Title, "0.jpg");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 obj = new Form3("学校简介", schoolMenu1.FocusedSchool.Content, false);
            if (obj.ShowDialog() == DialogResult.OK)
            {
                schoolMenu1.FocusedSchool.Content = obj.Value;
                FileHelper.WriteFile(FileHelper.htmlPath, schoolMenu1.FocusedSchool.Title, "0.html", obj.Value);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form2 obj = new Form2(schoolMenu1.FocusedSchool.Title);
            if (obj.ShowDialog() == DialogResult.OK)
            {
                GC.Collect();
            }
        }
    }
}
