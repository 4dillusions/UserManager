using System.Windows;
using System.Windows.Input;
using UserManager.Core.Common;
using UserManager.Core.Factory;
using UserManager.Model;
using UserManager.ViewModel.ViewManagement;

namespace UserManager.ViewModel
{
    public class LoginViewModel : NotificationObject
    {
        #region Fields
        bool isCanLogin = true;

        string loginName;
        string password;
        #endregion

        #region Properties
        public string LoginName
        {
            get { return loginName; }
            set
            {
                loginName = value;
                NotifyPropertyChanged("LoginName");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                NotifyPropertyChanged("Password");
            }
        }
        #endregion

        #region Commands
        RelayCommand loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                    loginCommand = new RelayCommand(Login, CanLogin);

                return loginCommand;
            }
        }

        void Login()
        {
            try
            {
                isCanLogin = false;

                if (User.IsUserExist(LoginName, Password))
                    Ioc<MainViewModel>.Instance.ViewType = ViewType.UserList;
                else
                    MessageBox.Show("Wrong LoginName or Password!");

                isCanLogin = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Refresh error", ex.Message + "\n " + ex.InnerException);
            }
            finally
            {
                isCanLogin = true;
            }
        }

        bool CanLogin()
        {
            return isCanLogin;
        }

        RelayCommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new RelayCommand(Cancel, CanCancel);

                return cancelCommand;
            }
        }

        void Cancel()
        {
            Application.Current.Shutdown();
        }

        bool CanCancel()
        {
            return true;
        }
        #endregion
    }
}
