using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        public bool Test(string input)
        {
            Trace.WriteLine("Test method called.");
            return true;//I understand it should return smth more informational but not required in this test application
        }


        public UsersManagementService()
        {
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


        public Returnable GetUsersCount(UInt32 timestamp, string sessionKey, byte[] hash)
        {

            Trace.WriteLine("GetUsersCount method called.");
            var hashOk = CheckHash(timestamp + sessionKey, hash);
            if (!hashOk)
            {
                Trace.TraceError("Request checksum failed.");
                return new Returnable(false, "Request checksum failed.", new List<User>());
            }

            var isAuthed = IsAuthed(sessionKey);
            if (!isAuthed)
            {
                Trace.TraceError("Not authenticated request received.");
                return new Returnable(false, "Not authenticated request received.", new List<User>());
            }

            var ret = int.MinValue;
            using (var ctx = new Model1())
            {
                ret = ctx.Users.Count();
            }

            return new Returnable(ret);
        }

        public Returnable ListUsers(UInt32 timestamp, string sessionKey, byte[] hash, int page=0, string filterSet=null)
        {

            Trace.WriteLine("ListUsers method called.");
            var hashOk = CheckHash(timestamp + sessionKey + filterSet??"null" + page, hash);
            if (!hashOk)
            {
                Trace.TraceError("Request checksum failed.");
                return new Returnable(false, "Request checksum failed.",new List<User>());
            }

            var isAuthed = IsAuthed(sessionKey);
            if (!isAuthed)
            {
                Trace.TraceError("Not authenticated request received.");
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
            Trace.WriteLine("Login method called.");
            var sessionKey = Convert.ToBase64String(MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(
                    timestamp + login + password
                )));
            var hashOk= CheckHash(timestamp + login + password, hash);
            if (!hashOk)
            {
                Trace.TraceError("Request checksum failed.");
                return new Returnable(false, "Request checksum failed.", new List<User>());
            }

            var ctx = new Model1();

            var found = new List<User>(ctx.Users).FindAll(a => a.Login.Equals(login) && a.Password.Equals(password));
            if (found.Count==0)
            {
                Trace.TraceError("Invalid username or password provided.", LogLevel.Error);
                return new Returnable(false, "Invalid username or password provided.", new List<User>());
            }


            ctx.Users.Single(a => a.Password.Equals(password) && a.Login.Equals(login)).SessionKey = sessionKey;
            ctx.SaveChanges();
            Trace.WriteLine(login+" successfully logged in, session key \""+sessionKey+"\" stored in database.");

            return new Returnable(sessionKey);
        }

        public Returnable Logout(UInt32 timestamp, string sessionKey, byte[] hash)
        {
            Trace.WriteLine("Logout method called.");

            bool ret = false;
            var hashOk = CheckHash(timestamp + sessionKey, hash);
            if (!hashOk)
            {
                Trace.TraceError("Request checksum failed.");
                return new Returnable(false, "Request checksum failed.", new List<User>());
            }

            var ctx = new Model1();

            var single=ctx.Users.Single(a => a.SessionKey.Equals(sessionKey));
            single.SessionKey = null;
            Trace.WriteLine(single.Login + " successfully logged out.");
            ctx.SaveChanges();

            return new Returnable(ret);
        }

        public Returnable Register(UInt32 timestamp, User data, byte[] hash)
        {
            var hashok = CheckHash(timestamp.ToString()+data.ToString(), hash);
            if (!hashok)
            {
                Trace.TraceError("Request checksum failed.");
                return new Returnable(false, "Request checksum failed.", new List<User>());
            }
            //            throw new NotImplementedException();
            Trace.WriteLine("Register method called. Data: "+data.ToString());
            var ctx = new Model1();

            try
            {
                ctx.Users.Add(data);
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                return new Returnable(false, e.Message);
            }

            bool ret = true;
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
