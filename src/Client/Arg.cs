using PGFTP.Client;
using System;

namespace Client
{
    public static class Arg
    {
        public static GClient Client;
        public static string Ip = "127.0.0.1";
        public static int Port = 43589;
        public static string Username;

        public static FileWatcher FileWatcher;
        public static string path = $"{ Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) }\\Desktop\\Amber Drive";
    }
}
