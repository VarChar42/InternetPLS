#region usings

using System;
using System.Text;

#endregion

namespace InternetPLS
{
    public class Base64Utils
    {
        public static string FromBase64(string data)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(data));
        }

        public static string ToBase64(string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }
    }
}
