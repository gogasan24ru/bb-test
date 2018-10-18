using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using backend2;

namespace Server
{
    //    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    //    111
    [ServiceBehavior]
    public class UsersManagementService : IUsersManagementService
    {
        //public return MethodName(args?)
        //{
        //    smthing
        //}
        private string _sessionName;



        public UsersManagementService()
        {
            _sessionName = "upstart";
        }



        public void StoreSession(string sessionName)
        {
            _sessionName = sessionName;
        }

        public string GetSessionName()
        {
            return _sessionName;
        }

        public List<User> ListUsers()
        {
            Program.Log("ListUsers method called.");
            return new List<User>();
            ///throw new NotImplementedException();
        }

        public bool Login(string login, string password)
        {
            Program.Log("Login method called.");
            return false;
        }

        public bool Logout()
        {
            Program.Log("Logout method called.");
            return false;
        }

        public bool Register(User data)
        {
            Program.Log("Register method called.");
            return false;
        }
    }
}
