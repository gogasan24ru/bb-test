using System.Collections.Generic;
using System.ServiceModel;

namespace backend2
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

        public Returnable ListUsers(string sessionKey, byte[] hash)
        {
            Program.Log("ListUsers method called.");
            var ret=new List<User>();
            return new Returnable(ret.GetType(),ret);
            ///throw new NotImplementedException();
        }

        public Returnable Login(string login, string password, byte[] hash)
        {
            Program.Log("Login method called.");
            string ret = "im session key";
            return new Returnable(ret.GetType(), ret);
        }

        public Returnable Logout(string sessionKey, byte[] hash)
        {
            Program.Log("Logout method called.");
            bool ret = false;
            return new Returnable(ret.GetType(), ret);
        }

        public Returnable Register(User data, byte[] hash)
        {
            Program.Log("Register method called.");
            bool ret = false;
            return new Returnable(ret.GetType(), ret);
            
        }
    }
}
