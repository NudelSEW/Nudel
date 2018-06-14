using JustConnect.Tcp;
using Newtonsoft.Json;
using Nudel.Client.Model;
using Nudel.Networking;
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
            Response response = JsonConvert.DeserializeObject<Response>(data, jsonSettings);

            if (response != null)
            {
                Log?.Invoke($"Received Response of Type: {response.Type}");

                RequestResponseType type = response.Type;
                Result result;

                if (type == RequestResponseType.Register ||
                    type == RequestResponseType.Login)
                {
                    MainModel.SessionToken = response.Result.SessionToken;
                }
                else if (type == RequestResponseType.Logout)
                {
                }
                else if (type == RequestResponseType.CreateEvent)
                {
                }
                else if (type == RequestResponseType.EditEvent)
                {
                }
                else if (type == RequestResponseType.DeleteEvent)
                {
                }
                else if (type == RequestResponseType.FindEvent)
                {
                }
                else if (type == RequestResponseType.InviteToEvent)
                {
                }
                else if (type == RequestResponseType.AcceptEvent)
                {
                }
                else if (type == RequestResponseType.LeaveEvent)
                {
                }
                else if (type == RequestResponseType.AddComment)
                {
                }
                else if (type == RequestResponseType.DeleteComment)
                {
                }
                else if (type == RequestResponseType.FindCurrentUser)
                {
                    MainModel.User = response.Result.FoundUser;
                }
                else if (type == RequestResponseType.FindUser)
                {
                }
                else if (type == RequestResponseType.EditUser)
                {
                }
                else if (type == RequestResponseType.DeleteUser)
                {
                }
            }
        }
    }
}
