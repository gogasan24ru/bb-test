using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_client.ServiceReference;

namespace web_client.Models
{
    public class ICUser
    {

        private UsersManagementServiceClient client;
        public ICUser()
        {
            client = new UsersManagementServiceClient("BasicHttpBinding_IServerService", "http://localhost:8000/svc");
        }

        public bool TestConnection()
        {
            try
            {
                return client.Test("nvm");
            }
            catch (Exception e)
            {
//                throw e;
            }
            return false;

        }

    }
}