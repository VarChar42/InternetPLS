using System;
using System.Net;
using System.Net.NetworkInformation;

namespace InternetPLS
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            NetworkInterface htlgkrInterface = null;
            IPAddress htlgkrAddress = null;
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                if (networkInterface.GetIPProperties().DnsSuffix.Equals("htl.grieskirchen.local"))
                {
                    htlgkrInterface = networkInterface;
                    break;
                }

            foreach (var ipInfo in htlgkrInterface.GetIPProperties().UnicastAddresses)
            {
                if (ipInfo.Address.IsIPv6LinkLocal) continue;
                htlgkrAddress = ipInfo.Address;
                Console.WriteLine(ipInfo.Address);
            }

            if (htlgkrAddress == null)
            {
                Console.WriteLine("HTL network not found.");
                Console.ReadKey();
                return;
            }
           

            var data = LoginData.Read().Decrypt();

            

            var login = new PostLogin(data, htlgkrAddress);
            
            //login.Login();

            var watchdog = new Watchdog(login);
            watchdog.Start();


            Console.ReadKey();
        
        }
    }
}