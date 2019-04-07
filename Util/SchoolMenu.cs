using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beiniu
{
    class SchoolMenu : UserControl
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SchoolMenu
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.Name = "SchoolMenu";
            this.Size = new System.Drawing.Size(375, 600);
            this.Load += new System.EventHandler(this.SchoolMenu_Load);
            this.Click += new System.EventHandler(this.SchoolMenu_Click);
            this.ResumeLayout(false);

        }

        public SchoolMenu()
        {
            InitializeComponent();
        }

        public SchoolInfo FocusedSchool
        {
            get
            {
                if (focusedIndex < 0) return null;
                SchoolInfo info = Controls[focusedIndex] as SchoolInfo;
                return info;
            }
        }
        public event EventHandler FocusedIndexChanged;

        public int focusedIndex = -1;

        public void Add(string folder, bool focused, bool refresh)
        {
            SchoolInfo info = new SchoolInfo();
            info.Click += SchoolInfo_Click;
            info.Location = new Point(0, Controls.Count == 0 ? 0 : Controls[Controls.Count - 1].Top + Controls[Controls.Count - 1].Height);
            info.Anchor |= AnchorStyles.Right;
            info.Width = ClientSize.Width;
            info.TabIndex = Controls.Count;
            info.TabStop = false;
            info.Title = folder;
            info.Content = string.Join(Environment.NewLine, FileHelper.ReadFile(FileHelper.htmlPath, folder, "0.html"));
            info.Image = FileHelper.GetFile(FileHelper.imagePath, folder, "0.jpg");
            Controls.Add(info);

            if (focused) focusedIndex = Controls.Count - 1;
            if (refresh) ShowRefresh();
        }

        public void Remove()
        {
            if (focusedIndex < 0) return;
            Controls.RemoveAt(focusedIndex);

            if (focusedIndex == Controls.Count)
                focusedIndex--;
            else for (int index = focusedIndex; index < Controls.Count; index++)
                { Controls[index].Top -= 120; Controls[index].TabIndex--; }

            ShowRefresh();
        }

        public void ShowRefresh()
        {
            for (int index = 0; index < Controls.Count; index++)
            {
                SchoolInfo info = Controls[index] as SchoolInfo;
                if (focusedIndex == index)
                    info.BorderStyle = BorderStyle.FixedSingle;
                else
                    info.BorderStyle = BorderStyle.None;
            }
            if (FocusedSchool != null)
                ScrollControlIntoView(FocusedSchool);
            if (FocusedIndexChanged != null)
                FocusedIndexChanged(this, new EventArgs());
            GC.Collect();
        }

        private void SchoolInfo_Click(object sender, EventArgs e)
        {
            SchoolInfo info = sender as SchoolInfo;
            if (info == null) return;
            int index = Controls.IndexOf(info);
            if (index == focusedIndex)
                index = -1;
            focusedIndex = index;
            ShowRefresh();
        }

        private void SchoolMenu_Click(object sender, EventArgs e)
        {
            focusedIndex = -1;
            ShowRefresh();
        }

        private void SchoolMenu_Load(object sender, EventArgs e)
        {
            foreach (string folder in FileHelper.GetFolders(FileHelper.htmlPath))
            {
                Add(folder, false, false);
            }
            focusedIndex = -1;
            ShowRefresh();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (Controls.Count > 0)
            {
                if (keyData == Keys.Up || keyData == (Keys.Shift | Keys.Tab))
                {
                    focusedIndex--;
                    if (focusedIndex == -1) focusedIndex = Controls.Count - 1;
                    ShowRefresh();
                    return true;
                }
                if (keyData == Keys.Down || keyData == Keys.Tab)
                {
                    focusedIndex++;
                    if (focusedIndex == Controls.Count) focusedIndex = 0;
                    ShowRefresh();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
