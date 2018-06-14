using JustConnect.Tcp;
using Newtonsoft.Json;
using Nudel.Client.Model;
using Nudel.Networking.Requests.Base;
using Nudel.Networking.Responses;
using Nudel.Networking.Responses.Base;
using System;
using System.Net;
using TcpClient = JustConnect.Tcp.Client;

namespace Nudel.Client
{
    /// <summary>
    /// basics of ports, sockets and tcp settings/methods
    /// </summary>
    public static class NetworkListener
    {
        private const int PORT = 8181;
        private static TcpClient client;
        private static readonly JsonSerializerSettings jsonSettings;

#if DEBUG
        public static event ClientLogHandler Log;
#endif

        /// <summary>
        /// default constructor (static)
        /// </summary>
        static NetworkListener()
        {
            client = new TcpClient(PORT);

            jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

#if DEBUG
            client.Log += Log;
#endif
            
            client.Received += ProcessResponse;
        }

        /// <summary>
        /// starting a client connection and returning a message for the powershell
        /// </summary>
        public static void Start()
        {
            client.Connect(IPAddress.Loopback);

            Log?.Invoke("NetworkListener started");
        }

        /// <summary>
        /// stoping the client connection and returning a message for the powershell
        /// </summary>
        public static void Stop()
        {
            client.Disconnect();

            Log?.Invoke("NetworkListener stopped");
        }

        /// <summary>
        /// sending and converting a Request object into a json object, sending a message to the powershell
        /// </summary>
        /// <param name="request"> a request obejct given as a parameter </param>
        public static void SendRequest(Request request)
        {
            string requestString = JsonConvert.SerializeObject(request, jsonSettings);
            client.Send(requestString);

            Log?.Invoke("Sent request to backend");
        }

        /// <summary>
        /// processing the response via converting into a json object, depending on the type it gets different changes 
        /// </summary>
        /// <param name="data">sensible data for the functionality of the application</param>
        public static void ProcessResponse(string data)
        {
            object rawResponse = JsonConvert.DeserializeObject<object>(data, jsonSettings);

            Log?.Invoke($"Received Response of Type: {rawResponse.GetType()}");

            if (rawResponse is LoginRegisterResponse)
            {
                LoginRegisterResponse response = rawResponse as LoginRegisterResponse;

                MainModel.SessionToken = response.SessionToken;
            }
            else if (rawResponse is GetUserResponse)
            {
                GetUserResponse response = rawResponse as GetUserResponse;

                MainModel.User = response.User;
            }
        }
    }
}
