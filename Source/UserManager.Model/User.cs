using System;
using System.IO;
using System.Linq;
using UserManager.Core.Common;
using UserManager.Core.Data;
using UserManager.Data;

namespace UserManager.Model
{
    [Serializable]
    public class User : NotificationObject
    {
        UserData data;

        public User()
        {
            data = new UserData();
        }

        public static bool IsUserExist(string loginName, string password)
        {
            if (File.Exists(Constants.DataFilePath + Constants.XmlDataFileName))
            {
                IDataManager<User> dataManager = new XmlDataManager<User>();
                var data = dataManager.Load(Constants.DataFilePath + Constants.XmlDataFileName);

                User user = data.Where(u => u.LoginName == loginName && u.Password == password).FirstOrDefault();
                return (user != null);
            }
            else
                throw new FileNotFoundException();
        }

        public override string ToString()
        {
            return UserId + LoginName + Password + FirstName + Surname + BirthDate + BirthPlace + AddressCity;
        }

        #region Properties
        public static User CurrentUser { get; set; }

        public int UserId
        {
            get { return data.UserId; }

            set
            {
                NotifyPropertyChanging("UserId");
                data.UserId = value;
                NotifyPropertyChanged("UserId");
            }
        }

        public string LoginName
        {
            get { return data.LoginName; }

            set
            {
                NotifyPropertyChanging("LoginName");
                data.LoginName = value;
                NotifyPropertyChanged("LoginName");
            }
        }

        public string Password
        {
            get { return data.Password; }

            set
            {
                NotifyPropertyChanging("Password");
                data.Password = value;
                NotifyPropertyChanged("Password");
            }
        }

        public string FirstName
        {
            get { return data.FirstName; }

            set
            {
                NotifyPropertyChanging("FirstName");
                data.FirstName = value;
                NotifyPropertyChanged("FirstName");
            }
        }

        public string Surname
        {
            get { return data.Surname; }

            set
            {
                NotifyPropertyChanging("Surname");
                data.Surname = value;
                NotifyPropertyChanged("Surname");
            }
        }

        public DateTime BirthDate
        {
            get { return data.BirthDate; }

            set
            {
                NotifyPropertyChanging("BirthDate");
                data.BirthDate = value;
                NotifyPropertyChanged("BirthDate");
            }
        }

        public string BirthPlace
        {
            get { return data.BirthPlace; }

            set
            {
                NotifyPropertyChanging("BirthPlace");
                data.BirthPlace = value;
                NotifyPropertyChanged("BirthPlace");
            }
        }

        public string AddressCity
        {
            get { return data.AddressCity; }

            set
            {
                NotifyPropertyChanging("AddressCity");
                data.AddressCity = value;
                NotifyPropertyChanged("AddressCity");
            }
        }
        #endregion
    }
}
