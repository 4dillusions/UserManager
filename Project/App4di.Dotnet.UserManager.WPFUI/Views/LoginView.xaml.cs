/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace App4di.Dotnet.UserManager.WPFUI;

/// <summary>
/// Interaction logic for LoginView.xaml
/// </summary>
public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();

        Password.PasswordChanged += (object sender, RoutedEventArgs e) =>
        {
            if (DataContext is LoginViewModel viewModel)
                viewModel.Password = Password.Password;
        };

        Loaded += LoginViewLoaded;
    }

    private void LoginViewLoaded(object sender, RoutedEventArgs e)
    {
        Dispatcher.BeginInvoke(
            DispatcherPriority.Input,
            new Action(() =>
            {
                LoginNameText.Focus();
                Keyboard.Focus(LoginNameText);
            }));
    }
}
