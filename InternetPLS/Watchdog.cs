#region usings

using System;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace InternetPLS
{
    internal class Watchdog
    {
        #region Constants and Fields

        private const string Who = "8.8.8.8";

        private double averagePing = -1;
        private readonly PostLogin login;
        private byte[] buffer;
        private PingOptions pingOptions;

        private Ping pingSender;

        #endregion

        public Watchdog(PostLogin login)
        {
            this.login = login;
        }

        public void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            Console.WriteLine("ping status: {0}", reply.Status);

            if (averagePing < 0)
            {
                averagePing = reply.RoundtripTime;
            }
            
            averagePing = (averagePing * 10 + reply.RoundtripTime) / 11;
            
            if (reply.Status == IPStatus.Success) Console.WriteLine("RoundTrip time: {0} ms Average: {1} ms", reply.RoundtripTime, Math.Round(averagePing, 4));
        }

        public void Start()
        {
            pingSender = new Ping();
            pingSender.PingCompleted += PingCompletedCallback;

            var data = "4242";
            buffer = Encoding.ASCII.GetBytes(data);

            pingOptions = new PingOptions(64, true);

            Send();
        }

        private void Send()
        {
            pingSender.SendAsync(Who, 1000, buffer, pingOptions);
        }

        #region Event Handlers

        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            if (e.Cancelled) Console.WriteLine("Ping canceled.");
            PingReply? reply = e.Reply;

            if (reply == null || e.Error != null || reply.Status != IPStatus.Success)
            {
                Console.WriteLine("Ping failed! Logging in...");
                login.Login();
            }

            DisplayReply(reply);

            Task.Delay(1000).Wait();
            Send();
        }

        #endregion
    }
}
