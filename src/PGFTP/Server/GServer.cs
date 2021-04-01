using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PGFTP.Server
{
    public class GServer
    {
        public Action<ClientObj> Listening;
        public List<ClientObj> Clients;

        private TcpListener _server;

        public GServer(string ip, int port)
        {
            _server = new TcpListener(IPAddress.Parse(ip), port);
            Clients = new List<ClientObj>();
        }

        public void Start()
        {
            _server.Start();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var client = new ClientObj(_server.AcceptTcpClient(), Clients.Count);
                    Clients.Add(client);

                    Task.Run((() =>
                    {
                        Listening(client);
                    }));
                }
            });
        }

        public void Stop()
        {
            foreach (var item in Clients)
                item.Close();

            _server.Stop();
        }
    }
}
