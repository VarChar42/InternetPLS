#region usings

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

#endregion

namespace InternetPLS
{
    internal class PostLogin
    {
        #region Constants and Fields

        private const string Url = "http://10.10.0.251:8002/?zone=cp_htl";

        private readonly HttpClient client;
        private readonly LoginData loginData;

        #endregion

        public PostLogin(LoginData login, IPAddress addr)
        {
            loginData = login;
            client = GetHttpClient(addr);
        }

        public static HttpClient GetHttpClient(IPAddress address)
        {
            if (IPAddress.Any.Equals(address))
                return new HttpClient();

            var handler = new SocketsHttpHandler
            {
                ConnectCallback = async (context, cancellationToken) =>
                {
                    var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

                    socket.Bind(new IPEndPoint(address, 0));

                    socket.NoDelay = true;

                    try
                    {
                        await socket.ConnectAsync(context.DnsEndPoint, cancellationToken).ConfigureAwait(false);

                        return new NetworkStream(socket, true);
                    }
                    catch
                    {
                        socket.Dispose();

                        throw;
                    }
                }
            };

            return new HttpClient(handler);
        }

        public void Login()
        {
            var values = new Dictionary<string, string>
            {
                {"auth_user", loginData.Username},
                {"auth_pass", loginData.Password},
                {"accept", "Anmelden"}
            };
            var content = new FormUrlEncodedContent(values);

            Task<HttpResponseMessage> request = client.PostAsync(Url, content);
            try
            {
                string data = request.Result.Content.ReadAsStringAsync().Result;
                if (data.Contains("freigeschalten"))
                    Console.WriteLine("Logged in!");
                else
                    Console.WriteLine("Response: " + data);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Could not login...");
            }
        }
    }
}
