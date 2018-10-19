using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Web;
using web_client.ServiceReference;
using backend2;

namespace web_client.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ICUser
    {

        private UsersManagementServiceClient client;
        public ICUser()
        {
//            client = new UsersManagementServiceClient("BasicHttpBinding_IUsersManagementService", "http://localhost:8000/svc");
//            client = new UsersManagementServiceClient(new BasicHttpBinding(), "http://localhost:8000/svc");
//            BasicHttpBinding b = new BasicHttpBinding();
            
            client= new UsersManagementServiceClient("WSHttpBinding_IUsersManagementService1", "http://localhost:8000/UsersManagementService");
        }

        public bool TestConnection()
        {
                return client.Test("nvm");
        }

        public bool Logout(string sessionKey)
        {
            //            backend2.Returnable data = (backend2.Returnable)client.Logout(sessionKey,
            //                MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(sessionKey + GlobalVar.ClientSecret)));

            client.Logout(sessionKey,
                MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(sessionKey + GlobalVar.ClientSecret)));
//            var t= Convert.ChangeType(data.ExtractData(), data.GetType());
            return false;
        }
    }
}