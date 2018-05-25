using JustConnect.Tcp;
using Newtonsoft.Json;
using Nudel.Networking.Requests;
using Nudel.Networking.Responses;
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
            nudel = new NudelService();
            jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

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
        private void ProcessRequest(string data, Socket clientSocket)
        {
            Object rawRequest = JsonConvert.DeserializeObject<Object>(data, jsonSettings);

            if (rawRequest is RegisterRequest)
            {
                RegisterRequest request = rawRequest as RegisterRequest;

                string sessionToken = nudel.Register(
                    request.Username,
                    request.Email,
                    request.Password,
                    request.FirstName,
                    request.LastName
                );

                string response = JsonConvert.SerializeObject(new AuthenticationResponse(sessionToken));
                server.Send(response, clientSocket);
            }
            else if (rawRequest is AuthenticationRequest)
            {
                AuthenticationRequest request = rawRequest as AuthenticationRequest;

                string sessionToken = nudel.Authenticate(request.UsernameOrEmail, request.Password);

                string response = JsonConvert.SerializeObject(new AuthenticationResponse(sessionToken));
                server.Send(response, clientSocket);
            }
            else if (rawRequest is CreateEventRequest)
            {
                CreateEventRequest request = rawRequest as CreateEventRequest;
                nudel.CreateEvent(request.Event);

            }
        }
    }
}
