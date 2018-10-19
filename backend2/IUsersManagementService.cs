using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace backend2
{
    [DataContract]
    [KnownType(typeof(DateTime))]
    [KnownType(typeof(byte[]))]
    [KnownType(typeof(Type))]
    [KnownType(typeof(bool))]
    [KnownType(typeof(object))]
    public class Returnable
    {
        [DataMember]
        private DateTime timestamp;
        [DataMember]
        private byte[] CheckSum;
        [DataMember]
        private Type DataTypeLocal;
        [DataMember]
        private object Data;
        
        public Returnable(Type DT, object D)
        {
            timestamp=DateTime.Now;
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
    /// <summary>
    /// Dummy global vars
    /// </summary>
    static class GlobalVar
    {
        /// <summary>
        /// Used in data validating. Because no https :(
        /// </summary>
        public const string ClientSecret = "ImMegaClientSecret";
        public const string ServerSecret = "ImMegaServerSecret";
    }
    [ServiceContract]
    interface IUsersManagementService
    {

        [OperationContract]
        bool Test(string input);

        [OperationContract]
        dynamic ListUsers(string sessionKey, byte[] hash);

        [OperationContract]
        dynamic Login(string login, string password, byte[] hash);

        [OperationContract]
        object Logout(string sessionKey, byte[] hash);


        [OperationContract]
        dynamic Register(User data, byte[] hash);

        //[OperationContract]
        //return MethodName(args?);
        //..

    }
}
