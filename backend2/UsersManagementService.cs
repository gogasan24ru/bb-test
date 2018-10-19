using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using Newtonsoft.Json;

namespace backend2
{

    [DataContract]
    public class Returnable
    {

        [DataMember]
        public DateTime timestamp { get; set; }
        [DataMember]
        public byte[] CheckSum { get; set; }
        [DataMember]
        public Type DataTypeLocal { get; set; }
        [DataMember]
        public object Data { get; set; }

        public Dictionary<string, object> ToDictionary()//dummy walkaround
        {
            var ret = new Dictionary<string,object>();
            ret.Add("timestamp", timestamp);
            ret.Add("CheckSum", CheckSum);
            ret.Add("DataTypeLocal", DataTypeLocal);
            ret.Add("Data", Data);
            return ret;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this.ToDictionary());
        }

        public Returnable(Type DT, object D)
        {
            timestamp = DateTime.Now;
            Data = D;
            DataTypeLocal = DT;
            CheckSum = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(timestamp.ToString() + GlobalVar.ServerSecret));
        }

        //        private bool CheckSumOk => MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(timestamp.ToString() + GlobalVar.ServerSecret)).Equals(CheckSum);

        //        public object ExtractData()//will it work??
        //        {
        //            //            var k=typeof(Convert.ChangeType(null,DataTypeLocal));
        //            return Data;//Convert.ChangeType(Data, DataTypeLocal);
        ////            return Data;
        //        }
    }



    [ServiceKnownType(typeof(Returnable))]
    [KnownType(typeof(Returnable))]
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

        public string ListUsers(string sessionKey, byte[] hash)
        {
            Program.Log("ListUsers method called.");
            var ret=new List<User>();
            return new Returnable(ret.GetType(),ret).ToJson();
            //throw new NotImplementedException();
        }

        public string Login(string login, string password, byte[] hash)
        {
            Program.Log("Login method called.");
            string ret = "im session key";
            return new Returnable(ret.GetType(), ret).ToJson();
        }

        public string Logout(string sessionKey, byte[] hash)
        {
//            return false;
            Program.Log("Logout method called.");

            bool ret = false;
            var localHash = MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(
                    sessionKey + GlobalVar.ClientSecret
                ));
            ret = localHash.SequenceEqual(hash);
            return new Returnable(ret.GetType(), ret).ToJson();
            //return new Returnable(ret.GetType(), ret);
            //KnownType not worked. 
        }

        public string Register(User data, byte[] hash)
        {
            Program.Log("Register method called.");
            bool ret = false;
            return new Returnable(ret.GetType(), ret).ToJson();
            
        }
    }
}
