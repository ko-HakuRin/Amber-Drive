using Newtonsoft.Json;
using Server.Types;
using System.Collections.Generic;
using System.IO;

namespace Server
{
    public class Db
    {
        public static List<Client> Clients { get; private set; }

        public static void Instance()
        {
            if (File.Exists("client.json"))
            {
                Clients = JsonConvert.DeserializeObject<List<Client>>(File.ReadAllText("client.json"));
                if (Clients == null)
                    Clients = new List<Client>();
            }
            else
            {
                File.Create("client.json");
                Clients = new List<Client>();
            }
        }

        public static bool Exist(Client client)
        {
            return Clients.Exists(e => e.Name == client.Name && e.Password == client.Password);
        }

        public static void SaveToFile()
        {
            File.WriteAllText("client.json", JsonConvert.SerializeObject(Clients));
        }
    }
}
