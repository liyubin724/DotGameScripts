using System.Security.Cryptography;
using System.Text;

namespace Dot.Util
{
    public static class MD5Util
    {
        public static string GetMD5(string str)
        {
            StringBuilder sb = new StringBuilder();
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
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
