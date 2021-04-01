using System;
using System.Windows;
using System.Windows.Input;
using PGFTP.Client;
using PGFTP.Enums;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Client.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Arg.Client = new GClient();
        }

        private void LoginBtn_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = $"{ LoginTBox.Text },{ PasswordTBox.Text },old";
                var response = Arg.Client.Connect(Arg.Ip, Arg.Port, data);
                if (response.Code == StatusCodes.NotAccount)
                {
                    MessageBox.Show(
                        "Введенное вами имя пользователя не принадлежит аккаунту. Проверьте свое имя пользователя и повторите попытку.",
                        "Ошибка Входа",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else if (response.Code == StatusCodes.OK)
                {
                    Arg.Username = LoginTBox.Text;
                    Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void RegisterBtn_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = $"{ LoginTBox.Text },{ PasswordTBox.Text },new";
                var response = Arg.Client.Connect(Arg.Ip, Arg.Port, data);
                if (response.Code == StatusCodes.ExistAccount)
                {
                    MessageBox.Show(
                        "Это имя пользователя уже занято. Попробуйте другое.",
                        "Ошибка Регистрации",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else if (response.Code == StatusCodes.OK)
                {
                    Arg.Username = LoginTBox.Text;
                    Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
