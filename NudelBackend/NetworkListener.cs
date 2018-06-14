using JustConnect.Tcp;
using Newtonsoft.Json;
using Nudel.Networking;
using System;
using System.Net.Sockets;

namespace Nudel.Backend
{
    /// <summary>
    /// the networking component of the nudelService, used for every kind of communication
    /// </summary>
    public class NetworkListener
    {
        private const int PORT = 8181;
        private Server server;
        private NudelService nudel;
        private readonly JsonSerializerSettings jsonSettings;

        /// <summary>
        ///  constructor for instantiating a server object und jsonSettings
        /// </summary>
        public NetworkListener()
        {
            server = new Server(PORT);
            jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            server.Log += Log;
            server.Received += ProcessRequest;
        }

        /// <summary>
        /// starting the server
        /// </summary>
        public void Start()
        {
            server.Start();
        }

        /// <summary>
        /// stoping the server
        /// </summary>
        public void Stop()
        {
            server.Stop();
        }

        /// <summary>
        /// writing a string message on the console
        /// </summary>
        /// <param name="data"></param>
        public void Log(string data)
        {
            Console.WriteLine(data);
        }

        /// <summary>
        /// sending a message response via socket communication
        /// </summary>
        /// <param name="type"> type of the response </param>
        /// <param name="result"> result data being sent to with the response </param>
        /// <param name="clientSocket"> the Client Socket </param>
        public void SendResponse(RequestResponseType type, Result result, Socket clientSocket)
        {
            Response response = new Response { Type = type, Result = result };
            string responseString = JsonConvert.SerializeObject(response, jsonSettings);

            server.Send(responseString, clientSocket);
        }

        /// <summary>
        /// receiving and converting a request via json and sending a response
        /// </summary>
        /// <param name="data"> a string with sensible data about the application </param>
        /// <param name="clientSocket"> the client socket  </param>
        private void ProcessRequest(string data, Socket clientSocket)
        {
            Request request = JsonConvert.DeserializeObject<Request>(data, jsonSettings);

            if (request != null)
            {
                Log($"Received Response of Type: {request.Type}");

                RequestResponseType type = request.Type;
                Result result;

                if (type == RequestResponseType.Register)
                {
                    nudel = new NudelService();

                    result = nudel.Register(request.User);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.Login)
                {
                    nudel = new NudelService();

                    result = nudel.Login(request.User);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.Logout)
                {
                    result = nudel.Logout(request.SessionToken);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.CreateEvent)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.CreateEvent(request.Event);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.EditEvent)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.EditEvent(request.Event);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.DeleteEvent)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.DeleteEvent(request.Event);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.FindEvent)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.FindEvent(request.Event.ID);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.InviteToEvent)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.InviteToEvent(request.Event, request.User);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.AcceptEvent)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.AcceptEvent(request.Event);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.LeaveEvent)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.CreateEvent(request.Event);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.AddComment)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.AddComment(request.Event, request.Comment);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.DeleteComment)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.DeleteComment(request.Event, request.Comment);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.FindCurrentUser)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.FindCurrentUser();

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.FindUser)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.FindUser(request.User.ID);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.EditUser)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.EditUser(request.User);

                    SendResponse(type, result, clientSocket);
                }
                else if (type == RequestResponseType.DeleteUser)
                {
                    nudel = new NudelService(request.SessionToken);

                    result = nudel.DeleteUser(request.User);

                    SendResponse(type, result, clientSocket);
                }
            }
        }
    }
}
