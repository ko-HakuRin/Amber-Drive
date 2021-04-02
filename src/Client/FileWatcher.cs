using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Client
{
    public class FileWatcher
    {
        private readonly FileSystemWatcher _watcher;

        public FileWatcher(string path)
        {
            _watcher = new FileSystemWatcher(path, "*.*")
            {
                NotifyFilter = NotifyFilters.Attributes
                               | NotifyFilters.CreationTime
                               | NotifyFilters.DirectoryName
                               | NotifyFilters.FileName
                               | NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.Security
                               | NotifyFilters.Size
            };
            _watcher.Changed += OnChanged;
            _watcher.Deleted += OnDeleted;
            _watcher.Created += OnCreated;
            _watcher.Renamed += OnRenamed;
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Thread.Sleep(300);
            var list = new Dictionary<string, string>()
            {
                {e.OldName, e.Name}
            };
            Arg.Client.Post("renamefiles", JsonConvert.SerializeObject(list));
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(e.FullPath))
                return;
            Thread.Sleep(300);
            var dict = new Dictionary<string, byte[]>()
            {
                {e.Name, File.ReadAllBytes(e.FullPath)}
            };
            Arg.Client.Post("createfile", JsonConvert.SerializeObject(dict));
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(300);
            var list = new List<string>()
            {
                {e.Name}
            };
            Arg.Client.Post("deletefile", JsonConvert.SerializeObject(list));
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(e.FullPath))
                return;
            Thread.Sleep(300);
            var dict = new Dictionary<string, byte[]>()
            {
                {e.Name, File.ReadAllBytes(e.FullPath)}
            };
            Arg.Client.Post("changefiles", JsonConvert.SerializeObject(dict));
        }
    }
}
