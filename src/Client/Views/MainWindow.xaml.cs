using Newtonsoft.Json;
using PGFTP.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FileInfo = System.IO.FileInfo;

namespace Client.Views
{
    public partial class MainWindow : Window
    {
        public static void GetFiles(string dicectory, Dictionary<string, int> filess)
        {
            foreach (var file in Directory.GetFiles(dicectory))
            {
                filess.Add(file, (int)new FileInfo(file).Length);
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            Directory.CreateDirectory(Arg.path);
            Arg.FileWatcher = new FileWatcher(Arg.path);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

            new LoginWindow().ShowDialog();

            NickTb.Text = Arg.Username;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    var req = Arg.Client.Get("getfilepaths", String.Empty);
                    var req2 = Arg.Client.Get("getfiles", String.Empty);
                    if (req.Code != StatusCodes.NotResponcing)
                    {
                        var dict = new Dictionary<string, int>();
                        var dict2 = new Dictionary<string, byte[]>();

                        dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(req.Data);
                        dict2 = JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(req2.Data);
                        foreach (var item in dict)
                        {
                            if (File.Exists($"{ Arg.path }\\{ item.Key }"))
                            {
                                if (new FileInfo($"{ Arg.path }\\{ item.Key }").Length != item.Value)
                                {
                                    File.WriteAllBytes($"{ Arg.path }\\{ item.Key }", dict2[item.Key]);
                                }
                            }
                            else
                            {
                                File.WriteAllBytes($"{ Arg.path }\\{ item.Key }", dict2[item.Key]);
                            }
                        }

                        var files = new Dictionary<string, int>();
                        GetFiles(Arg.path, files);
                        if (dict.Count < files.Count)
                        {
                            foreach (var file in files)
                            {
                                if (!dict.ContainsKey(Path.GetFileName(file.Key)))
                                {
                                    if (File.Exists($"{ file.Key }"))
                                    {

                                        var temp = new Dictionary<string, byte[]>()
                                        {
                                            {Path.GetFileName(file.Key), File.ReadAllBytes($"{file.Key}")}
                                        };

                                        Arg.Client.Post("createfile", JsonConvert.SerializeObject(temp));
                                    }
                                }
                            }
                        }
                    }

                }
                catch (FileFormatException exception)
                {
                    MessageBox.Show(exception.ToString());
                    throw;
                }
            });
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Sync_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var req = Arg.Client.Get("getfilepaths", String.Empty);
                    var req2 = Arg.Client.Get("getfiles", String.Empty);
                    if (req.Code != StatusCodes.NotResponcing)
                    {
                        var dict = new Dictionary<string, int>();
                        var dict2 = new Dictionary<string, byte[]>();

                        dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(req.Data);
                        dict2 = JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(req2.Data);
                        foreach (var item in dict)
                        {
                            if (File.Exists($"{ Arg.path }\\{ item.Key }"))
                            {
                                if (new FileInfo($"{ Arg.path }\\{ item.Key }").Length != item.Value)
                                {
                                    File.WriteAllBytes($"{ Arg.path }\\{ item.Key }", dict2[item.Key]);
                                }
                            }
                            else
                            {
                                File.WriteAllBytes($"{ Arg.path }\\{ item.Key }", dict2[item.Key]);
                            }
                        }

                        var files = new Dictionary<string, int>();
                        GetFiles(Arg.path, files);
                        if (dict.Count < files.Count)
                        {
                            foreach (var file in files)
                            {
                                if (!dict.ContainsKey(Path.GetFileName(file.Key)))
                                {
                                    if (File.Exists($"{ file.Key }"))
                                    {
                                        var temp = new Dictionary<string, byte[]>()
                                        {
                                            {Path.GetFileName(file.Key), File.ReadAllBytes($"{file.Key}")}
                                        };

                                        Arg.Client.Post("createfile", JsonConvert.SerializeObject(temp));
                                    }
                                }
                            }
                        }
                    }

                }
                catch (FileFormatException exception)
                {
                    MessageBox.Show(exception.ToString());
                    throw;
                }
            });
        }
    }
}
