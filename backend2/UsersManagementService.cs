using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using Newtonsoft.Json;
using ClassLibrary1;

namespace backend2
{





    [ServiceKnownType(typeof(Returnable))]//useless
    [KnownType(typeof(Returnable))]//useless
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

        public Returnable ListUsers(UInt32 timestamp, string sessionKey, byte[] hash, int page=0, string filterSet=null)
        {

            Program.Log("ListUsers method called.",Program.LogLevel.Information);
            var hashOk = CheckHash(timestamp + sessionKey + filterSet??"null" + page, hash);
            if (!hashOk)
            {
                Program.Log("Request checksum failed.", Program.LogLevel.Error);
                return new Returnable(false, "Request checksum failed.",new List<User>());
            }
            var ret=new List<User>();
            using (var ctx = new Model1())
            {
                ret = new List<User>(ctx.Users.Include("Passport").OrderBy(a=> a.UserId) .Skip(10 * page).Take(10));
            }

            return new Returnable(ret);
        }

        public Returnable Login(UInt32 timestamp, string login, string password, byte[] hash)
        {
            var hashOk= CheckHash(timestamp + login + password, hash);
            Program.Log("Login method called.");
            string ret = "im session key";
            return new Returnable(ret);
        }

        public Returnable Logout(UInt32 timestamp, string sessionKey, byte[] hash)
        {
            Program.Log("Logout method called.");

            bool ret = false;
            var localHash = MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(timestamp+
                    sessionKey + GlobalVar.ClientSecret
                ));
            ret = CheckHash(timestamp+sessionKey, hash);

            return new Returnable(ret);
        }

        public Returnable Register(UInt32 timestamp, User data, byte[] hash)
        {
            var hashok = CheckHash(timestamp.ToString(), hash);
            //            throw new NotImplementedException();
            Program.Log("Register method called.");
            bool ret = false;
            return new Returnable(ret);
        }

        private bool CheckHash(string data, byte[] remoteHash)
        {
            return MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(data +
                GlobalVar.ClientSecret)).SequenceEqual(remoteHash);
        }
    }
}
