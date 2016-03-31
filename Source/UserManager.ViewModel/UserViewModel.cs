using System.Windows.Input;
using UserManager.Core.Common;
using UserManager.Core.Factory;
using UserManager.Model;
using UserManager.ViewModel.ViewManagement;

namespace UserManager.ViewModel
{
    public class UserViewModel : NotificationObject
    {
        #region Fields
        User user;
        string userValidator;
        #endregion

        #region Properties
        public User User
        {
            get { return user; }

            set
            {
                user = value;
                NotifyPropertyChanged("User");
            }
        }

        public string UserValidator
        {
            get { return userValidator; }

            set
            {
                userValidator = value;
                NotifyPropertyChanged("UserValidator");
            }
        }
        #endregion

        #region Constructor
        public UserViewModel()
        {
            UserValidator = null;
            user = UserManager.Model.User.CurrentUser;
        }
        #endregion

        #region Commands
        RelayCommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(Save, CanSave);

                return saveCommand;
            }
        }

        void Save()
        {
            UserList.SaveCurrentUsers();
            Ioc<MainViewModel>.Instance.ViewType = ViewType.UserList;
        }

        bool CanSave()
        {
            return UserValidator == null;
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
            Ioc<MainViewModel>.Instance.ViewType = ViewType.UserList;
        }

        bool CanCancel()
        {
            return true;
        }
        #endregion
    }
}
