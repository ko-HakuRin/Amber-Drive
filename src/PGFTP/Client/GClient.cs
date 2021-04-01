using Newtonsoft.Json;
using PGFTP.Enums;
using System;
using System.IO;
using System.Net.Sockets;

namespace PGFTP.Client
{
    public class GClient
    {
        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;

        public bool IsConnected
        {
            get { return _client.Connected; }
        }

        public void Close()
        {
            _client.Close();
        }

        public Response Connect(string ip, int port, string data)
        {
            _client = new TcpClient();
            _client.Connect(ip, port);

            _reader = new StreamReader(_client.GetStream());
            _writer = new StreamWriter(_client.GetStream())
            {
                AutoFlush = true
            };

            _writer.WriteLine(JsonConvert.SerializeObject(new Request(RequestTypes.Get, "login", data)));

            var responce = _reader.ReadLine();
            if (responce != null)
            {
                return JsonConvert.DeserializeObject<Response>(responce);
            }
            return new Response(StatusCodes.NotResponcing, String.Empty);
        }

        public void Post(string command, string data)
        {
            _writer.WriteLine(JsonConvert.SerializeObject(new Request(RequestTypes.Post, command, data)));
        }

        public Response Get(string command, string data)
        {
            _writer.WriteLine(JsonConvert.SerializeObject(new Request(RequestTypes.Get, command, data)));

            var responce = _reader.ReadLine();
            if (responce != null)
            {
                return JsonConvert.DeserializeObject<Response>(responce);
            }
            return new Response(StatusCodes.NotResponcing, String.Empty);
        }
    }
}
