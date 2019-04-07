using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beiniu
{
    public partial class Form2 : Form
    {
        public Form2(string text)
        {
            InitializeComponent();
            Tag = Text = text;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            pageCount = 0;
            while (FileHelper.CheckFile(FileHelper.htmlPath, Tag.ToString(), (pageCount + 1) + ".html")) { pageCount++; }
            if (pageCount == 0) { AddPage(); }
            else { pageIndex = 0; ShowPage(); }
        }

        public int pageIndex, pageCount;

        public void AddPage()
        {
            pageCount++;
            pageIndex = pageCount - 1;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<title>school</title>");
            sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            sb.AppendLine("<style type=\"text/css\">");
            sb.AppendLine("<!--.img-group { position: relative;}");
            sb.AppendLine(".img-tip {position:absolute; top:12px; width:100%; text-align:center;letter-spacing:0px;}");
            sb.AppendLine(".img-tip1 {position:absolute; top:30px; width:100%; left:20px; letter-spacing:0px;}");
            sb.AppendLine(".img-tip2 {position:absolute; top:3px; width:100%; left:20px; letter-spacing:0px;line-height:66px}");
            sb.AppendLine("-->");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body bgcolor=\"#F0EFF4\">");
            sb.AppendLine("");
            sb.AppendLine("<!-- Save for Web Slices (SMEAG123.psd) -->");
            sb.AppendLine("<table id=\"__01\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">");
            sb.AppendLine("	<tr>");
            sb.AppendLine("		<td> <div class=\"img-group\">  <img src=\"../../uploads\\photo\\APP\\" + Tag.ToString() + "\\" + (pageIndex + 1) + ".jpg\" width=\"100%\" /><div class=\"img-tip\"></div></div> </td>");
            sb.AppendLine("	</tr>");
            sb.AppendLine("	<tr>");
            sb.AppendLine("		<td> <div class=\"img-group\">  <img src=\"../../uploads\\photo\\APP\\demo\\3.jpg\" width=\"100%\" /><div class=\"img-tip1\">");
            sb.AppendLine("		<font face=\"微软雅黑\" size=\"9\" color=\"#111820\"><strong>无主题</strong></font>");
            sb.AppendLine("		</div></div> </td>");
            sb.AppendLine("	</tr>");
            sb.AppendLine("	<tr>");
            sb.AppendLine("		<td> <div class=\"img-group\">  <img src=\"../../uploads\\photo\\APP\\demo\\4.jpg\" width=\"100%\" /><div class=\"img-tip2\">");
            sb.AppendLine("		<font face=\"微软雅黑\" size=\"6\" color=\"#a4835f\">");
            sb.AppendLine("");
            sb.AppendLine("内容1</br>");
            sb.AppendLine("内容2</br>");
            sb.AppendLine("内容3</br>");
            sb.AppendLine("		</font>");
            sb.AppendLine("		</div></div> </td>");
            sb.AppendLine("	</tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            FileHelper.WriteFile(FileHelper.htmlPath, Tag.ToString(), (pageIndex + 1) + ".html", sb.ToString());
            ShowPage();
        }

        public void ShowPage()
        {
            Text = Tag + "[" + (pageIndex + 1) + "/" + pageCount + "]";
            schoolPage1.Image = FileHelper.GetFile(FileHelper.imagePath, Tag.ToString(), (pageIndex + 1) + ".jpg");
            string[] contents = FileHelper.ReadFile(FileHelper.htmlPath, Tag.ToString(), (pageIndex + 1) + ".html");
            List<string> content = new List<string>();
            string title = "";
            foreach (string s in contents)
            {
                if (s.EndsWith("</strong></font>"))
                {
                    int index1 = s.IndexOf("<strong>");
                    int index2 = s.IndexOf("</strong>");
                    if (index1 < 0 || index2 < 0 || index2 < index1 + 8) continue;
                    title = s.Substring(index1, index2 - index1).Substring(8);
                }
                if (s.EndsWith("</br>"))
                {
                    content.Add(s.Replace("</br>", ""));
                }
            }
            schoolPage1.Content = string.Join("\r\n", content.ToArray());
            schoolPage1.Title = title;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pageIndex == 0) return;
            pageIndex--;
            ShowPage();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pageIndex == pageCount - 1) AddPage();
            else
            {
                pageIndex++;
                ShowPage();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "是否确认删除该页面？", "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            FileHelper.DeleteFile(FileHelper.imagePath, Tag.ToString(), (pageIndex + 1) + ".jpg");
            FileHelper.DeleteFile(FileHelper.htmlPath, Tag.ToString(), (pageIndex + 1) + ".html");
            pageCount--;
            if (pageCount == 0) AddPage();
            else
            {
                if (pageIndex == pageCount) pageIndex--;
                else
                {
                    for (int i = pageIndex; i < pageCount; i++)
                    {
                        FileHelper.MoveFile(FileHelper.imagePath, Tag.ToString(), (i + 1) + ".jpg", (i + 2) + ".jpg");
                        FileHelper.MoveFile(FileHelper.htmlPath, Tag.ToString(), (i + 1) + ".html", (i + 2) + ".html");

                        string[] contents = FileHelper.ReadFile(FileHelper.htmlPath, Tag.ToString(), (i + 1) + ".html");
                        for (int j = 0; j < contents.Length; j++)
                        {
                            if (contents[j].Contains(".jpg"))
                            {
                                contents[j] = "		<td> <div class=\"img-group\">  <img src=\"../../uploads\\photo\\APP\\" + Tag.ToString() + "\\" + (i + 1) + ".jpg\" width=\"100%\" /><div class=\"img-tip\"></div></div> </td>";
                                break;
                            }
                        }
                        StringBuilder sb = new StringBuilder();
                        foreach (string s in contents)
                        {
                            sb.AppendLine(s);
                        }
                        FileHelper.WriteFile(FileHelper.htmlPath, Tag.ToString(), (i + 1) + ".html", sb.ToString());
                    }
                }
                ShowPage();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try { Process.Start(FileHelper.GetFile(FileHelper.htmlPath, Tag.ToString(), (pageIndex + 1) + ".html")); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog obj = new OpenFileDialog();
            obj.Filter = "图片文件|*.jpg|所有文件|*.*";
            if (obj.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap img = new Bitmap(obj.FileName);
                    if (img.Width != 750 || img.Height != 650)
                        throw new Exception("图片格式必须为750*650");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                FileHelper.CopyFile(FileHelper.imagePath, Tag.ToString(), (pageIndex + 1) + ".jpg", obj.FileName);
                ShowPage();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form3 obj = new Form3("编写主题", schoolPage1.Title, false);
            if (obj.ShowDialog() == DialogResult.OK)
            {
                string[] contents = FileHelper.ReadFile(FileHelper.htmlPath, Tag.ToString(), (pageIndex + 1) + ".html");

                for (int index = 0; index < contents.Length; index++)
                {
                    if (contents[index].Contains("<strong>"))
                    {
                        contents[index] = "		<font face=\"微软雅黑\" size=\"9\" color=\"#111820\"><strong>" + obj.Value + "</strong></font>";
                        break;
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (string s in contents)
                {
                    sb.AppendLine(s);
                }
                FileHelper.WriteFile(FileHelper.htmlPath, Tag.ToString(), (pageIndex + 1) + ".html", sb.ToString());
                ShowPage();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form3 obj = new Form3("编写内容", schoolPage1.Content, true);
            if (obj.ShowDialog() == DialogResult.OK)
            {
                string[] newContent = obj.Value.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
                string[] contents = FileHelper.ReadFile(FileHelper.htmlPath, Tag.ToString(), (pageIndex + 1) + ".html");

                StringBuilder sb = new StringBuilder();
                foreach (string s in contents)
                {
                    if (s.EndsWith("</br>"))
                        break;
                    sb.AppendLine(s);
                }
                foreach (string s in newContent)
                {
                    decimal count = 0;
                    int index1 = 0, index2 = 0;
                    foreach (char c in s)
                    {
                        if (c > 127) count += 1; else count += 0.5M;
                        if (count > 20)
                        {
                            if (c > 127) count = 1; else count = 0.5M;
                            sb.AppendLine(s.Substring(index1, index2 - index1) + "</br>");
                            index1 = index2;
                        }
                        index2++;
                    }
                    if (index2 > index1)
                        sb.AppendLine(s.Substring(index1, index2 - index1) + "</br>");
                }
                sb.AppendLine("		</font>");
                sb.AppendLine("		</div></div> </td>");
                sb.AppendLine("	</tr>");
                sb.AppendLine("</table>");
                sb.AppendLine("</body>");
                sb.AppendLine("</html>");
                FileHelper.WriteFile(FileHelper.htmlPath, Tag.ToString(), (pageIndex + 1) + ".html", sb.ToString());
                ShowPage();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
