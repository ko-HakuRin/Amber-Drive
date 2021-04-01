using System;
using System.Windows;

namespace Client
{
    public partial class App : Application
    {
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Arg.Client.Close();
        }
    }
}
