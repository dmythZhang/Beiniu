using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Beiniu
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            try { LoadConfig(); } catch { }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Contains("-config"))
            {
                if (new Form5().ShowDialog() == DialogResult.OK)
                    SaveConfig();
            }
            else Application.Run(new Form1());
        }

        static byte[] AesEncrypt(string value)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(value);
            RijndaelManaged rm = new RijndaelManaged()
            {
                Key = Guid.NewGuid().ToByteArray(),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform crypto = rm.CreateEncryptor();
            return rm.Key.Concat(crypto.TransformFinalBlock(buffer, 0, buffer.Length)).ToArray();
        }

        static string AesDecrypt(byte[] buffer)
        {
            if (buffer.Length < 16) return null;
            RijndaelManaged rm = new RijndaelManaged()
            {
                Key = buffer.Take(16).ToArray(),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform crypto = rm.CreateDecryptor();
            byte[] result = crypto.TransformFinalBlock(buffer, 16, buffer.Length - 16);
            return Encoding.UTF8.GetString(result);
        }
        
        const string config = "config.dat";

        static void LoadConfig()
        {
            if (!File.Exists(config)) return;
            byte[] buffer = File.ReadAllBytes(config);
            string value = AesDecrypt(buffer);
            if (string.IsNullOrWhiteSpace(value)) return;
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(value);
            if (xml.GetElementsByTagName("FtpEnable").Count > 0)
                if (!string.IsNullOrWhiteSpace(xml.GetElementsByTagName("FtpEnable")[0].InnerText))
                    FtpEnable = Convert.ToBoolean(xml.GetElementsByTagName("FtpEnable")[0].InnerText);
            if (xml.GetElementsByTagName("FtpHost").Count > 0)
                if (!string.IsNullOrWhiteSpace(xml.GetElementsByTagName("FtpHost")[0].InnerText))
                    FtpHost = xml.GetElementsByTagName("FtpHost")[0].InnerText;
            if (xml.GetElementsByTagName("FtpUser").Count > 0)
                if (!string.IsNullOrWhiteSpace(xml.GetElementsByTagName("FtpUser")[0].InnerText))
                    FtpUser = xml.GetElementsByTagName("FtpUser")[0].InnerText;
            if (xml.GetElementsByTagName("FtpPass").Count > 0)
                if (!string.IsNullOrWhiteSpace(xml.GetElementsByTagName("FtpPass")[0].InnerText))
                    FtpPass = xml.GetElementsByTagName("FtpPass")[0].InnerText;
            if (xml.GetElementsByTagName("DataPath").Count > 0)
                if (!string.IsNullOrWhiteSpace(xml.GetElementsByTagName("DataPath")[0].InnerText))
                    DataPath = xml.GetElementsByTagName("DataPath")[0].InnerText;
            if (xml.GetElementsByTagName("HtmlPath").Count > 0)
                if (!string.IsNullOrWhiteSpace(xml.GetElementsByTagName("HtmlPath")[0].InnerText))
                    HtmlPath = xml.GetElementsByTagName("HtmlPath")[0].InnerText;
            if (xml.GetElementsByTagName("ImagePath").Count > 0)
                if (!string.IsNullOrWhiteSpace(xml.GetElementsByTagName("ImagePath")[0].InnerText))
                    ImagePath = xml.GetElementsByTagName("ImagePath")[0].InnerText;
        }

        static void SaveConfig()
        {
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement root = xml.CreateElement("config");
            root.AppendChild(xml.CreateElement("FtpEnable")).InnerText = FtpEnable.ToString();
            root.AppendChild(xml.CreateElement("FtpHost")).InnerText = FtpHost;
            root.AppendChild(xml.CreateElement("FtpUser")).InnerText = FtpUser;
            root.AppendChild(xml.CreateElement("FtpPass")).InnerText = FtpPass;
            root.AppendChild(xml.CreateElement("DataPath")).InnerText = DataPath;
            root.AppendChild(xml.CreateElement("HtmlPath")).InnerText = HtmlPath;
            root.AppendChild(xml.CreateElement("ImagePath")).InnerText = ImagePath;
            xml.AppendChild(root);
            byte[] buffer = AesEncrypt(xml.InnerXml);
            File.WriteAllBytes(config, buffer);
        }

        public static bool FtpEnable { get; set; }
        public static string FtpHost { get { return FtpHelper.ftpHost; } set { FtpHelper.ftpHost = value; } }
        public static string FtpUser { get { return FtpHelper.userName; } set { FtpHelper.userName = value; } }
        public static string FtpPass { get { return FtpHelper.passWord; } set { FtpHelper.passWord = value; } }
        public static string DataPath { get { return FileHelper.dataPath; } set { FileHelper.dataPath = value; } }
        public static string HtmlPath { get { return FileHelper.htmlPath; } set { FileHelper.htmlPath = value; } }
        public static string ImagePath { get { return FileHelper.imagePath; } set { FileHelper.imagePath = value; } }
    }
}
