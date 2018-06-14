using JustConnect.Tcp;
using Newtonsoft.Json;
using Nudel.BusinessObjects;
using Nudel.Networking.Requests;
using Nudel.Networking.Requests.Base;
using Nudel.Networking.Responses;
using Nudel.Networking.Responses.Base;
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
        /// sending a message respone via socket communication
        /// </summary>
        /// <param name="response"> variable responsable for the response </param>
        /// <param name="clientSocket"> the Client Socket </param>
        public void SendResponse(Response response, Socket clientSocket)
        {
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
            object rawRequest = JsonConvert.DeserializeObject<object>(data, jsonSettings);

            Log($"Received Request of Type: {rawRequest.GetType()}");

            if (rawRequest is RegisterRequest)
            {
                RegisterRequest request = rawRequest as RegisterRequest;

                NudelService nudel = new NudelService();

                string sessionToken = nudel.Register(
                    request.Username,
                    request.Email,
                    request.Password,
                    request.FirstName,
                    request.LastName
                );

                SendResponse(new LoginRegisterResponse(sessionToken), clientSocket);
            }
            else if (rawRequest is LoginRequest)
            {
                LoginRequest request = rawRequest as LoginRequest;

                NudelService nudel = new NudelService();

                string sessionToken = nudel.Login(request.UsernameOrEmail, request.Password);

                SendResponse(new LoginRegisterResponse(sessionToken), clientSocket);
            }
            else if (rawRequest is CreateEventRequest)
            {
                CreateEventRequest request = rawRequest as CreateEventRequest;

                NudelService nudel = new NudelService(request.SessionToken);

                Event @event = new Event
                {
                    Title = request.Title,
                    Description = request.Description,
                    Time = request.Time,
                    Location = request.Location,
                    Options = request.Options
                };

                nudel.CreateEvent(@event);
            }
            else if (rawRequest is GetUserRequest)
            {
                GetUserRequest request = rawRequest as GetUserRequest;

                NudelService nudel = new NudelService(request.SessionToken);

                SendResponse(new GetUserResponse(nudel.GetUser()), clientSocket);
            }
        }
    }
}
