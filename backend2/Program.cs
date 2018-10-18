using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace backend2
{
    
    class Program
    {
        public static List<Event> events;

        public static void Log(string msg)
        {
            events.Add(new Event(msg));
        }

        /// <summary>
        /// Start service
        /// </summary>
        /// <returns>Service entity. You should stop it, when shutting down app</returns>
        private static ServiceHost  RunService()
        {
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            //            smb.HttpGetBinding=new BasicHttpBinding();
            ServiceHost host = new ServiceHost(smb, new Uri("http://localhost:8000/ServerService"));
            host.Open();
//            Console.WriteLine("The service is ready.");
//            Console.WriteLine("Press <ENTER> to terminate service.");
//            Console.WriteLine();
//            Console.ReadLine();
//            host.Close();
            return host;
        }

        /// <summary>
        /// Generate demo database with hardcoded random values
        /// </summary>
        private static void DemoInit()
        {
            using (var ctx = new Model1())
            {
                List<String> Names = new List<string>() { "Михаил", "Виталий","Петр","Антон","Вадим","Валерий" };
                List<String> Surnames = new List<string>() { "Иванов", "Петров","Сидоров","Лукьяненко" };
                List<String> Passwords = new List<string>() { "cjhjrnsc", "zxj6tpmz","yd40gece","yekb9fyf" };//y
                //List<String> Logins = new List<string>() { "user1", "" };//autoincrement in demo code
                DateTime start = DateTime.Now;

                //ctx.init();
                var rnd=new Random();//4
                var generated=new List<User>();
                for (int i = 1; i < 31; i++)
                {
                    generated.Add(
                        new User()
                        {
                            Age= rnd.Next(18,100),
                            Login = "user"+i,
                            Name=Names[rnd.Next(0,Names.Count)],
                            Passport = new Passport() { Number = rnd.Next(0, 9999), Serial = rnd.Next(0, 999999) },
                            Password = Passwords[rnd.Next(0, Passwords.Count)],
                            Sex = sex.Male,//equal 0
                            Surname = Surnames[rnd.Next(0, Surnames.Count)]
                        }
                        );
                }


                ctx.Users.AddRange(generated);
                ctx.SaveChanges();

                Console.WriteLine("New demo database created in (millis) " +
                                  (DateTime.Now - start).TotalMilliseconds);
            }

        }

        static void Main(string[] args)
        {
//            Console.WriteLine(args[0]);
//            Console.ReadKey();
            ServiceHost host = null;
            events = new List<Event>();
            events.Add(new Event("Application start."));

            if (args.Length >= 1)//TODO: replace with advanced args parsing!
            {
                if (args[0] == "--help")
                {
                    throw new Exception("No help supplied yet :)");
                }

                if (args[0] == "--create-with-demo")
                {
                    DemoInit();
                }

                if (args[0] == "--drop-database")
                {
                    Log((new Model1()).Database.Delete()
                        ? "Successfully done"
                        : "Done with error (no database?)");
                }

                if (args[0] == "--create-database")
                {
                    CreateDatabase();
                }

                if (args[0] == "--run-service")
                {
                    CreateDatabase();
                    host = RunService();
                    if(host!=null)Log("service start, "+host.BaseAddresses[0]);
                }
            }

            if(host!=null)//Service started. wait until shutdown with ^c maybe.
                while (true)
                {
                    HandleEvents();
                    //print logs/events/hz
                }




            HandleEvents();
        }

        private static void CreateDatabase()
        {
            DateTime start = DateTime.Now;
            Log((new Model1()).Database.CreateIfNotExists()
                ? "New database created in (millis) " + (DateTime.Now - start).TotalMilliseconds.ToString()
                : "Database already exist");
        }

        private static void HandleEvents()
        {
            foreach (var a in events.FindAll(a=>!a.Displayed))
                Console.WriteLine(a.ToString());
        }
    }

    public class Event
    {
        private bool displayed;
        private string message;
        private DateTime timestamp;

        public Event(string msg)
        {
            timestamp = DateTime.Now;
            displayed = false;
            message = msg;
        }

        public bool Displayed => displayed;

        /// <summary>
        /// custom override with self modifying
        /// </summary>
        /// <returns>DateTime : Message</returns>
        public override string ToString()
        {
            displayed = true;
            return timestamp + " : " + message; //NOTTODO: +severity or smthing. Ignoring: possible over-engineering 
        }
    }
}
