#region usings

using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

#endregion

namespace InternetPLS
{
    internal class Program
    {
        private static string FromBase64(string data)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(data));
        }

        private static string ToBase64(string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        private static void Main(string[] args)
        {
            string credsFile = "creds.dat";

            LoginData loginData;

            if (File.Exists(credsFile))
            {
                string[] data = FromBase64(File.ReadAllText(credsFile)).Split(';');

                loginData = new LoginData()
                {
                    Username = FromBase64(data[0]),
                    Password = FromBase64(data[1])
                };
            }
            else
            {
                Console.Write("Username > ");
                string username = Console.ReadLine();
                Console.Write("Password > ");
                string pw = Console.ReadLine();

                loginData = new LoginData()
                {
                    Username = username,
                    Password = pw
                };

                username = ToBase64(username);
                pw = ToBase64(pw);
                string content = ToBase64(username + ";" + pw);
                File.WriteAllText("creds.dat", content);
            }

            NetworkInterface htlgkrInterface = null;
            IPAddress htlgkrAddress = null;
            bool iAmAtHome = false;
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                string dnsSuffix = networkInterface.GetIPProperties().DnsSuffix;
                if (dnsSuffix.Equals("htl.grieskirchen.local"))
                {
                    htlgkrInterface = networkInterface;
                    break;
                }
                else if (dnsSuffix.Equals("wiesinger.local")) iAmAtHome = true;
            }

            if (htlgkrInterface != null)
            {
                foreach (var ipInfo in htlgkrInterface.GetIPProperties().UnicastAddresses)
                {
                    if (ipInfo.Address.IsIPv6LinkLocal) continue;
                    htlgkrAddress = ipInfo.Address;
                    Console.WriteLine($"Using network: {ipInfo.Address}");
                }
            }
            else if (iAmAtHome)
            {
                Console.WriteLine($"--- @HOME MODE ---\n Username: {loginData.Username}\n Password: {loginData.Password}");
            }
            else
            {
                Console.WriteLine("HTL network not found. (Press any key to abort)");
                Console.ReadKey();
                return;
            }

            var login = new PostLogin(loginData, htlgkrAddress);
            var watchdog = new Watchdog(login);
            watchdog.Start();

            Console.ReadKey();
        }
    }
}
