using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Beiniu
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        Dictionary<string, string> ftpfiles;

        private void Form4_Load(object sender, EventArgs e)
        {
            try
            {
                ftpfiles = new Dictionary<string, string>();
                string[] folders = FileHelper.GetFolders(FileHelper.htmlPath);
                foreach (string folder in folders)
                {
                    string[] files = FileHelper.GetFiles(FileHelper.htmlPath, folder);
                    foreach (string file in files)
                    {
                        ftpfiles[FileHelper.GetFile(FileHelper.htmlPath, folder, file)] = FileHelper.htmlPath.Replace('\\', '/') + "/" + folder + "/" + file;
                    }
                }
                folders = FileHelper.GetFolders(FileHelper.imagePath);
                foreach (string folder in folders)
                {
                    if (folder == "demo") continue;
                    string[] files = FileHelper.GetFiles(FileHelper.imagePath, folder);
                    foreach (string file in files)
                    {
                        ftpfiles[FileHelper.GetFile(FileHelper.imagePath, folder, file)] = FileHelper.imagePath.Replace('\\', '/') + "/" + folder + "/" + file;
                    }
                }
                progressBar1.Maximum = ftpfiles.Count;
                Text = "资料上传[" + progressBar1.Value + "/" + progressBar1.Maximum + "]";
                if (ftpfiles.Count == 0) button1.Enabled = false;
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
                button1.Enabled = false;
            }
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                if (MessageBox.Show(this, "文件正在上传，是否关闭？", "",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    backgroundWorker1.CancelAsync();
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (backgroundWorker1.IsBusy) return;
            progressBar1.Value = 0;
            backgroundWorker1.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                foreach (string localPath in ftpfiles.Keys)
                {
                    if (backgroundWorker1.CancellationPending) { e.Result = "用户取消"; return; }
                    string remotePath = ftpfiles[localPath];
                    bool result = FtpHelper.UploadFile(localPath, remotePath, s => label1.Text = s);
                    backgroundWorker1.ReportProgress(progressBar1.Value);
                }
                e.Result = "上传完毕";
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
                button1.Enabled = true;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage + 1;
            Text = "资料上传[" + progressBar1.Value + "/" + progressBar1.Maximum + "]";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Text = e.Result.ToString();
            if (!button1.Enabled) button2.Text = "完成";
        }
    }
}
