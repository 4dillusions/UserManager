using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using UserManager.Core.Common;
using UserManager.Core.Data;
using UserManager.Core.Factory;
using UserManager.Model;
using UserManager.ViewModel.ViewManagement;

namespace UserManager.ViewModel
{
    public class UserListViewModel : NotificationObject
    {
        #region Fields
        AddressCity selectedAddressCity;
        ObservableCollection<AddressCity> addressCities;

        User selectedUser;
        ObservableCollection<User> users;

        UserFilter filter;
        #endregion

        #region Methods
        public UserListViewModel()
        {
            Reset();
        }

        void Reset()
        {
            filter = new UserFilter();

            AddressCities = new AddressCityList().Cities;
            SelectedAddressCity = AddressCities.FirstOrDefault();

            Users = new UserList(filter).Users;
            if (User.CurrentUser == null)
                SelectedUser = Users.FirstOrDefault();
            else
                SelectedUser = User.CurrentUser;
        }
        #endregion

        #region Properties
        public AddressCity SelectedAddressCity
        {
            get { return selectedAddressCity; }
            set
            {
                selectedAddressCity = value;
                filter.AddressCity = value.CityName;
                NotifyPropertyChanged("SelectedAddressCity");
            }
        }

        public ObservableCollection<AddressCity> AddressCities
        {
            get { return addressCities; }
            set
            {
                addressCities = value;
                NotifyPropertyChanged("AddressCities");
            }
        }

        public string TextInAll
        {
            get { return filter.TextInAll; }
            set
            {
                filter.TextInAll = value;
                NotifyPropertyChanged("TextInAll");
            }
        }

        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                User.CurrentUser = value;

                selectedUser = value;
                NotifyPropertyChanged("SelectedUser");
            }
        }

        public ObservableCollection<User> Users
        {
            get { return users; }
            set
            {
                users = value;
                NotifyPropertyChanged("Users");
            }
        }
        #endregion

        #region Commands
        RelayCommand findCommand;
        public ICommand FindCommand
        {
            get
            {
                if (findCommand == null)
                    findCommand = new RelayCommand(Find, CanFind);

                return findCommand;
            }
        }

        void Find()
        {
            Users = new UserList(filter).Users;
            SelectedUser = Users.FirstOrDefault();
        }

        bool CanFind()
        {
            return true;
        }

        RelayCommand editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (editCommand == null)
                    editCommand = new RelayCommand(Edit, CanEdit);

                return editCommand;
            }
        }

        void Edit()
        {
            if (SelectedUser != null)
            {
                UserList.CurrentUsers = Users;
                Ioc<MainViewModel>.Instance.ViewType = ViewType.User;
            }
        }

        bool CanEdit()
        {
            return true;
        }

        RelayCommand exportCommand;
        public ICommand ExportCommand
        {
            get
            {
                if (exportCommand == null)
                    exportCommand = new RelayCommand(Export, CanExport);

                return exportCommand;
            }
        }

        void Export()
        {
            IDataManager<User> dataManager = new JsonDataManager<User>();

            if (File.Exists(Constants.DataFilePath + Constants.JsonDataFileName))
                File.Delete(Constants.DataFilePath + Constants.JsonDataFileName);

            dataManager.Save(users.ToList(), Constants.DataFilePath + Constants.JsonDataFileName);

            if (File.Exists(Constants.DataFilePath + Constants.JsonDataFileName))
                MessageBox.Show("Data exported to Json file");
        }

        bool CanExport()
        {
            return true;
        }
        #endregion
    }
}
