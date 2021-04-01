using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Types
{
    public class Client
    {
        public string Name { get; }
        public string Password { get; }

        public string Dir { get; }

        public Client(string name, string password, int id)
        {
            Name = name;
            Password = password;
            Dir = $"Static\\{name}";
        }
    }
}
