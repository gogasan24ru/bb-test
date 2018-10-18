using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Server
{
    //    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    //    111
    [ServiceBehavior]
    public class ServerService : IServerService
    {
        private string _sessionName;

        public ServerService()
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

        //public return MethodName(args?)
        //{
        //    smthing
        //}

    }
}
