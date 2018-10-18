using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using backend2;

namespace Server
{
    /// <summary>
    /// Dummy global vars
    /// </summary>
    static class GlobalVar
    {
        /// <summary>
        /// Used in data validating. Because no https :(
        /// </summary>
        public const string ClientSecret = "";
    }
    [ServiceContract]
    interface IUsersManagementService
    {
        [OperationContract]
        List<User> ListUsers();

        [OperationContract]
        bool Login(string login, string password);

        [OperationContract]
        bool Logout();


        [OperationContract]
        bool Register(User data);

        //[OperationContract]
        //return MethodName(args?);
        //..

    }
}
