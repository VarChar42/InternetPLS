#region usings

using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace InternetPLS
{
    internal class Watchdog
    {
        #region Constants and Fields

        private const string Who = "1.1.1.1";

        private readonly PostLogin login;
        private byte[] buffer;
        private PingOptions pingOptions;

        private Ping pingSender;

        #endregion

        public Watchdog(PostLogin login)
        {
            this.login = login;
        }

        public static void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            Console.WriteLine("ping status: {0}", reply.Status);
            if (reply.Status == IPStatus.Success) Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
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
            pingSender.SendAsync(Who, 2000, buffer, pingOptions);
        }

        #region Event Handlers

        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            if (e.Cancelled) Console.WriteLine("Ping canceled.");
            var reply = e.Reply;

            if (reply == null || e.Error != null || reply.Status != IPStatus.Success)
            {
                Console.WriteLine("Ping failed:");
                login.Login();
            }

            DisplayReply(reply);

            Task.Delay(1000).Wait();
            Send();
        }

        #endregion
    }
}
