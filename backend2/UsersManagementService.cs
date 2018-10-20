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

    [DataContract]
    [Serializable()]
    public class Returnable
    {

        [DataMember]
        public UInt32 timestamp { get; set; }
        [DataMember]
        public string CheckSum { get; set; }
        [DataMember]
        public string Contains { get; set; }
        [DataMember]
        public bool Boolean { get; set; }

        [DataMember]
        public string StringData { get; set; }
        [DataMember]
        public List<User> UserList { get; set; }

        #region used functions
        public string ToJson() //TODO check behavior 
        {
            return JsonConvert.SerializeObject(this);
        }
        
        public Returnable()
        {
        }

        public Returnable(string sd) : this(false, sd) { }
        public Returnable(List<User> ld) : this(false, null, ld) { }


        public Returnable(bool booleanData=true, string stringData=null, List<User> listData=null)
        {
            timestamp = (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Boolean = booleanData;
            StringData = stringData;
            UserList = listData;
            CheckSum = Convert.ToBase64String(MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(
                    timestamp +
                    (Boolean?"true":"false") +
                    (StringData ?? "null") +
                    ((UserList == null) ? "null" : UserList.ToArray().ToString()) +
                    GlobalVar.ServerSecret
                )
            ));//TODO confirm correct behavior, add data
//            int a = 1;

        }

        #endregion

        //        private bool CheckSumOk => MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(timestamp.ToString() + GlobalVar.ServerSecret)).Equals(CheckSum);

        public dynamic ExtractData(out Type t)//TODO remove
        {
            if (StringData != null)
            {
                t = StringData.GetType();
                return StringData;
            }

            if (UserList != null)
            {
                t = UserList.GetType();
                return UserList;
            }
            t = Boolean.GetType();
            return Boolean;
        }
    }



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

        public Returnable ListUsers(UInt32 timestamp, string sessionKey, byte[] hash, string filterSet=null)
        //TODO "LIMIT A B"
        {
            var hashOk = CheckHash(timestamp + sessionKey + filterSet??"null", hash);
            Program.Log("ListUsers method called.");
            var ret=new List<User>();
            using (var ctx = new Model1())
            {
                ret = new List<User>(ctx.Users.Include("Passport"));
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
