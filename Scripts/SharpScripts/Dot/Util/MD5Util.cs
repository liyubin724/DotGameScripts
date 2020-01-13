using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dot.Util
{
    public static class MD5Util
    {
        public static string GetMD5(string str)
        {
            return GetMD5(Encoding.UTF8.GetBytes(str));
        }

        public static string GetFileMD5(string filePath)
        {
            if(!File.Exists(filePath))
            {
                return string.Empty;
            }
            byte[] bytes = File.ReadAllBytes(filePath);
            return GetMD5(bytes);
        }

        public static string GetMD5(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] data = md5.ComputeHash(bytes);
                int length = data.Length;
                for (int i = 0; i < length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}
