using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UserManager.Core.Common;
using UserManager.Core.Data;

namespace UserManager.Model
{
    public class UserList
    {
        ObservableCollection<User> users;

        public ObservableCollection<User> Users
        {
            get { return users; }
            set { users = value; }
        }

        public static ObservableCollection<User> CurrentUsers { get; set; }

        /// <summary> Mocking </summary>
        public UserList()
        {
            Users = new ObservableCollection<User>();

            Users.Add(new User
            {
                UserId = 1,
                LoginName = "Albert",
                Password = "albert",
                FirstName = "Albert",
                Surname = "Einstein",
                BirthDate = new System.DateTime(2879, 3, 14),
                BirthPlace = "German",
                AddressCity = "Württemberg"
            });

            Users.Add(new User
            {
                UserId = 2,
                LoginName = "Zoltan",
                Password = "zoltan",
                FirstName = "Zoltan",
                Surname = "Kodaly",
                BirthDate = new System.DateTime(1882, 12, 16),
                BirthPlace = "Hungary",
                AddressCity = "Kecskemet"
            });

            Users.Add(new User
            {
                UserId = 3,
                LoginName = "Erno",
                Password = "erno",
                FirstName = "Erno",
                Surname = "Rubik",
                BirthDate = new System.DateTime(1944, 7, 13),
                BirthPlace = "Hungary",
                AddressCity = "Budapest"
            });
        }

        /// <summary> Usefilter without mock </summary>
        public UserList(UserFilter filter) : this()
        {
            if (File.Exists(Constants.DataFilePath + Constants.XmlDataFileName))
            {
                IDataManager<User> dataManager = new XmlDataManager<User>();
                var userList = new List<User>(dataManager.Load(Constants.DataFilePath + Constants.XmlDataFileName));

                if (filter.AddressCity == AddressCity.DefaultName)
                    users = new ObservableCollection<User>(userList.Where(u => u.ToString().Contains(filter.TextInAll)));
                else
                    users = new ObservableCollection<User>(userList.Where(u => u.AddressCity == filter.AddressCity && 
                        u.ToString().Contains(filter.TextInAll)));
            }
            else
                throw new FileNotFoundException();
        }

        public static void SaveCurrentUsers()
        {
            if (File.Exists(Constants.DataFilePath + Constants.XmlDataFileName))
            {
                IDataManager<User> dataManager = new XmlDataManager<User>();
                dataManager.Save(CurrentUsers.ToList(), Constants.DataFilePath + Constants.XmlDataFileName);
            }
        }
    }
}
