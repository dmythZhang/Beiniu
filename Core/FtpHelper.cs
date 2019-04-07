using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Beiniu
{
    class FtpHelper
    {
        public static string ftpHost = "127.0.0.1";
        public static string userName = "anonymous";
        public static string passWord = "";

        private static FtpWebRequest CreateRequest(string path, string method)
        {
            string url = ftpHost.TrimEnd('/') + "/" + path.TrimStart('/');
            if (!url.StartsWith("ftp://")) url = "ftp://" + url;
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
            request.Credentials = new NetworkCredential(userName, passWord);
            request.Proxy = null;
            request.EnableSsl = false;
            request.KeepAlive = false;
            request.UseBinary = true;
            request.UsePassive = true;
            request.Method = method;
            return request;
        }

        public static bool UploadFile(string localPath, string remotePath, Action<string> updateProgress = null)
        {
            if (updateProgress != null) updateProgress(remotePath);
            
            int index = remotePath.LastIndexOf('/');
            while (index > 0)
            {
                if (ExistsPath(remotePath.Substring(0, index))) break;
                index = remotePath.LastIndexOf('/', index - 1);
            }
            if (index == 0)
                GetNlst(remotePath.Substring(0, index + 1));
            index = remotePath.IndexOf('/', index + 1);
            while (index > 0)
            {
                CreatePath(remotePath.Substring(0, index));
                index = remotePath.IndexOf('/', index + 1);
            }

            FtpWebRequest ftp = CreateRequest(remotePath, WebRequestMethods.Ftp.UploadFile);
            using (Stream rs = ftp.GetRequestStream())
            {
                using (FileStream fs = File.OpenRead(localPath))
                {
                    long len = 0;
                    byte[] buffer = new byte[1024];
                    int count = fs.Read(buffer, 0, buffer.Length);
                    while (count > 0)
                    {
                        Thread.Sleep(0);
                        rs.Write(buffer, 0, count);
                        len += count;
                        if (updateProgress != null) updateProgress(remotePath + "," + (100 * len / fs.Length) + "%");
                        count = fs.Read(buffer, 0, buffer.Length);
                    }
                }
            }
            return true;
        }

        public static bool CreatePath(string path)
        {
            try
            {
                FtpWebRequest ftp = CreateRequest(path, WebRequestMethods.Ftp.MakeDirectory);
                using (WebResponse response = ftp.GetResponse()) { }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ExistsPath(string path)
        {
            try
            {
                FtpWebRequest ftp = CreateRequest(path, WebRequestMethods.Ftp.ListDirectory);
                using (WebResponse response = ftp.GetResponse()) { }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string[] GetList(string path)
        {
            List<string> list = new List<string>();
            FtpWebRequest ftp = CreateRequest(path, WebRequestMethods.Ftp.ListDirectoryDetails);
            using (WebResponse response = ftp.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        list.Add(line);
                        line = reader.ReadLine();
                    }
                }
            }
            return list.ToArray();
        }

        public static string[] GetNlst(string path)
        {
            List<string> list = new List<string>();
            FtpWebRequest ftp = CreateRequest(path, WebRequestMethods.Ftp.ListDirectory);
            using (WebResponse response = ftp.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        list.Add(line);
                        line = reader.ReadLine();
                    }
                }
            }
            return list.ToArray();
        }
    }
}
