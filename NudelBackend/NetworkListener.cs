using JustConnect.Tcp;
using Newtonsoft.Json;
using Nudel.BusinessObjects;
using Nudel.Networking.Requests;
using Nudel.Networking.Responses;
using Nudel.Networking.Responses.Base;
using System;
using System.Net.Sockets;

namespace Nudel.Backend
{
    public class NetworkListener
    {
        private const int PORT = 8181;
        private Server server;
        private NudelService nudel;
        private readonly JsonSerializerSettings jsonSettings;

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

        public void Start()
        {
            server.Start();
        }

        public void Stop()
        {
            server.Stop();
        }

        public void Log(string data)
        {
            Console.WriteLine(data);
        }

        public void SendResponse(Response response, Socket clientSocket)
        {
            string responseString = JsonConvert.SerializeObject(response);
            server.Send(responseString, clientSocket);
        }

        private void ProcessRequest(string data, Socket clientSocket)
        {
            Object rawRequest = JsonConvert.DeserializeObject<Object>(data, jsonSettings);

            //Console.WriteLine($"Received Request of Type: {rawRequest.GetType()}");

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
        }
    }
}
