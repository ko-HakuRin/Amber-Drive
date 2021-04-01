using System;
using System.IO;
using System.Net.Sockets;

namespace PGFTP.Server
{
    public class ClientObj
    {
        private readonly TcpClient _client;
        private readonly StreamWriter _writer;
        private  readonly StreamReader _reader;

        public int Id { get; }

        public bool Connected
        {
            get
            {
                return _client.Connected;
            }
        }

        public void Close()
        {
            _client.Close();
        }

        public ClientObj(TcpClient client, int id)
        {
            _client = client;
            Id = id;
            _reader = new StreamReader(_client.GetStream());
            _writer = new StreamWriter(_client.GetStream())
            {
                AutoFlush = true
            };
        }

        public void Write(string json)
        {
            _writer.WriteLine(json);
        }

        public string Read()
        {
            var requestsInJson = _reader.ReadLine();
            if (requestsInJson != null)
            {
                return requestsInJson;
            }

            return String.Empty;
        }
    }
}
