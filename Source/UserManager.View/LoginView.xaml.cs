using System.Windows;
using System.Windows.Controls;
using UserManager.ViewModel;

namespace UserManager.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

            Password.PasswordChanged += (object sender, RoutedEventArgs e) => (DataContext as LoginViewModel).Password = Password.Password;
        }
    }
}
