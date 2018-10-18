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
            throw new NotImplementedException();
        }

        public bool Login(string login, string password)
        {
            throw new NotImplementedException();
        }

        public bool Logout()
        {
            throw new NotImplementedException();
        }

        public bool Register(User data)
        {
            throw new NotImplementedException();
        }
    }
}
