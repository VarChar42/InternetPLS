#region usings

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.PropertyGridInternal;

#endregion

namespace InternetPLS
{
    internal class Program
    {
        private const string CredentialsFile = "creds.dat";
        private static void Main(string[] args)
        {
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

            NetworkInterface? htlgkrInterface = null;
            IPAddress? htlgkrAddress = null;

            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                string dnsSuffix = networkInterface.GetIPProperties().DnsSuffix;
                if (dnsSuffix.Equals("htl.grieskirchen.local") && networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    htlgkrInterface = networkInterface;
                    break;
                }
            }

            if (htlgkrInterface != null)
            {
                foreach (UnicastIPAddressInformation ipInfo in htlgkrInterface.GetIPProperties().UnicastAddresses)
                {
                    if (ipInfo.Address.IsIPv6LinkLocal) continue;
                    htlgkrAddress = ipInfo.Address;
                    Console.WriteLine($"Using network: {ipInfo.Address}");
                }
            }
            else
            {
                Console.WriteLine("HTL network not found. (Press any key to abort)");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("Press enter to abort.");

            var login = new PostLogin(loginData, htlgkrAddress!);
            
            login.Login();

            var watchdog = new Watchdog(login);
            watchdog.Start();
            
            Application.Run();
        }


    }
}