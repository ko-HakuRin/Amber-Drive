using PGFTP.Enums;

namespace PGFTP
{
    public class Request
    {
        public RequestTypes Type { get; }
        public string Command { get; }
        public string Data { get; }

        public Request(RequestTypes type, string command, string data)
        {
            Type = type;
            Command = command;
            Data = data;
        }
    }
}
