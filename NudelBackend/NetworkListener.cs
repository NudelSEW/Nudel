using JustConnect.Tcp;
using Newtonsoft.Json;
using Nudel.Networking.Requests;
using Nudel.Networking.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Nudel.Backend
{
    public class NetworkListener
    {
        private const int PORT = 8181;
        private Server server;
        private NudelService nudel;
        private JsonSerializerSettings jsonSettings;

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
            Request rawRequest = JsonConvert.DeserializeObject<Request>(data, jsonSettings);

            if (rawRequest is AuthenticationRequest)
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
