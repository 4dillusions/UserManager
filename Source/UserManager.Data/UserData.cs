using System;

namespace UserManager.Data
{
    [Serializable]
    public class UserData
    {
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string AddressCity { get; set; }
    }
}
