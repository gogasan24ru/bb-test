using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Web;
using web_client.ServiceReference;
using ClassLibrary1;

namespace web_client.Models
{
    public class UserRegModel
    {
        public string Username { get; set; }
    }
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

        public List<User> ListUsers(string sessionKey, int page = 0, string filterSet =null)
        {
            var cts = CurrentTimestamp();
            var Answer = client.ListUsers(cts, sessionKey, ComputeHash(cts + sessionKey + filterSet ?? "null" + page), page, filterSet);
            CheckAnswerChecksum(Answer);

            if (Answer.Boolean)
                return new List<User>(Answer.UserList);
            else
            {
//                return null;
                throw new Exception(Answer.StringData);
            }
        }

        private static void CheckAnswerChecksum(Returnable Answer)
        {
            if (!Answer.CheckSumOk)
            {
                throw new Exception("Received data have checksum mismatch.");
            }
        }

        public bool Logout(string sessionKey)
        {
            var cts = CurrentTimestamp();
            var Answer=  client.Logout(cts,sessionKey,ComputeHash(cts+sessionKey));
            return false;
        }

        public string Login(string login, string password)
        {
            var cts = CurrentTimestamp();
            //var Answer = client.ListUsers(cts, sessionKey, ComputeHash(cts + sessionKey + filterSet ?? "null" + page), page, filterSet);
            var Answer = client.Login(cts, login, password, ComputeHash(cts+login+password));
            CheckAnswerChecksum(Answer);

            if (Answer.Boolean)
                return Answer.StringData;
            else
            {
                throw new Exception(Answer.StringData);
            }
        }

        private UInt32 CurrentTimestamp()
        {
            return (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        private byte[] ComputeHash(string data)
        {
            return MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(
                    data +
                    GlobalVar.ClientSecret
                )
            );
        }
    }
}