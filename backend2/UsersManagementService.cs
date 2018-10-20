using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        public UInt32 timestamp { get; set; }
        [DataMember]
        public string CheckSum { get; set; }
        [DataMember]
        public Type DataTypeLocal { get; set; }
        [DataMember]
        public object Data { get; set; }

        #region used functions
//        public Dictionary<string, object> ToDictionary()//dummy walkaround
//        {
//            var ret = new Dictionary<string,object>();
//            ret.Add("timestamp", (UInt32)timestamp);
//            ret.Add("CheckSum", CheckSum);
//            ret.Add("DataTypeLocal", DataTypeLocal.Name);
//            ret.Add("Data", Data);
//            return ret;
//        }
//
//        public string ToJson()
//        {
//            return JsonConvert.SerializeObject(this.ToDictionary());
//        }
//
//        public Returnable()
//        {
//        }
//
//        public Returnable(string json)
//        {
//            Dictionary<string, string> dict = new Dictionary<string, string>(
//                JsonConvert.DeserializeObject<Dictionary<string, string>>(json)
//                );
//            //dict[""];
//            timestamp = UInt32.Parse(dict["timestamp"]);
////            DataTypeLocal = Type.GetType(dict["DataTypeLocal"]);
//            //TODO!!!!!!!!! looks bad, should think about...
//            switch (dict["DataTypeLocal"].ToLower())
//            {
//                case "boolean":
//                {
//                    DataTypeLocal = typeof(bool);
//                    bool temp = dict["Data"].ToLower().Equals("true");
//                    Data = temp;
//                    break;
//                }
//                case "string":
//                {
//                    DataTypeLocal = typeof(String);
//                    Data = dict["Data"];
//                    break;
//                }
//                //prepare for List<User>!!! TODO
//            }
//
//            if (Data == null ||
//                DataTypeLocal == null)
//                throw new Exception("Failed to parse JSON: "+json);
//
//
//            CheckSum = dict["CheckSum"];
//
//
//
//            int a = 1;
//        }
//
//        public Returnable(Type DT, object D)
//        {
//            timestamp = (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
//            Data = D;
//            DataTypeLocal = DT;
//            CheckSum = Convert.ToBase64String(MD5.Create().ComputeHash(
//                Encoding.UTF8.GetBytes(
//                    timestamp +
//                    GlobalVar.ServerSecret
//                )
//            ));//TODO confirm correct behavior
//
//            
//        }
    
        #endregion

        //        private bool CheckSumOk => MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(timestamp.ToString() + GlobalVar.ServerSecret)).Equals(CheckSum);

        //        public object ExtractData()//will it work??
        //        {
        //            //            var k=typeof(Convert.ChangeType(null,DataTypeLocal));
        //            return Data;//Convert.ChangeType(Data, DataTypeLocal);
        ////            return Data;
        //        }
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

        public Returnable ListUsers(string sessionKey, byte[] hash)
        {
            Program.Log("ListUsers method called.");
            var ret=new List<User>();
            return new Returnable()
            {
                timestamp = (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
                Data = ret,
                DataTypeLocal = ret.GetType()
            };
            ///return new Returnable(ret.GetType(),ret).ToJson();
            //throw new NotImplementedException();
        }

        public Returnable Login(string login, string password, byte[] hash)
        {
            Program.Log("Login method called.");
            string ret = "im session key";
            return new Returnable()
            {
                timestamp = (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
                Data = ret,
                DataTypeLocal = ret.GetType()
            };
            //return new Returnable(ret.GetType(), ret).ToJson();
        }

        public Returnable Logout(string sessionKey, byte[] hash)
        {
//            return false;
            Program.Log("Logout method called.");

            bool ret = false;
            var localHash = MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(
                    sessionKey + GlobalVar.ClientSecret
                ));
            ret = localHash.SequenceEqual(hash);
//            var returnable =new  Returnable(ret.GetType(), ret);
//            Program.Log("CS: "+returnable.CheckSum);//TODO comment or remove
            return new Returnable()
            {
                timestamp = (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
                Data = ret,
                DataTypeLocal = ret.GetType()
            };
            //return new Returnable(ret.GetType(), ret);
            //KnownType not worked. 
        }

        public Returnable Register(User data, byte[] hash)
        {
            Program.Log("Register method called.");
            bool ret = false;
            return new Returnable()
            {
                timestamp= (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
                Data=ret,
                DataTypeLocal=ret.GetType()
            };
            
        }
    }
}
