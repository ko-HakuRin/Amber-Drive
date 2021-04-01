using Newtonsoft.Json;
using PGFTP;
using PGFTP.Enums;
using PGFTP.Server;
using Server.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    class Program
    {
        public static string Splitter(string path)
        {
            var array = path.Split('\\');
            var result = "";
            for (int i = 1; i < array.Length; i++)
            {
                result = Path.Combine(result, array[i]);
            }

            return result;
        }

        public static void GetFiles(string dicectory, Dictionary<string, int> filess)
        {
            foreach (var file in Directory.GetFiles(dicectory))
            {
                filess.Add(Splitter(Splitter(file)), (int)new FileInfo(file).Length);
            }
        }

        static void Listening(ClientObj clientObj)
        {
            string Name2 = "";

            while (clientObj.Connected)
            {
                var req = JsonConvert.DeserializeObject<Request>(clientObj.Read());
                if (req == null)
                {
                    clientObj.Close();
                    break;
                }
                Console.WriteLine();
                Console.WriteLine($"[ Request { req.Type }]::[{ Name2 }]::[{ req.Command } ]");

                if (req.Command == "login")
                {
                    var logindata = req.Data.Split(',');
                    var client = new Client(logindata[0], logindata[1], clientObj.Id);
                    if (logindata[2] == "new")
                    {
                        if (Db.Exist(client))
                        {
                            clientObj.Write(JsonConvert.SerializeObject(new Response(StatusCodes.ExistAccount, String.Empty)));
                        }
                        else
                        {
                            Db.Clients.Add(client);
                            Directory.CreateDirectory(client.Dir);
                            Name2 = client.Name;
                            Console.WriteLine($"[ Register New Client Name:'{client.Name}' ]");
                            clientObj.Write(JsonConvert.SerializeObject(new Response(StatusCodes.OK, String.Empty)));
                        }
                    }
                    else if (logindata[2] == "old")
                    {
                        if (Db.Exist(client))
                        {
                            Console.WriteLine($" Login Old Client Name:'{ client.Name }' ]");
                            Name2 = client.Name;
                            clientObj.Write(JsonConvert.SerializeObject(new Response(StatusCodes.OK, String.Empty)));
                        }
                        else
                        {
                            clientObj.Write(JsonConvert.SerializeObject(new Response(StatusCodes.NotAccount, string.Empty)));
                        }
                    }
                    else
                    {
                        clientObj.Write(JsonConvert.SerializeObject(new Response(StatusCodes.BadRequest, String.Empty)));
                    }
                }
                else if (req.Command == "getfilepaths")
                {
                    var files = new Dictionary<string, int>();
                    var client = Db.Clients.Find(item => item.Name == Name2);
                    GetFiles($"{ client.Dir }", files);

                    clientObj.Write(JsonConvert.SerializeObject(new Response(StatusCodes.OK, JsonConvert.SerializeObject(files))));
                }
                else if (req.Command == "getfiles")
                {
                    var files = new Dictionary<string, byte[]>();
                    var client = Db.Clients.Find(item => item.Name == Name2);
                    var files2 = new Dictionary<string, int>();
                    GetFiles($"{ client.Dir }", files2);
                    foreach (var i in files2)
                    {
                        files.Add(i.Key, File.ReadAllBytes($@"Static\\{ client.Name }\\{ i.Key }"));
                    }
                    clientObj.Write(JsonConvert.SerializeObject(new Response(StatusCodes.OK, JsonConvert.SerializeObject(files))));
                }
                else if (req.Command == "createfile")
                {
                    var files = new Dictionary<string, byte[]>();
                    var client = Db.Clients.Find(item => item.Name == Name2);
                    files = JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(req.Data);
                    foreach (var file in files)
                    {
                        File.WriteAllBytes($"{client.Dir}\\{file.Key}", file.Value);
                    }
                }
                else if (req.Command == "deletefile")
                {
                    var files = new List<string>();
                    var client = Db.Clients.Find(item => item.Name == Name2);
                    files = JsonConvert.DeserializeObject<List<string>>(req.Data);
                    foreach (var file in files)
                    {
                        if (File.Exists($"{client.Dir}\\{file}"))
                        {
                            File.Delete($"{client.Dir}\\{file} ");
                        }
                    }
                }
                else if (req.Command == "renamefiles")
                {
                    var files = new Dictionary<string, string>();
                    var client = Db.Clients.Find(item => item.Name == Name2);
                    files = JsonConvert.DeserializeObject<Dictionary<string, string>>(req.Data);
                    foreach (var file in files)
                    {
                        if (File.Exists($"{client.Dir}\\{file.Key}"))
                        {
                            File.Move($"{client.Dir}\\{file.Key}", $"{client.Dir}\\{file.Value}");
                        }
                    }
                }
                else if (req.Command == "changefiles")
                {
                    var files = new Dictionary<string, byte[]>();
                    var client = Db.Clients.Find(item => item.Name == Name2);
                    files = JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(req.Data);
                    foreach (var file in files)
                    {
                        File.WriteAllBytes($"{client.Dir}\\{file.Key}", file.Value);
                    }
                }
            }
            Console.WriteLine($"[ Disconnect Client ID: { clientObj.Id } ]");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Db.Instance();
            Arg.Server = new GServer(Arg.Ip, Arg.Port)
            {
                Listening = Listening
            };

            Console.WriteLine($"[ Server is Started ]::[ { Arg.Ip }:{ Arg.Port } ]::[ { DateTime.UtcNow } ]");
            Arg.Server.Start();

            Console.ReadLine();

            Arg.Server.Stop();

            Db.SaveToFile();
        }
    }
}
