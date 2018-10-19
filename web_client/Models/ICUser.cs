using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;
using System.Web;
using web_client.ServiceReference;

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

        public void Logout(string sessionKey)
        {
            throw new NotImplementedException();
        }
    }
}