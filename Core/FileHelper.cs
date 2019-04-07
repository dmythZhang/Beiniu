using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Beiniu
{
    class FileHelper
    {
        public static string dataPath = Application.StartupPath;
        public static string htmlPath = "\\html";
        public static string imagePath = "\\uploads\\photo\\APP";

        public static string GetFolder(string subPath, string folder)
        {
            if (Path.IsPathRooted(folder)) return folder;
            return dataPath + subPath + "\\" + folder;
        }

        public static string GetFile(string subPath, string folder, string file)
        {
            if (Path.IsPathRooted(file)) return file;
            return dataPath + subPath + "\\" + folder + "\\" + file;
        }

        public static string[] GetFolders(string subPath, string filter = "*")
        {
            CreateFolder(subPath);
            List<string> folder = new List<string>();
            foreach (string path in Directory.GetDirectories(dataPath + subPath, filter))
                folder.Add(Path.GetFileName(path));
            return folder.ToArray();
        }

        public static string[] GetFiles(string subPath, string folder, string filter = "*")
        {
            CreateFolder(subPath, folder);
            List<string> file = new List<string>();
            foreach (string path in Directory.GetFiles(dataPath + subPath + "\\" + folder, filter))
                file.Add(Path.GetFileName(path));
            return file.ToArray();
        }

        public static bool CheckFolder(string subPath, string folder)
        {
            string path = GetFolder(subPath, folder);
            return Directory.Exists(path);
        }

        public static bool CheckFile(string subPath, string folder, string file)
        {
            string path = GetFile(subPath, folder, file);
            return File.Exists(path);
        }

        public static void DeleteFolder(string subPath, string folder)
        {
            string path = GetFolder(subPath, folder);
            if (CheckFolder(subPath, folder))
                Directory.Delete(path, true);
        }

        public static void DeleteFile(string subPath, string folder, string file)
        {
            string path = GetFile(subPath, folder, file);
            if (CheckFile(subPath, folder, file))
                File.Delete(path);
        }

        public static void CreateFolder(string subPath, string folder = "")
        {
            string path = GetFolder(subPath, folder);
            if (!CheckFolder(subPath, folder))
                Directory.CreateDirectory(path);
        }

        public static string[] ReadFile(string subPath, string folder, string file)
        {
            string path = GetFile(subPath, folder, file);
            if (CheckFile(subPath, folder, file))
                return File.ReadAllLines(path);
            else
                return new string[0];
        }

        public static void WriteFile(string subPath, string folder, string file, string value = "")
        {
            CreateFolder(subPath, folder);
            file = GetFile(subPath, folder, file);
            File.WriteAllText(file, value);
        }

        public static void CopyFile(string subPath, string folder, string file, string value)
        {
            CreateFolder(subPath, folder);
            file = GetFile(subPath, folder, file);
            value = GetFile(subPath, folder, value);
            if (File.Exists(value))
                File.Copy(value, file, true);
        }

        public static void MoveFile(string subPath, string folder, string file, string value)
        {
            CreateFolder(subPath, folder);
            file = GetFile(subPath, folder, file);
            value = GetFile(subPath, folder, value);
            if (File.Exists(value))
                File.Move(value, file);
        }
    }
}
