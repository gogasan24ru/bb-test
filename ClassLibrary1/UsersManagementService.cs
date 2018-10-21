using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace ClassLibrary1
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
        public Logger logger { get; set; } 

        public void setLogger(Logger l)
        { logger = l;  }

        public bool Test(string input)
        {
            return true;//I understand it should return smth more informational but not required in this test application
        }


        public UsersManagementService()
        {
            //TODO need to be replaced with some native logging.
            logger = (Logger) GlobalVar.Misc[0];

            _sessionName = "upstart";
        }

//        ~UsersManagementService()
//        {
//            logger.HandleEvents();
//        }



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

            logger.Log("ListUsers method called.", LogLevel.Information);
            var hashOk = CheckHash(timestamp + sessionKey + filterSet??"null" + page, hash);
            if (!hashOk)
            {
                logger.Log("Request checksum failed.", LogLevel.Error);
                return new Returnable(false, "Request checksum failed.",new List<User>());
            }

            var isAuthed = IsAuthed(sessionKey);
            if (!isAuthed)
            {
                logger.Log("Not authenticated request received.", LogLevel.Error);
                return new Returnable(false, "Not authenticated request received.", new List<User>());
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
            logger.Log("Login method called.");
            var sessionKey = Convert.ToBase64String(MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(
                    timestamp + login + password
                )));
            var hashOk= CheckHash(timestamp + login + password, hash);
            if (!hashOk)
            {
                logger.Log("Request checksum failed.", LogLevel.Error);
                return new Returnable(false, "Request checksum failed.", new List<User>());
            }

            var ctx = new Model1();

            var found = new List<User>(ctx.Users).FindAll(a => a.Login.Equals(login) && a.Password.Equals(password));
            if (found.Count==0)
            {
                logger.Log("Invalid username or password provided.", LogLevel.Error);
                return new Returnable(false, "Invalid username or password provided.", new List<User>());
            }


            ctx.Users.Single(a => a.Password.Equals(password) && a.Login.Equals(login)).SessionKey = sessionKey;
            ctx.SaveChanges();
            logger.Log(login+" successfully logged in, session key \""+sessionKey+"\" stored in database.");

            return new Returnable(sessionKey);
        }

        public Returnable Logout(UInt32 timestamp, string sessionKey, byte[] hash)
        {
            logger.Log("Logout method called.");

            bool ret = false;
            var hashOk = CheckHash(timestamp + sessionKey, hash);
            if (!hashOk)
            {
                logger.Log("Request checksum failed.", LogLevel.Error);
                return new Returnable(false, "Request checksum failed.", new List<User>());
            }

            var ctx = new Model1();

            var single=ctx.Users.Single(a => a.SessionKey.Equals(sessionKey));
            single.SessionKey = null;
            logger.Log(single.Login + " successfully logged out.");
            ctx.SaveChanges();

            return new Returnable(ret);
        }

        public Returnable Register(UInt32 timestamp, User data, byte[] hash)
        {
            var hashok = CheckHash(timestamp.ToString(), hash);
            //            throw new NotImplementedException();
            logger.Log("Register method called.");
            bool ret = false;
            return new Returnable(ret);
        }

        private bool IsAuthed(string sessionKey)
        {
            return new List<User>((new Model1()).Users).FindAll(
                       a=>
                           (a.SessionKey!=null)?
                           a.SessionKey.Equals(sessionKey):
                               (false)
                       ).Count>0;
        }

        private bool CheckHash(string data, byte[] remoteHash)
        {
            return MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(data +
                GlobalVar.ClientSecret)).SequenceEqual(remoteHash);
        }
    }
}
