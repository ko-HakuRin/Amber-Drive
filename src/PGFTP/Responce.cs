using PGFTP.Enums;

namespace PGFTP
{
    public class Response
    {
        public StatusCodes Code { get; }

        public string Data { get; }

        public Response(StatusCodes code, string data)
        {
            Code = code;
            Data = data;
        }
    }
}
