using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

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
        public bool Test(string input)
        {
            //Console.WriteLine("Test: {0}", input);
            return true;//I understand it should return smth more informational but not required in this test application
        }


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

        public dynamic ListUsers(string sessionKey, byte[] hash)
        {
            Program.Log("ListUsers method called.");
            var ret=new List<User>();
            return new Returnable(ret.GetType(),ret);
            ///throw new NotImplementedException();
        }

        public dynamic Login(string login, string password, byte[] hash)
        {
            Program.Log("Login method called.");
            string ret = "im session key";
            return new Returnable(ret.GetType(), ret);
        }

        public object Logout(string sessionKey, byte[] hash)
        {
            Program.Log("Logout method called.");

            bool ret = false;
            var localHash = MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(
                    sessionKey + GlobalVar.ClientSecret
                ));
            ret = localHash.SequenceEqual(hash);
            
            return new Returnable(ret.GetType(), ret);
        }

        public dynamic Register(User data, byte[] hash)
        {
            Program.Log("Register method called.");
            bool ret = false;
            return new Returnable(ret.GetType(), ret);
            
        }
    }
}
