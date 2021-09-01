using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class User
    {
        public string userId { get; set; }

        public string Name { get; set; }

        static List<User> userList { get; set; }

        public static void AddDummyUsers()
        {
            List<User> ulist = new List<User>()
            {
                new User {Name = "user1",userId= "u1" },
                new User {Name = "user2",userId= "u2" },
                new User {Name = "user3",userId= "u3" },
                new User {Name = "user4",userId= "u4" }
            };

            userList = ulist;
        }

        public void AddUser(User user)
        {
            userList.Add(user);
        }

        public User GetUserById(string userid)
        {
            return userList.FirstOrDefault(x => x.userId == userid);
        }

    }
}
