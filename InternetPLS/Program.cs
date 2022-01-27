#region usings

using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

#endregion

namespace InternetPLS
{
    internal class Program
    {
        #region Constants and Fields

        private const string CredentialsFile = "creds.dat";

        #endregion

        private static void Main(string[] args)
        {
            VisibilityManager.HideWindow();
            NotifyIconManager.Setup();
            LoginData loginData;

            if (File.Exists(CredentialsFile))
            {
                string[] data = Base64Utils.FromBase64(File.ReadAllText(CredentialsFile)).Split(';');

                loginData = new LoginData()
                {
                    Username = Base64Utils.FromBase64(data[0]),
                    Password = Base64Utils.FromBase64(data[1])
                };
            }
            else
            {
                VisibilityManager.ShowWindow();
                string username = ConsoleUtils.Prompt("Username > ");
                string pw = ConsoleUtils.SecretPrompt("Password > ");

                loginData = new LoginData()
                {
                    Username = username,
                    Password = pw
                };

                username = Base64Utils.ToBase64(username);
                pw = Base64Utils.ToBase64(pw);
                string content = Base64Utils.ToBase64($"{username};{pw}");
                File.WriteAllText(CredentialsFile, content);
            }

            IPAddress htlgkrAddress;

            while (true)
            {
                htlgkrAddress = FindHtlAddress();
                if (htlgkrAddress != null)
                {
                    break;
                }
                Console.WriteLine("HTL network not found. Retrying in 5s ...");
                VisibilityManager.ShowWindow();
                Thread.Sleep(5000);
            }

            VisibilityManager.HideWindow();
            
            var login = new PostLogin(loginData, htlgkrAddress);
            login.Login();

            var watchdog = new Watchdog(login);
            watchdog.Start();

            Application.Run();
        }

        private static IPAddress FindHtlAddress()
        {
            NetworkInterface htlgkrInterface = null;
            IPAddress htlgkrAddress = null;

            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                string dnsSuffix = networkInterface.GetIPProperties().DnsSuffix;
                if (dnsSuffix.Equals("htl.grieskirchen.local") && networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    htlgkrInterface = networkInterface;
                    break;
                }
            }

            if (htlgkrInterface == null)
                return null;

            foreach (UnicastIPAddressInformation ipInfo in htlgkrInterface.GetIPProperties().UnicastAddresses)
            {
                if (ipInfo.Address.IsIPv6LinkLocal) continue;
                htlgkrAddress = ipInfo.Address;
                Console.WriteLine($"Using network: {ipInfo.Address}");
            }

            return htlgkrAddress;
        }
    }
}
