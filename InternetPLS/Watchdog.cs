using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace InternetPLS
{
    internal class Watchdog
    {
        private byte[] buffer;

        private readonly PostLogin login;
        private PingOptions pingOptions;

        private Ping pingSender;
        private const string who = "google.at";

        public Watchdog(PostLogin login)
        {
            this.login = login;
        }

        public void Start()
        {
            pingSender = new Ping();
            pingSender.PingCompleted += PingCompletedCallback;

            var data = "bledbledbledbledbledbledbledbledbled";
            buffer = Encoding.ASCII.GetBytes(data);

            pingOptions = new PingOptions(64, true);

            Send();
        }

        private void Send()
        {
            pingSender.SendAsync(who, 2000, buffer, pingOptions);
        }

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

            Task.Delay(2000).Wait();
            Send();
        }

        public static void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            Console.WriteLine("ping status: {0}", reply.Status);
            if (reply.Status == IPStatus.Success) Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
        }
    }
}